using System.Collections.Generic;

namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Интерфейс менеджера отчетов.
  /// </summary>
  public interface IReportManager
  {
    /// <summary>
    /// Список принтеров.
    /// </summary>
    IList<IReportPrinter> Printers { get; }

    /// <summary>
    /// Создать отчет.
    /// </summary>
    /// <returns>Отчет.</returns>
    IReport Create();
  }
}
