using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;
using System;
using System.Linq;

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Правило для проверки наличия справки у функций.
  /// Считается, что справка отсутствует, если не заполнены поля Help и Comment (для старого формата справки).
  /// </summary>
  public class FunctionWithoutHelpRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "F019";

    #endregion

    #region Поля

    /// <summary>
    /// Инфо правила.
    /// </summary>
    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(FunctionWithoutHelpRule).Name, Resources.FunctionWithoutHelpRuleDescription), true);

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

      var function = context.Development.Functions
        .FirstOrDefault(f => string.Equals(f.Name, document.ComponentName, StringComparison.OrdinalIgnoreCase));
      if(function != null && string.IsNullOrWhiteSpace(function.Help) && string.IsNullOrWhiteSpace(function.Comment))
        report.AddWarning(Code, Resources.NoFunctionHelp, document, new TextPosition());
    }

    #endregion
  }
}
