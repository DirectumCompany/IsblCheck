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

namespace IsblCheck.BaseRules.ObjectModel
{
  /// <summary>
  /// Правило поиска объектов, которые не восстанавливают свое состояние.
  /// </summary>
  internal class RecoveryObjectStateRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "I001";

    #endregion

    #region Внутренние классы

    /// <summary>
    /// Определение состояния объекта.
    /// </summary>
    private class ObjectStateDefinition
    {
      /// <summary>
      /// Имя объекта, вызывающего метод.
      /// </summary>
      public string InvokerName { get; set; }

      /// <summary>
      /// Имя вызываемого метода.
      /// </summary>
      public string CallMethodName { get; set; }

      /// <summary>
      /// Имя получаемого идентификатора состояния.
      /// </summary>
      public string IdentifierStateName { get; set; }

      /// <summary>
      /// Контекст вызова.
      /// </summary>
      public ParserRuleContext StatementContext { get; set; }
    }

    /// <summary>asdfasdfasdf
    /// Поиск объектов с незавершенным состоянием, открываемые метод с параметром.
    /// </summary>
    private class CallMethodsWithSaveStateListener : IsblBaseListener
    {
      /// <summary>
      /// Определения объектов с не восстановленным состоянием.
      /// </summary>
      public readonly List<ObjectStateDefinition> NotRestoredObjectDefinitions = new List<ObjectStateDefinition>();

      /// <summary>
      /// Вызываемые методы.
      /// </summary>
      private static readonly Dictionary<string, string> callMethods = new Dictionary<string, string>
      {
        { "AddFrom", "DelFrom" },
        { "AddJoin", "DelJoin" },
        { "AddOrderBy", "DelOrderBy" },
        { "AddSelect", "DelSelect" },
        { "AddWhere", "DelWhere" }
      };

      /// <summary>
      /// Получить предложение означивания.
      /// </summary>
      /// <param name="node">Узел правил.</param>
      /// <returns>Предложение означивания.</returns>
      private static IsblParser.AssignStatementContext GetParentAssignStatementContext(IRuleNode node)
      {
        var parent = node.Parent;
        while (parent != null)
        {
          if (parent is IsblParser.AssignStatementContext)
            break;
          parent = parent.Parent;
        }
        return parent as IsblParser.AssignStatementContext;
      }

      /// <summary>
      /// Вход в COM вызов.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterInvocationCall(IsblParser.InvocationCallContext context)
      {
        var callMethod = context.GetChild<IsblParser.IdentifierContext>(0);
        if (callMethod == null)
          return;

        var callMethodName = callMethod.GetText();
        var isOpenMethod = callMethods
          .Any(m => m.Key.Equals(callMethodName, StringComparison.OrdinalIgnoreCase));
        var isCloseMethod = callMethods
          .Any(m => m.Value.Equals(callMethodName, StringComparison.OrdinalIgnoreCase));

        if (isOpenMethod)
        {
          var operand = context.Parent as IsblParser.OperandContext;
          if (operand == null)
            return;

          var invokerName = operand.children
           .TakeWhile(ctx => ctx != context)
           .Select(ctx => ctx.GetText())
           .Aggregate((accumulate, text) => accumulate + text);
          // Удаляем последнюю точку.
          invokerName = invokerName.Remove(invokerName.Length - 1, 1);

          var assingStatement = GetParentAssignStatementContext(context);
          if (assingStatement == null)
            return;

          var identifierState = assingStatement.GetChild<IsblParser.VariableContext>(0);
          if (identifierState == null)
            return;

          var identifierStateName = identifierState.GetText();

          var objectStateDefinition = new ObjectStateDefinition
          {
            CallMethodName = callMethodName,
            InvokerName = invokerName,
            IdentifierStateName = identifierStateName,
            StatementContext = assingStatement
          };
          this.NotRestoredObjectDefinitions.Add(objectStateDefinition);
        }
        else if (isCloseMethod)
        {
          var invokeStatement = context.Parent as IsblParser.InvokeStatementContext;
          if (invokeStatement == null)
            return;

          var invokerName = invokeStatement.children
           .TakeWhile(ctx => ctx != context)
           .Select(ctx => ctx.GetText())
           .Aggregate((accumulate, text) => accumulate + text);
          // Удаляем последнюю точку.
          invokerName = invokerName.Remove(invokerName.Length - 1, 1);

          var openMethodName = callMethods
            .First(m => m.Value.Equals(callMethodName, StringComparison.OrdinalIgnoreCase))
            .Key;

          var parameterList = context.GetChild<IsblParser.ParameterListContext>(0);
          if (parameterList == null)
            return;

          var identifierState = parameterList.GetChild<IsblParser.ExpressionContext>(0);
          if (identifierState == null)
            return;

          var identifierStateName = identifierState.GetText();

          var objectStateDefinitions = this.NotRestoredObjectDefinitions
            .Where(d => d.CallMethodName.Equals(openMethodName, StringComparison.OrdinalIgnoreCase) &&
              d.InvokerName.Equals(invokerName, StringComparison.OrdinalIgnoreCase) &&
              d.IdentifierStateName.Equals(identifierStateName, StringComparison.OrdinalIgnoreCase))
            .ToList();
          foreach (var objectStateDefinition in objectStateDefinitions)
            this.NotRestoredObjectDefinitions.Remove(objectStateDefinition);
        }
      }
    }

    /// <summary>
    /// Поиск объектов с незавершенным состоянием, открываемые функциями без параметра.
    /// </summary>
    private class CallMethodsWithoutSaveStateListener : IsblBaseListener
    {
      /// <summary>
      /// Определения объектов с не восстановленным состоянием.
      /// </summary>
      public readonly List<ObjectStateDefinition> NotRestoredObjectDefinitions = new List<ObjectStateDefinition>();

      /// <summary>
      /// Список переменных, созданных через CreateCachedReference/СоздатьКэшированныйСправочник.
      /// </summary>
      private readonly ISet<string> cachedReferenceVariableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      /// <summary>
      /// Вызываемые парные методы.
      /// </summary>
      private static readonly Dictionary<string, string> complementMethodNames = new Dictionary<string, string>
      {
        { "AddCheckPoint", "ReleaseCheckPoint" },
        { "DisableAll", "EnableAll" },
        { "DisableControls", "EnableControls" },
        { "EnterEditMode", "LeaveEditMode" },
        { "Open", "Close" },
        { "OpenRecord", "CloseRecord" }
      };

      public override void EnterAssignStatement([NotNull]IsblParser.AssignStatementContext context)
      {
        var variableContext = context.GetChild(0) as IsblParser.VariableContext;
        if (variableContext == null)
          return;

        var eq = context.GetChild(1) as TerminalNodeImpl;
        if (eq == null ||
          (eq.Symbol.Type != IsblParser.EQ &&
          eq.Symbol.Type != IsblParser.COLON))
          return;

        var variableName = variableContext.GetText();

        if (IsAssignedFromCreateCachedReferenceFunctionCall(context))
        {
          this.cachedReferenceVariableNames.Add(variableName);
        }
        else
        {
          this.cachedReferenceVariableNames.Remove(variableName);
        }
      }

      private static bool IsAssignedFromCreateCachedReferenceFunctionCall(IsblParser.AssignStatementContext context)
      {
        if (context.expression()?.operand()?.ChildCount != 1)
          return false;

        var function = context.expression().operand().GetChild(0) as IsblParser.FunctionContext;
        if (function == null)
          return false;

        var functionName = function.identifier().GetText();
        return functionName.Equals("CreateCachedReference", StringComparison.OrdinalIgnoreCase) ||
               functionName.Equals("СоздатьКэшированныйСправочник", StringComparison.OrdinalIgnoreCase);
      }

      public override void EnterInvocationCall(IsblParser.InvocationCallContext context)
      {
        var callMethod = context.GetChild<IsblParser.IdentifierContext>(0);
        if (callMethod == null)
          return;

        var invokeStatement = context.Parent as IsblParser.InvokeStatementContext;
        if (invokeStatement == null)
          return;

        var callMethodName = callMethod.GetText();
        var isOpenMethod = complementMethodNames
          .Any(m => m.Key.Equals(callMethodName, StringComparison.OrdinalIgnoreCase));
        var isCloseMethod = complementMethodNames
          .Any(m => m.Value.Equals(callMethodName, StringComparison.OrdinalIgnoreCase));

        if (!isOpenMethod && !isCloseMethod)
          return;

        var invokerName = invokeStatement.children
         .TakeWhile(ctx => ctx != context)
         .Select(ctx => ctx.GetText())
         .Aggregate((accumulate, text) => accumulate + text);
        // Удаляем последнюю точку.
        invokerName = invokerName.Remove(invokerName.Length - 1, 1);

        if (isOpenMethod)
        {
          var isCachedReference = this.cachedReferenceVariableNames.Contains(invokerName);
          if (!isCachedReference || !callMethodName.Equals("Open", StringComparison.OrdinalIgnoreCase))
          {
            var objectStateDefinition = new ObjectStateDefinition
            {
              CallMethodName = callMethodName,
              InvokerName = invokerName,
              StatementContext = invokeStatement
            };
            this.NotRestoredObjectDefinitions.Add(objectStateDefinition);
          }
        }
        else
        {
          var openMethodName = complementMethodNames
            .First(m => m.Value.Equals(callMethodName, StringComparison.OrdinalIgnoreCase))
            .Key;

          var objectStateDefinitions = this.NotRestoredObjectDefinitions
            .Where(d => d.CallMethodName.Equals(openMethodName, StringComparison.OrdinalIgnoreCase) &&
                        d.InvokerName.Equals(invokerName, StringComparison.OrdinalIgnoreCase))
            .ToList();
          foreach (var objectStateDefinition in objectStateDefinitions)
            this.NotRestoredObjectDefinitions.Remove(objectStateDefinition);
        }
      }
    }

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(RecoveryObjectStateRule).Name, Resources.RecoveryObjectStateRuleDescription), true);

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
      var paramListener = new CallMethodsWithSaveStateListener();
      var noParamListener = new CallMethodsWithoutSaveStateListener();

      walker.Walk(paramListener, tree);

      foreach (var notRestoredObjectDefinition in paramListener.NotRestoredObjectDefinitions)
      {
        report.AddWarning(Code, string.Format(Resources.ObjectDoesNotRestoreSelfStatus, notRestoredObjectDefinition.InvokerName),
          document, notRestoredObjectDefinition.StatementContext.GetTextPosition());
      }


      walker.Walk(noParamListener, tree);

      foreach (var notRestoredObjectDefinition in noParamListener.NotRestoredObjectDefinitions)
      {
        report.AddWarning(Code, string.Format(Resources.ObjectDoesNotRestoreSelfStatus, notRestoredObjectDefinition.InvokerName),
          document, notRestoredObjectDefinition.StatementContext.GetTextPosition());
      }
    }

    #endregion
  }
}
