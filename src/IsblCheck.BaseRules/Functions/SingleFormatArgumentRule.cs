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
  /// Правило для обнаружения вызовов функций Format, LoadStringFormat, в которые передаётся одно значение через ArrayOf.
  /// </summary>
  public class SingleFormatArgumentRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "F018";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(SingleFormatArgumentRule).Name, Resources.SingleFormatArgumentRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info { get { return info.Value; } }

    #endregion

    private class SingleFormatArgumentRuleListener : IsblBaseListener
    {
      public List<ParserRuleContext> SingleArgumentEntries { get; } = new List<ParserRuleContext>();

      #region IsblBaseListener

      public override void EnterFunction([NotNull] IsblParser.FunctionContext context)
      {
        var functionName = context.identifier().GetText();
        if (!functionName.Equals("Format", StringComparison.OrdinalIgnoreCase) &&
            !functionName.Equals("Формат", StringComparison.OrdinalIgnoreCase) &&
            !functionName.Equals("LoadStringFmt", StringComparison.OrdinalIgnoreCase))
          return;
        if (context.parameterList() == null)
          return;
        var parameters = context.parameterList().expression();
        IsblParser.OperandContext argsListOperand = null;
        if (functionName.Equals("LoadStringFmt", StringComparison.OrdinalIgnoreCase))
        {
          if (parameters.Length < 3)
            return;
          argsListOperand = parameters[2].operand();
        }
        else
        {
          if (parameters.Length < 2)
            return;
          argsListOperand = parameters[1].operand();
        }
        if (argsListOperand == null || argsListOperand.function() == null)
          return;
        var argsListFunctionName = argsListOperand.function().identifier().GetText();
        if (!argsListFunctionName.Equals("ArrayOf", StringComparison.OrdinalIgnoreCase) &&
            !argsListFunctionName.Equals("Массив", StringComparison.OrdinalIgnoreCase))
          return;
        var arrayOfFunctionParams = argsListOperand.function().parameterList();
        if (arrayOfFunctionParams == null)
          return;
        var arguments = arrayOfFunctionParams.expression();
        if (arguments.Length == 1)
        {
          this.SingleArgumentEntries.Add(argsListOperand.function());
        }
      }

      #endregion
    }

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new SingleFormatArgumentRuleListener();
      walker.Walk(listener, tree);
      foreach (var argument in listener.SingleArgumentEntries)
      {
        string description = Resources.SingleArgumentForFormatFunction;
        report.AddInformation(Code, description, document, argument.GetTextPosition());
      }
    }

    #endregion
  }
}
