using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;

namespace IsblCheck.BaseRules.Variables
{
  /// <summary>
  /// Правило поиска переопределенных переменных (переменные, которые определялись несколько раз до их использования).
  /// </summary>
  internal class UsingRedefinedVarRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "A002";

    #endregion

    #region Вложенные классы

    /// <summary>
    /// Определение переменной.
    /// </summary>
    private class VariableDefinition
    {
      /// <summary>
      /// Имя переменной.
      /// </summary>
      public string VariableName { get; set; }

      /// <summary>
      /// Переменная использовалась.
      /// </summary>
      public bool IsUsed { get; set; }

      /// <summary>
      /// Переменная переопределена.
      /// </summary>
      public bool IsRedefined { get; set; }

      /// <summary>
      /// Контекст переменной.
      /// </summary>
      public IsblParser.VariableContext VariableContext { get; set; }

      /// <summary>
      /// Блок предложений.
      /// </summary>
      public IsblParser.StatementBlockContext StatementBlock { get; set; }

      /// <summary>
      /// Присваиваемое выражение.
      /// </summary>
      public string AssignExpression { get; set; }
    }

    /// <summary>
    /// Поиск переопределенных до их использования переменных.
    /// </summary>
    private class UsingRedefinedVariabeleListener : IsblBaseListener
    {
      /// <summary>
      /// Контекст приложения и разработки.
      /// </summary>
      private readonly IContext globalContext;

      /// <summary>
      /// Текущий документ.
      /// </summary>
      private readonly IDocument document;

      /// <summary>
      /// Определения переменных.
      /// </summary>
      public readonly List<VariableDefinition> variableDefinitions = new List<VariableDefinition>();

      /// <summary>
      /// Получить родительский блок предложений.
      /// </summary>
      /// <param name="ruleNode">Узел правил.</param>
      /// <returns>Блок предложений.</returns>
      private static IsblParser.StatementBlockContext GetParentStatementBlock(IRuleNode ruleNode)
      {
        var parent = ruleNode.Parent;
        while (parent != null)
        {
          if (parent is IsblParser.StatementBlockContext)
            break;
          parent = parent.Parent;
        }
        return parent as IsblParser.StatementBlockContext;
      }

      /// <summary>
      /// Вход в переменную.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterVariable([NotNull]IsblParser.VariableContext context)
      {
        var variableName = context.GetText();

        // Проверить, что переменная не константа
        if (!globalContext.IsUserVariable(this.document, variableName))
          return;

        // Проверяем что переменная не левый операнд предложения означивания.
        if (context.Parent is IsblParser.AssignStatementContext assignStatement)
        {
          var leftOperand = assignStatement.GetChild(0) as IsblParser.VariableContext;
          var eq = assignStatement.GetChild(1) as TerminalNodeImpl;
          if (leftOperand == context && eq != null && eq.Symbol.Type == IsblParser.EQ)
            return;
        }

        // Проверяем что переменная не левый операнд предложения объявления.
        if (context.Parent is IsblParser.DeclareVariableStatementContext declareStatement)
        {
          var leftOperand = declareStatement.GetChild(0) as IsblParser.VariableContext;
          if (leftOperand == context)
            return;
        }

        var variableDefinition = this.variableDefinitions
          .LastOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));
        if (variableDefinition == null)
          return;

        variableDefinition.IsUsed = true;
      }

      /// <summary>
      /// Выход из предложения означивания.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void ExitDeclareVariableStatement([NotNull] IsblParser.DeclareVariableStatementContext context)
      {
        // Первый потомок переменная.
        var variableContext = context.GetChild(0) as IsblParser.VariableContext;
        if (variableContext == null)
          return;

        // Проверяем что это не константа.
        var variableName = variableContext.GetText();
        if (!globalContext.IsUserVariable(this.document, variableName))
          return;

        // Помечаем предыдущую переменную на этом же уровне как переопределенную.
        var parentStatementBlock = GetParentStatementBlock(context);
        var variableDefinition = this.variableDefinitions
          .LastOrDefault(d =>
            d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase) && 
            d.StatementBlock == parentStatementBlock);
        if (variableDefinition != null)
          variableDefinition.IsRedefined = true;

        // Добавляем новую переменную в список.
        variableDefinition = new VariableDefinition
        {
          VariableName = variableName,
          VariableContext = variableContext,
          StatementBlock = parentStatementBlock,
          AssignExpression = string.Empty
        };
        this.variableDefinitions.Add(variableDefinition);
      }

      /// <summary>
      /// Выход из предложения означивания.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void ExitAssignStatement([NotNull]IsblParser.AssignStatementContext context)
      {
        // Первый потомок переменная.
        var variableContext = context.GetChild(0) as IsblParser.VariableContext;
        if (variableContext == null)
          return;

        // Второй потомок знак точка.
        var eq = context.GetChild(1) as TerminalNodeImpl;
        if (eq == null || eq.Symbol.Type != IsblParser.EQ)
          return;

        // Проверяем что это не константа.
        var variableName = variableContext.GetText();
        if (!globalContext.IsUserVariable(this.document, variableName))
          return;

        // Помечаем предыдущую переменную на этом же уровне как переопределенную.
        var parentStatementBlock = GetParentStatementBlock(context);
        var variableDefinition = this.variableDefinitions
          .LastOrDefault(d =>
            d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase) &&
            d.StatementBlock == parentStatementBlock);
        if (variableDefinition != null)
          variableDefinition.IsRedefined = true;

        // Добавляем новую переменную в список.
        variableDefinition = new VariableDefinition
        {
          VariableName = variableName,
          VariableContext = variableContext,
          StatementBlock = parentStatementBlock,
          AssignExpression=context.expression().GetText()
        };
        this.variableDefinitions.Add(variableDefinition);
      }

      /// <summary>
      /// Конструктор
      /// </summary>
      /// <param name="globalContext">Контекст приложения</param>
      /// <param name="document">Документ</param>
      public UsingRedefinedVariabeleListener(IContext globalContext, IDocument document)
      {
        this.globalContext = globalContext;
        this.document = document;
      }
    }

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() => 
      new RuleInfo(typeof(UsingRedefinedVarRule).Name, Resources.UsingRedefindedVarRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info => info.Value;

    #endregion

    #region Методы

    /// <summary>
    /// Применить правило.
    /// </summary>
    /// <param name="report">Отчет.</param>
    /// <param name="document">Документ.</param>
    /// <param name="context">Контекст.</param>
    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new UsingRedefinedVariabeleListener(context, document);
      walker.Walk(listener, tree);

      var definitions = listener.variableDefinitions
        .Where(d => d.IsRedefined && !d.IsUsed && 
        !d.AssignExpression.Equals("null",StringComparison.OrdinalIgnoreCase) &&
        !d.AssignExpression.Equals("nil",StringComparison.OrdinalIgnoreCase));
      foreach (var definition in definitions)
        report.AddWarning(Code, string.Format(Resources.VariableIsRedefinedBeforeUsing, definition.VariableName),
          document, definition.VariableContext.Start.ToTextPosition());
    }

    #endregion
  }
}
