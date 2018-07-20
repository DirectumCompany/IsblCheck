using System.Collections.Generic;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal class IntegratedReportFolderHandler : ReportFolderHandler<IntegratedReport>
  {
    #region FolderHandlerBase

    protected override string FolderName => "IntegratedReports";

    protected override string CardModelRootNode => "IntegratedReport";

    #endregion

    #region ReportFolderHandler

    protected override IEnumerable<IntegratedReport> ReadComponents(ComponentModel model, string componentFolderPath)
    {
      foreach (var report in base.ReadComponents(model, componentFolderPath))
      {
        report.ReferenceName = model.ReferenceName;
        yield return report;
      }
    }

    #endregion
  }
}
