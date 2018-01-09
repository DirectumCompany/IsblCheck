using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;
using System;

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Правило ограничения максимальной длины функции.
  /// </summary>
  public class FunctionTooBigRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "F012";

    /// <summary>
    /// Максимальное количество строк в функции.
    /// </summary>
    private const int MaxFunctionLinesCount = 500;

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(FunctionTooBigRule).Name, Resources.FunctionTooBigRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info { get { return info.Value; } }

    #endregion

    #region AbstractRule

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      if (document.ComponentType != ComponentType.Function)
        return;

      var linesCount = 0;
      var functionText = document.Text;
      for (int i = 0; i < functionText.Length; i++)
      {
        if (functionText[i] == '\n')
          linesCount++;
      }
      if (linesCount > 0)
        linesCount++;
      if (linesCount > MaxFunctionLinesCount)
      {
        report.AddInformation(Code, string.Format(Resources.FunctionTooBig, MaxFunctionLinesCount), document, new TextPosition());
      }
    }

    #endregion
  }
}
