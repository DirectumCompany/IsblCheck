using System;
using IsblCheck.Core.Reports;

namespace IsblCheck.Reports.Printers
{
  /// <summary>
  /// Принтер отчетов в консоль.
  /// </summary>
  public class ConsoleReportPrinter : IReportPrinter
  {
    #region IReportPrinter

    /// <summary>
    /// Напечатать отчет.
    /// </summary>
    /// <param name="report">Отчет.</param>
    public void Print(IReport report)
    {
      foreach (var message in report.Messages)
      {
        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\n", message.Code, message.Severity, message.Description,
          message.Position.Line, message.Position.Column);
      }
    }

    #endregion
  }
}
