namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Интерфейс принтера отчетов
  /// </summary>
  public interface IReportPrinter
  {
    /// <summary>
    /// Напечатать отчет.
    /// </summary>
    /// <param name="report">Отчет.</param>
    void Print(IReport report);
  }
}
