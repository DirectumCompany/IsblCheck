using Antlr4.Runtime.Tree;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;

namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Базовый класс правила.
  /// </summary>
  public abstract class AbstractRule : IRule
  {
    #region IRule

    /// <summary>
    /// Применить правило.
    /// </summary>
    /// <param name="report">Отчет.</param>
    /// <param name="document">Документ.</param>
    /// <param name="context">Контекст.</param>
    public abstract void Apply(IReport report, IDocument document, IContext context);

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected AbstractRule()
    {
    }
  }
}
