using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal class IntegratedReportFolderHandler : ReportFolderHandler<IntegratedReport>
  {
    #region FolderHandlerBase

    protected override string FolderName { get { return "IntegratedReports"; } }

    protected override string CardModelRootNode { get { return "IntegratedReport"; } } 

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
