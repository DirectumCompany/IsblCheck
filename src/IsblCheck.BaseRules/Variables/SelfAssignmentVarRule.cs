using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsblCheck.BaseRules.Variables
{
  /// <summary>
  /// Правило поиска переменных, которые присваиваются сами себе.
  /// </summary>
  internal class SelfAssignmentVarRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "A005";

    #endregion

    #region Вложенные классы

    /// <summary>
    /// Поиск переменных, которые присваиваются сами себе.
    /// </summary>
    private class SelfAssignmentVarListener : IsblBaseListener
    {
      private IContext mainContext;


      /// <summary>
      /// Конструктор.
      /// </summary>
      /// <param name="context">Контекст</param>
      public SelfAssignmentVarListener(IContext context)
      {
        this.mainContext = context;
      }

      /// <summary>
      /// Список переменных, попадающих под данное правило. 
      /// </summary>
      public List<IsblParser.AssignStatementContext> ContextsWithError { get; } = new List<IsblParser.AssignStatementContext>();

      /// <summary>
      /// Вход в предложение означивания.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterAssignStatement([NotNull]IsblParser.AssignStatementContext context)
      {
        var childCount = context.ChildCount;
        var builder = new StringBuilder();
        for (var i = 0; i < childCount; i++)
        {
          var child = context.GetChild(i);
          if (IsEqual(child))
            break;
          builder.Append(child.GetText());
        }

        string leftExpression = builder.ToString();
        string rightExpression = context.expression().GetText();
        if(mainContext.Application.IsExistPredefinedVariable(context.variable().GetText()))
        {
          leftExpression = leftExpression.Trim('!');
          rightExpression = rightExpression.Trim('!');
        }

        if (leftExpression.Equals(rightExpression, StringComparison.OrdinalIgnoreCase))
        {
          if (context.typedefinition() == null)
          {
            IsblParser.InvocationCallContext[] invCallContexts = context.invocationCall();

            int numberOfInvCalls = invCallContexts.Length;
            if (numberOfInvCalls > 1)
            {
              string requisites = invCallContexts[numberOfInvCalls - 2].identifier().GetText();
              string value = invCallContexts[numberOfInvCalls - 1].identifier().GetText();

              if (!requisites.Equals("requisites", StringComparison.OrdinalIgnoreCase) ||
                !(value.Equals("value", StringComparison.OrdinalIgnoreCase) || value.StartsWith("as", StringComparison.OrdinalIgnoreCase)) ||
                invCallContexts[numberOfInvCalls - 1].parameterList() != null)
              {
                this.ContextsWithError.Add(context);
              }
            }
            else
            {
              if (numberOfInvCalls == 1)
              {
                var name = context.variable().GetText().Trim('!');
                if ((!name.Equals("object", StringComparison.OrdinalIgnoreCase) &&
                  !name.Equals("sender", StringComparison.OrdinalIgnoreCase)) ||
                  invCallContexts[0].parameterList() != null)
                {
                  this.ContextsWithError.Add(context);
                }
              }
              else
                this.ContextsWithError.Add(context);
            }
          }
          else
            this.ContextsWithError.Add(context);
        }
      }

      /// <summary>
      /// Проверка на знак равенства.
      /// </summary>
      /// <param name="context">Контекст.</param>
      private bool IsEqual(IParseTree child)
      {
        var eq = child as TerminalNodeImpl;
        return (eq != null &&
          ((eq.Symbol.Type == IsblParser.EQ) ||
          (eq.Symbol.Type == IsblParser.COLON)));
      }

    }

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() => 
      new RuleInfo(typeof(SelfAssignmentVarRule).Name, Resources.SelfAssignmentVarRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info { get { return info.Value; } }

    #endregion

    #region Методы

    /// <summary>
    /// Применить правило.
    /// </summary>
    /// <param name="report">Отчет.</param>
    /// <param name="document">Документ.</param>
    /// <param name="tree">Дерево исходного кода.</param>
    /// <param name="context">Контекст.</param>
    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new SelfAssignmentVarListener(context);
      walker.Walk(listener, tree);
      var listContextesWithError = listener.ContextsWithError;

      foreach (var contextWithError in listContextesWithError)
      {
        report.AddWarning(Code,
          string.Format(Resources.SelfAssignmentVarRuleDescription, contextWithError.expression().GetText()),
          document, contextWithError.GetTextPosition());
      }
    }

    #endregion
  }
}
