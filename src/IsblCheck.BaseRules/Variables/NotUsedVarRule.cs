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
  /// Правило поиска неиспользованных переменных.
  /// </summary>
  internal class NotUsedVarRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "A003";

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
      /// Переменная хотя бы раз использовалась.
      /// </summary>
      public bool IsEverUsed { get; set; }

      /// <summary>
      /// Контекст.
      /// </summary>
      public IsblParser.VariableContext VariableContext { get; set; }
    }

    /// <summary>
    /// Поиск неиспользуемых переменных.
    /// </summary>
    private class NotUsedVarListener : IsblBaseListener
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
      /// Контекст объявленных переменных. 
      /// </summary>
      public readonly List<VariableDefinition> variableDefinitions = new List<VariableDefinition>();

      /// <summary>
      /// Вход в предложение объявления переменной.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterDeclareVariableStatement([NotNull] IsblParser.DeclareVariableStatementContext context)
      {
        var variableContext = context.GetChild(0) as IsblParser.VariableContext;
        if (variableContext == null)
          return;

        var variableName = variableContext.GetText();
        if (!globalContext.IsUserVariable(this.document, variableName))
          return;

        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));
        if (variableDefinition == null)
        {
          variableDefinition = new VariableDefinition
          {
            VariableName = variableName,
            IsEverUsed = false,
            VariableContext = variableContext
          };
          this.variableDefinitions.Add(variableDefinition);
        }
        else
          variableDefinition.VariableContext = variableContext;
       }

      /// <summary>
      /// Вход в предложение означивания.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterAssignStatement([NotNull]IsblParser.AssignStatementContext context)
      {
        // Первый потомок переменная.
        var variableContext = context.GetChild(0) as IsblParser.VariableContext;
        if (variableContext == null)
          return;

        // Второй потомок знак равенства или двоеточие.
        var eq = context.GetChild(1) as TerminalNodeImpl;
        if (eq == null ||
          (eq.Symbol.Type != IsblParser.EQ &&
          eq.Symbol.Type != IsblParser.COLON))
          return;

        var variableName = variableContext.GetText();
        if (!globalContext.IsUserVariable(this.document, variableName))
          return;
        
        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));
        if (variableDefinition == null)
        {
          variableDefinition = new VariableDefinition
          {
            VariableName = variableName,
            IsEverUsed = false,
            VariableContext = variableContext
          };
          this.variableDefinitions.Add(variableDefinition);
        }
        else
          variableDefinition.VariableContext = variableContext;
      }

      /// <summary>
      /// Вход в переменную.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterVariable([NotNull]IsblParser.VariableContext context)
      {
        var variableName = context.GetText();

        if (!globalContext.IsUserVariable(this.document, variableName))
          return;

        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));
        if (variableDefinition == null)
          return;
        
        if (variableDefinition.VariableContext != context)
          variableDefinition.IsEverUsed = true;
      }

      /// <summary>
      /// Конструктор
      /// </summary>
      /// <param name="globalContext">Контекст приложения</param>
      /// <param name="document">Документ</param>
      public NotUsedVarListener(IContext globalContext, IDocument document)
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
      new RuleInfo(typeof(NotUsedVarRule).Name, Resources.NotUsedVarRuleDescription), true);

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
      // Отчеты не проверяем. в них переменные могут использоваться для отчета и для шаблона.
      // TODO: Сделать менеджмент правил, перенести эту проверку туда.
      if (document.ComponentType == ComponentType.CommonReport ||
        document.ComponentType == ComponentType.IntegratedReport)
        return;

      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new NotUsedVarListener(context, document);
      walker.Walk(listener, tree);

      var notUsedVariableDefinitions = listener.variableDefinitions
        .Where(d => !d.IsEverUsed);
      foreach (var notUsedVariableDefinition in notUsedVariableDefinitions)
        report.AddWarning(Code, string.Format(Resources.NotUsedVariable, notUsedVariableDefinition.VariableName),
          document, notUsedVariableDefinition.VariableContext.Start.ToTextPosition());
    }

    #endregion
  }
}
