using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Правило "Использование несуществующей строки локализации".
  /// </summary>
  public class UsingNonExistingLocalizationStringRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "F003";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(UsingNonExistingLocalizationStringRule).Name, Resources.UsingNonExistingLocalizationStringRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info => info.Value;

    #endregion

    private class UsingNonExistingLocalizationStringListener : IsblBaseListener
    {
      public class UsingNonExistingLocalizationStringEntry
      {
        public ParserRuleContext Context { get; set; }
        public string LocalizationStringName { get; set; }
        public string LocalizationStringGroup { get; set; }
      }

      private readonly IContext globalContext;

      public List<UsingNonExistingLocalizationStringEntry> Entries { get; } = new List<UsingNonExistingLocalizationStringEntry>();

      #region IsblBaseListener

      public override void EnterFunction([NotNull] IsblParser.FunctionContext context)
      {
        var functionName = context.identifier().GetText();
        if (!functionName.Equals("LoadString", StringComparison.OrdinalIgnoreCase) &&
            !functionName.Equals("LoadStringFmt", StringComparison.OrdinalIgnoreCase))
          return;
        ProcessLoadStringFmtFunction(context);
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
        if (parameters.Length < 2)
          return;
        var name = GetStringOperandValue(parameters[0]);
        if (name == null)
          return;
        var group = GetStringOperandValue(parameters[1]);
        if (group == null)
          return;
        // Пропускаем системные строки локализации.
        if (group.StartsWith("SYSRES_", StringComparison.OrdinalIgnoreCase) ||
            group.StartsWith("DIRRES_", StringComparison.OrdinalIgnoreCase) ||
            group.Equals("ISBSYSDEV", StringComparison.OrdinalIgnoreCase))
          return;
        if (!IsLocalizationStringExist(group, name))
        {
          Entries.Add(new UsingNonExistingLocalizationStringEntry
          {
            Context = parameters[0],
            LocalizationStringGroup = group,
            LocalizationStringName = name
          });
        }
      }

      /// <summary>
      /// Проверить, существует ли строка локализации.
      /// </summary>
      /// <param name="group">Группа строк.</param>
      /// <param name="name">Имя строки.</param>
      /// <returns>Признак того, что такая строка локализации существует.</returns>
      private bool IsLocalizationStringExist(string group, string name)
      {
        return this.globalContext.Development.LocalizationStrings.Any(
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
        var operand = expression.operand();
        if (operand?.@string() == null)
          return null;
        return operand.@string().GetText().Trim('\'', '"');
      }

      /// <summary>
      /// Конструктор.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public UsingNonExistingLocalizationStringListener(IContext context)
      {
        this.globalContext = context;
      }
    }

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new UsingNonExistingLocalizationStringListener(context);
      walker.Walk(listener, tree);
      foreach (var entry in listener.Entries)
      {
        var description = string.Format(Resources.LocalizationStringNotFound, entry.LocalizationStringGroup, entry.LocalizationStringName);
        report.AddError(Code, description, document, entry.Context.GetTextPosition());
      }
    }

    #endregion

  }
}
