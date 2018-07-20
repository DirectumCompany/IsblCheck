using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;

namespace IsblCheck.BaseRules.LogicalExpressions
{
  /// <summary>
  /// Правило о том, что нельзя использовать константы True и False в логическом выражении.
  /// </summary>
  public class UsingTrueFalseKeywordsRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "B003";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(UsingTrueFalseKeywordsRule).Name, Resources.UsingTrueFalseKeywordsRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info => info.Value;

    #endregion

    private class UsingTrueFalseKeywordsListener : IsblBaseListener
    {
      private const string TrueConstant = "True";
      private const string FalseConstant = "False";

      public List<ParserRuleContext> Entries { get; } = new List<ParserRuleContext>();

      #region IsblBaseListener

      public override void EnterVariable([NotNull] IsblParser.VariableContext context)
      {
        if (!IsTrueFalseConstant(context.GetText()))
          return;

        if(IsVariableUsedAsArgument(context))
        {
          Entries.Add(context);
        }
      }

      #endregion
      
      private static bool IsTrueFalseConstant(string constName)
      {
        return string.Equals(constName, TrueConstant, StringComparison.OrdinalIgnoreCase) ||
               string.Equals(constName, FalseConstant, StringComparison.OrdinalIgnoreCase);
      }

      /// <summary>
      /// Проверить, что переменная используется как аргумент в выражении.
      /// </summary>
      /// <param name="context">Контекст переменной.</param>
      private static bool IsVariableUsedAsArgument(IsblParser.VariableContext context)
      {
        // Считаем, что переменная используется в логическом выражении, если
        // она используется не только как операнд
        // (то есть используется вместе с оператором или другим выражением).
        var parent = context.Parent;
        while (parent != null)
        {
          if (parent is IsblParser.ExpressionContext expression)
          {
            if (expression.ChildCount > 1)
            {
              return true;
            }
          }
          else
          {
            if (!(parent is IsblParser.OperandContext))
              return false;
          }
          parent = parent.Parent;
        }
        return false;
      }
    }

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new UsingTrueFalseKeywordsListener();
      walker.Walk(listener, tree);
      foreach (var entry in listener.Entries)
      {
        report.AddWarning(Code, Resources.UsingTrueFalseKeywords, document, entry.GetTextPosition());
      }
    }

    #endregion

  }
}
