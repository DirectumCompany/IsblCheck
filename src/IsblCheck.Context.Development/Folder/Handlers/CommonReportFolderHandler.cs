using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal class CommonReportFolderHandler : ReportFolderHandler<CommonReport>
  {
    #region FolderHandlerBase

    protected override string FolderName { get { return "Reports"; } }

    protected override string CardModelRootNode { get { return "Report"; } }

    #endregion
  }
}
