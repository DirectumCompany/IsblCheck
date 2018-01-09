using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Context.Development;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Правило для проверки форматной строки, передаваемой в функции Format(), LoadStringFmt().
  /// Правило учитывает варианты вызова функции как со строковой константой, так и с константой локализации.
  /// Если в функцию передаётся константа локализации, то анализируются значения константы для всех поддерживаемых языков.
  /// Обрабатываются форматы вызовов:
  ///   Format(LoadString(...); ArrayOf(...))
  ///   Format("..."; ArrayOf(...))
  ///   Формат(LoadString(...); Массив(...))
  ///   Формат("..."; Массив(...))
  ///   LoadStringFmt(...)
  /// </summary>
  public class IncorrectFormatStringRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила "Для описателя форматной строки не передан соответствующий аргумент".
    /// </summary>
    private const string FormatItemNotFoundRuleCode = "F005";
    /// <summary>
    /// Код правила "Для описателя форматной строки передан пустой параметр".
    /// </summary>
    private const string EmptyArgumentForFormatItemRuleCode = "F021";
    /// <summary>
    /// Код правила "Для переданного параметра не найден соответствующий описатель".
    /// </summary>
    private const string ArgumentForFormatItemNotFound = "F022";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(IncorrectFormatStringRule).Name, Resources.IncorrectFormatStringRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info { get { return info.Value; } }

    #endregion

    /// <summary>
    /// Поиск некорректных вызовов функций форматирования строки.
    /// </summary>
    private class IncorrectFormatStringListener : IsblBaseListener
    {
      public enum TemplateStringSource
      {
        StringConstant,
        LocalizationString
      }

      public class IncorrectFormatEntry<T>
      {
        public string TemplateString { get; set; }
        public TemplateStringSource TemplateStringSource { get; set; }
        public ParserRuleContext Context { get; set; }
        public T Data { get; set; }
      }

      /// <summary>
      /// Контекст приложения и разработки.
      /// </summary>
      private readonly IContext globalContext;

      /// <summary>
      /// Описатели шаблона строки, для которых не найдены соответствующие аргументы.
      /// </summary>
      public List<IncorrectFormatEntry<FormatItem>> FormatItemsWithoutArguments { get; } = new List<IncorrectFormatEntry<FormatItem>>();

      /// <summary>
      /// Описатели шаблона строки, для которых переданы пустые аргументы.
      /// </summary>
      public List<IncorrectFormatEntry<FormatItem>> FormatItemsWithEmptyArguments { get; } = new List<IncorrectFormatEntry<FormatItem>>();

      /// <summary>
      /// Аргументы, для которых в шаблоне строки не найдены описатели.
      /// </summary>
      public List<IncorrectFormatEntry<ParameterValue>> ArgumentsWithoutFormatItems { get; } = new List<IncorrectFormatEntry<ParameterValue>>();

      #region IsblBaseListener

      public override void EnterFunction([NotNull] IsblParser.FunctionContext context)
      {
        var functionName = context.identifier().GetText();
        if (functionName.Equals("Format", StringComparison.OrdinalIgnoreCase) ||
            functionName.Equals("Формат", StringComparison.OrdinalIgnoreCase))
        {
          ProcessFormatFunction(context);
        }
        else if (functionName.Equals("LoadStringFmt", StringComparison.OrdinalIgnoreCase))
        {
          ProcessLoadStringFmtFunction(context);
        }
      }

      #endregion

      /// <summary>
      /// Обработать вызов функции LoadStringFmt.
      /// </summary>
      /// <param name="function">Контекст вызова функции.</param>
      private void ProcessLoadStringFmtFunction(IsblParser.FunctionContext function)
      {
        if (function.parameterList() == null)
          return;
        var parameters = function.parameterList().expression();
        if (parameters.Length < 3)
          return;
        var templateLocalizationString = this.GetLocalizedString(function);
        if (templateLocalizationString == null)
          return;
        if (parameters[2].ChildCount != 1)
          return;
        var argsListOperand = parameters[2].operand();
        if (argsListOperand == null || argsListOperand.function() == null)
          return;
        var argsListFunctionName = argsListOperand.function().identifier().GetText();
        if (!argsListFunctionName.Equals("ArrayOf", StringComparison.OrdinalIgnoreCase) &&
            !argsListFunctionName.Equals("Массив", StringComparison.OrdinalIgnoreCase))
          return;
        var arrayOfFunctionParams = argsListOperand.function().parameterList();
        if (arrayOfFunctionParams == null)
          return;

        foreach (var localizationValue in templateLocalizationString.Values)
        {
          this.CheckTemplateString(localizationValue.Value, function.parameterList().expression().First(), 
            TemplateStringSource.LocalizationString, arrayOfFunctionParams);
        }
      }

      /// <summary>
      /// Обработать вызов функции Format.
      /// </summary>
      /// <param name="function">Контекст вызова функции.</param>
      private void ProcessFormatFunction(IsblParser.FunctionContext function)
      {
        if (function.parameterList() == null)
          return;
        var parameters = function.parameterList().expression();
        if (parameters.Length != 2)
          return;
        if (parameters[1].ChildCount != 1)
          return;
        var argsListOperand = parameters[1].operand();
        if (argsListOperand == null || argsListOperand.function() == null)
          return;
        var argsListFunctionName = argsListOperand.function().identifier().GetText();
        if (!argsListFunctionName.Equals("ArrayOf", StringComparison.OrdinalIgnoreCase) &&
           !argsListFunctionName.Equals("Массив", StringComparison.OrdinalIgnoreCase))
          return;
        var arrayOfFunctionParams = argsListOperand.function().parameterList();
        if (arrayOfFunctionParams == null)
          return;
        var operand = parameters[0].operand();
        if (operand == null)
          return;
        if (operand.@string() != null)
        {
          // Шаблон - строковая константа.
          var templateString = operand.@string().GetText();
          this.CheckTemplateString(templateString, operand.@string(), TemplateStringSource.StringConstant, arrayOfFunctionParams);
        }
        else if (operand.function() != null && 
          operand.function().identifier().GetText().Equals("LoadString", StringComparison.OrdinalIgnoreCase))
        {
          // Шаблон - функция LoadString.
          var templateLocalizationString = this.GetLocalizedString(operand.function());
          if (templateLocalizationString == null)
            return;
          foreach (var localizationValue in templateLocalizationString.Values)
          {
            this.CheckTemplateString(localizationValue.Value, operand.function(), TemplateStringSource.LocalizationString, arrayOfFunctionParams);
          }
        }
      }

      /// <summary>
      /// Получить строку локализации из контекста вызова функции LoadString.
      /// </summary>
      /// <param name="loadStringFunctionContext">Контекст вызова фунции LoadString.</param>
      /// <returns>Найденная строка локализации, или null, если строку локализации определить не удаётся.</returns>
      private LocalizationString GetLocalizedString(IsblParser.FunctionContext loadStringFunctionContext)
      {
        var loadStringParamsContext = loadStringFunctionContext.parameterList();
        if (loadStringParamsContext == null)
          return null;
        var parameters = loadStringParamsContext.expression();
        if (parameters.Length < 2)
          return null;
        var name = GetStringOperandValue(parameters[0]);
        if (name == null)
          return null;
        var group = GetStringOperandValue(parameters[1]);
        if (group == null)
          return null;
        return this.globalContext.Development.LocalizationStrings.FirstOrDefault(
          s => s.Group.Equals(group, StringComparison.OrdinalIgnoreCase) &&
               s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
      }

      /// <summary>
      /// Получить строковую константу из выражения.
      /// </summary>
      /// <param name="expression">Выражение.</param>
      /// <returns>Строковая константа, либо null, если выражение не является строкой.</returns>
      private static string GetStringOperandValue(IsblParser.ExpressionContext expression)
      {
        return expression.operand()?.@string()?.GetText().Trim('\'', '"');
      }

      private void CheckTemplateString(string templateString, ParserRuleContext stringSourceContext,
        TemplateStringSource templateStringSource, IsblParser.ParameterListContext paramsOfArrayFunction)
      {
        var templateParser = new TemplateStringParser(templateString);
        var paramsParser = new ParameterListParser(paramsOfArrayFunction);
        templateParser.Parse();
        paramsParser.Parse();
        foreach(var formatItem in templateParser.FormatItems)
        {
          var foundArg = paramsParser.ParameterValues.FirstOrDefault(a => a.Index == formatItem.Index);
          if(foundArg == null)
          {
            this.FormatItemsWithoutArguments.Add(new IncorrectFormatEntry<FormatItem>
            {
              TemplateString = templateString,
              Data = formatItem,
              Context = stringSourceContext,
              TemplateStringSource = templateStringSource
            });
          }
          else if(foundArg.IsEmpty)
          {
            this.FormatItemsWithEmptyArguments.Add(new IncorrectFormatEntry<FormatItem>
            {
              TemplateString = templateString,
              Data = formatItem,
              Context = stringSourceContext,
              TemplateStringSource = templateStringSource
            });
          }
        }
        foreach(var parameter in paramsParser.ParameterValues.Where(p => !p.IsEmpty))
        {
          if(!templateParser.FormatItems.Any(f => f.Index == parameter.Index))
          {
            this.ArgumentsWithoutFormatItems.Add(new IncorrectFormatEntry<ParameterValue>
            {
              TemplateString = templateString,
              Data = parameter,
              Context = parameter.Context,
              TemplateStringSource = templateStringSource
            });
          }
        }
      }

      /// <summary>
      /// Конструктор.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public IncorrectFormatStringListener(IContext context)
      {
        this.globalContext = context;
      }
    }

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new IncorrectFormatStringListener(context);
      walker.Walk(listener, tree);
      foreach (var entry in listener.FormatItemsWithoutArguments)
      {
        string description = string.Format(Resources.ArgumentForTemplateNotFound, 
          entry.Data.Text, entry.Data.Index, entry.TemplateString);
        var position = entry.Context.GetTextPosition();
        if(entry.TemplateStringSource == IncorrectFormatStringListener.TemplateStringSource.StringConstant)
        {
          // Подсвечиваем некорректный описатель прямо в строковой константе.
          var startIndex = position.StartIndex;
          position.StartIndex = startIndex + entry.Data.Pos.StartIndex;
          position.EndIndex = startIndex + entry.Data.Pos.EndIndex;
          position.Column = entry.Data.Pos.Line == 0 ? position.Column + entry.Data.Pos.Column : entry.Data.Pos.Column;
          position.Line += entry.Data.Pos.Line;
        }
        report.AddError(FormatItemNotFoundRuleCode, description, document, position);
      }
      foreach (var entry in listener.FormatItemsWithEmptyArguments)
      {
        string description = string.Format(Resources.EmptyFormatArgument,
          entry.Data.Text, entry.Data.Index, entry.TemplateString);
        var position = entry.Context.GetTextPosition();
        if (entry.TemplateStringSource == IncorrectFormatStringListener.TemplateStringSource.StringConstant)
        {
          // Подсвечиваем некорректный описатель прямо в строковой константе.
          var startIndex = position.StartIndex;
          position.StartIndex = startIndex + entry.Data.Pos.StartIndex;
          position.EndIndex = startIndex + entry.Data.Pos.EndIndex;
          position.Column = entry.Data.Pos.Line == 0 ? position.Column + entry.Data.Pos.Column : entry.Data.Pos.Column;
          position.Line += entry.Data.Pos.Line;
        }
        report.AddWarning(EmptyArgumentForFormatItemRuleCode, description, document, position);
      }
      foreach (var incorrectFormatEntry in listener.ArgumentsWithoutFormatItems)
      {
        string description = string.Format(Resources.RedutantFormatArgument,
          incorrectFormatEntry.Data.Text, incorrectFormatEntry.Data.Index, incorrectFormatEntry.TemplateString);
        report.AddWarning(ArgumentForFormatItemNotFound, description, document, incorrectFormatEntry.Context.GetTextPosition());
      }
    }

    #endregion

  }
}
