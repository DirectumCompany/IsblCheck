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
  /// Правило поиска необъявленных переменных.
  /// </summary>
  internal class UsingNotAssignedVarRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "A001";

    #endregion

    #region Внутренние классы

    /// <summary>
    /// Определение переменной.
    /// </summary>
    private class VariableDefiniton
    {
      /// <summary>
      /// Имя переменной.
      /// </summary>
      public string VariableName { get; set; }

      /// <summary>
      /// Переменная переобъявлена.
      /// </summary>
      public bool IsReinitialized { get; set; }

      /// <summary>
      /// Блок предложений.
      /// </summary>
      public IsblParser.StatementBlockContext StatementBlock { get; set; }
    }

    /// <summary>
    /// Поиск необъявленных переменных.
    /// </summary>
    private class NotAssignedVariablesListener : IsblBaseListener
    {
      /// <summary>
      /// Контекст приложения и разработки
      /// </summary>
      private readonly IContext globalContext;

      /// <summary>
      /// Текущий документ.
      /// </summary>
      private readonly IDocument document;

      /// <summary>
      /// Контекст объявленных переменных.
      /// </summary>
      private readonly List<VariableDefiniton> variableDefinitions = new List<VariableDefiniton>();

      /// <summary>
      /// Необъявленные переменные.
      /// </summary>
      public List<IsblParser.VariableContext> NotAssignedVariables { get; }

      /// <summary>
      /// Неопределенные переменные.
      /// </summary>
      public List<IsblParser.VariableContext> UncertainVariables { get; }

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
      /// Вход в предложение объявления переменной.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterDeclareVariableStatement([NotNull] IsblParser.DeclareVariableStatementContext context)
      {
        // Первый потомок переменная.
        var variable = context.GetChild(0) as IsblParser.VariableContext;
        if (variable == null)
          return;

        var variableName = variable.GetText();
        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));

        if (variableDefinition == null)
        {
          variableDefinition = new VariableDefiniton
          {
            VariableName = variableName,
            IsReinitialized = false,
            StatementBlock = GetParentStatementBlock(context)
          };
          this.variableDefinitions.Add(variableDefinition);
        }
        else if (variableDefinition.StatementBlock == null)
        {
          variableDefinition.IsReinitialized = true;
          variableDefinition.StatementBlock = GetParentStatementBlock(context);
        }
      }

      /// <summary>
      /// Вход в предложение означивания.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterAssignStatement([NotNull] IsblParser.AssignStatementContext context)
      {
        // Первый потомок переменная.
        var variable = context.GetChild(0) as IsblParser.VariableContext;
        if (variable == null)
          return;

        // Второй потомок знак равенства или двоеточие.
        var eq = context.GetChild(1) as TerminalNodeImpl;
        if (eq == null ||
          (eq.Symbol.Type != IsblParser.EQ && 
          eq.Symbol.Type != IsblParser.COLON))
          return;

        var variableName = variable.GetText();
        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));

        if (variableDefinition == null)
        {
          variableDefinition = new VariableDefiniton
          {
            VariableName = variableName,
            IsReinitialized = false,
            StatementBlock = GetParentStatementBlock(context)
          };
          this.variableDefinitions.Add(variableDefinition);
        }
        else if (variableDefinition.StatementBlock == null)
        {
          variableDefinition.IsReinitialized = true;
          variableDefinition.StatementBlock = GetParentStatementBlock(context);
        }
      }

      /// <summary>
      /// Вход в foreach.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterForeachStatement([NotNull] IsblParser.ForeachStatementContext context)
      {
        var variable = context.GetChild(1) as IsblParser.VariableContext;

        var variableName = variable.GetText();
        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));

        if (variableDefinition == null)
        {
          variableDefinition = new VariableDefiniton
          {
            VariableName = variableName,
            IsReinitialized = false,
            StatementBlock = context.GetChild(4) as IsblParser.StatementBlockContext
          };
          this.variableDefinitions.Add(variableDefinition);
        }
        else if (variableDefinition.StatementBlock == null)
        {
          variableDefinition.IsReinitialized = true;
          variableDefinition.StatementBlock = context.GetChild(4) as IsblParser.StatementBlockContext;
        }
      }

      /// <summary>
      /// Выход из списка предложений.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void ExitStatementBlock([NotNull] IsblParser.StatementBlockContext context)
      {
        var variableDefinitions = this.variableDefinitions
          .Where(d => d.StatementBlock == context)
          .ToList();
        foreach(var variableDefinition in variableDefinitions)
        {
          if (variableDefinition.IsReinitialized)
          {
            variableDefinition.IsReinitialized = false;

            // Переводим переменную на блок выше.
            // TODO: Тут не ясно что делать с ForeachStatementContext и WhileStatementContext
            // Они могут и не объявить переменную.
            var parent = context.Parent;
            if (parent != null && (
              parent is IsblParser.TryStatementContext || 
              parent is IsblParser.IfStatementContext ||
              parent is IsblParser.ForeachStatementContext ||
              parent is IsblParser.WhileStatementContext))
              variableDefinition.StatementBlock = parent.Parent as IsblParser.StatementBlockContext;
            else
              variableDefinition.StatementBlock = null;
          }
          else
            variableDefinition.StatementBlock = null;
        }
      }

      /// <summary>
      /// Вход в переменную.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterVariable([NotNull] IsblParser.VariableContext context)
      {
        var variableName = context.GetText();

        // Проверить, что переменная не константа
        if (!globalContext.IsUserVariable(this.document, variableName))
          return;

        var variableDefinition = this.variableDefinitions
          .FirstOrDefault(d => d.VariableName.Equals(variableName, StringComparison.OrdinalIgnoreCase));
        if (variableDefinition == null)
          this.NotAssignedVariables.Add(context);
        else if (variableDefinition.StatementBlock == null)
          this.UncertainVariables.Add(context);
      }

      /// <summary>
      /// Конструктор.
      /// </summary>
      /// <param name="globalContext">Глобальный контекст.</param>
      /// <param name="document">Проверяемый документ.</param>
      public NotAssignedVariablesListener(IContext globalContext, IDocument document) 
      {
        this.globalContext = globalContext;
        this.document = document;        

        this.NotAssignedVariables = new List<IsblParser.VariableContext>();
        this.UncertainVariables = new List<IsblParser.VariableContext>();
      }
    }

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() => 
      new RuleInfo(typeof(UsingNotAssignedVarRule).Name, Resources.UsingNotAssignedVarRuleDescription), true);

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
      var listener = new NotAssignedVariablesListener(context, document);
      walker.Walk(listener, tree);

      foreach (var notAssignedVariable in listener.NotAssignedVariables)
        report.AddError(Code, string.Format(Resources.UsingNotAssignedVariable, notAssignedVariable.GetText()),
          document, notAssignedVariable.Start.ToTextPosition());

      foreach (var uncertainVariable in listener.UncertainVariables)
        report.AddWarning(Code, string.Format(Resources.VariableCanHasUncertainedValue, uncertainVariable.GetText()),
          document, uncertainVariable.Start.ToTextPosition());
    }

    #endregion
  }
}
