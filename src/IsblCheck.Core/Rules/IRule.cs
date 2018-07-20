using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;

namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Интерфейс правила.
  /// </summary>
  public interface IRule
  {
    /// <summary>
    /// Применить правило.
    /// </summary>
    /// <param name="report">Отчет.</param>
    /// <param name="document">Документ.</param>
    /// <param name="context">Контекст.</param>
    void Apply(IReport report, IDocument document, IContext context);
  }
}
