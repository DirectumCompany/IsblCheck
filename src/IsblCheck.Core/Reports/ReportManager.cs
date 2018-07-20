using System.Collections.Generic;

namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Менеджер отчетов.
  /// </summary>
  internal class ReportManager : IReportManager
  {
    /// <summary>
    /// Список принтеров.
    /// </summary>
    public IList<IReportPrinter> Printers { get; }

    /// <summary>
    /// Создать отчет.
    /// </summary>
    /// <returns>Отчет.</returns>
    public IReport Create()
    {
      return new Report(this.Printers);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal ReportManager()
    {
      this.Printers = new List<IReportPrinter>();
    }
  }
}
