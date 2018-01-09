using Antlr4.Runtime;
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

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Правило, по которому при создании исключения должен указываться класс исключения.
  /// </summary>
  public class ExceptionClassNotSpecifiedRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "F007";

    /// <summary>
    /// Имя функции создания исключения.
    /// </summary>
    private const string CreateExceptionFunctionName = "CreateException";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(ExceptionClassNotSpecifiedRule).Name, Resources.ExceptionClassNotSpecifiedRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info { get { return info.Value; } }

    #endregion

    #region Вложенные классы

    private class ExceptionClassNotSpecifiedRuleListener : IsblBaseListener
    {
      public List<ParserRuleContext> EmptyExceptionClassParams { get; } = new List<ParserRuleContext>();

      #region IsblBaseListener

      public override void EnterFunction([NotNull] IsblParser.FunctionContext context)
      {
        var functionName = context.identifier().GetText();
        if (!string.Equals(functionName, CreateExceptionFunctionName))
          return;
        var functionParams = context.parameterList()?.expression();
        if (functionParams == null)
          return;
        if (functionParams.Length == 0)
          return;
        var exceptionClassStringValue = GetStringOperandValue(functionParams[0]);
        if (exceptionClassStringValue?.Trim() == string.Empty)
        {
          EmptyExceptionClassParams.Add(functionParams[0]);
        }
      }

      #endregion

      /// <summary>
      /// Получить строковую константу из выражения.
      /// </summary>
      /// <param name="expression">Выражение.</param>
      /// <returns>Строковая константа, либо null, если выражение не является строкой.</returns>
      private static string GetStringOperandValue(IsblParser.ExpressionContext expression)
      {
        return expression.operand()?.@string()?.GetText().Trim('\'', '"');
      }
    }

    #endregion

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new ExceptionClassNotSpecifiedRuleListener();
      walker.Walk(listener, tree);
      foreach (var emptyExceptionClassParam in listener.EmptyExceptionClassParams)
      {
        report.AddInformation(Code, Resources.ExceptionClassNotSpecified, document, emptyExceptionClassParam.GetTextPosition());
      }
    }

    #endregion

  }
}
