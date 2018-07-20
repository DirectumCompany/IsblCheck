using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Context.Development;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;

namespace IsblCheck.BaseRules.Functions
{
  public class IncorrectFunctionParamsCountRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "F001";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(IncorrectFunctionParamsCountRule).Name, Resources.IncorrectFunctionParamsCountRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info => info.Value;

    #endregion

    private class IncorrectFunctionParamsCountListener : IsblBaseListener
    {
      // Имена функций-исключений из правил.
      private static readonly List<string> RuleExceptions = new List<string>
      {
        "ArrayOf",
        "Массив",
        "Вызвать",
        "CallProcedure",
        "CreateArray",
        "СоздатьМассив"
      };

      private readonly IReadOnlyDictionary<string, Function> developerFunctions;
      private readonly IReadOnlyDictionary<string, Function> systemFunctions;

      public List<IsblParser.FunctionContext> FunctionCalls { get; } = new List<IsblParser.FunctionContext>();

      #region IsblBaseListener

      public override void EnterFunction([NotNull] IsblParser.FunctionContext context)
      {
        var functionName = context.identifier().GetText();
        if (RuleExceptions.Contains(functionName, StringComparer.OrdinalIgnoreCase))
          return;
        if (!this.developerFunctions.TryGetValue(functionName, out Function function) &&
            !this.systemFunctions.TryGetValue(functionName, out function))
          return;
        var paramsParser = new ParameterListParser(context.parameterList());
        paramsParser.Parse();
        if(paramsParser.ParameterValues.Count > function.Arguments.Count)
        {
          this.FunctionCalls.Add(context);
          return;
        }
        // TODO: Сейчас билдер не считает ошибкой, если упустить обязательный параметр функции.
        // За исключением некоторых системных функций. Раскомментировать, когда (и если) поправят билдер.

        //int argIndex = 0;
        //foreach (var arg in function.Arguments
        //  .Where(a => !a.HasDefaultValue)
        //  .OrderBy(a => a.Number))
        //{
        //  if (!paramsParser.ParameterValues.Any(p => p.Index == argIndex && !p.IsEmpty))
        //  {
        //    this.FunctionCalls.Add(context);
        //    return;
        //  }
        //  argIndex++;
        //}
      }

      #endregion

      public IncorrectFunctionParamsCountListener(IContext context)
      {
        this.developerFunctions = context.Development.Functions.ToDictionary(k => k.Name);
        this.systemFunctions = context.Application.Functions.ToDictionary(k => k.Name);
      }
    }

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new IncorrectFunctionParamsCountListener(context);
      walker.Walk(listener, tree);
      foreach (var funcCall in listener.FunctionCalls)
      {
        var description = Resources.WrongFunctionArgumentsCount;
        report.AddError(Code, description, document, funcCall.identifier().GetTextPosition());
      }
    }

    #endregion

  }
}
