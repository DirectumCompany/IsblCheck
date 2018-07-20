using System.Collections.Generic;
using System.Linq;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик приложений-просмотрщиков.
  /// </summary>
  internal class ViewerFolderHandler : FolderHandlerBase<Viewer, ComponentModel>
  {
    #region Константы

    /// <summary>
    /// Имя реквизита расширения.
    /// </summary>
    public const string ExtensionReqName = "Расширение";

    /// <summary>
    /// Имя реквизита типа приложения-просмотрщика.
    /// </summary>
    public const string ViewerTypeReqName = "ТипРедактора";

    /// <summary>
    /// Id значения Crystal Reports.
    /// </summary>
    private const string CrystalReports = "SYSRES_SYSCOMP.REPORT_APP_VIEWER_CRYSTAL_REPORTS";

    /// <summary>
    /// Id значения Microsoft Excel.
    /// </summary>
    private const string MicrosoftExcel = "SYSRES_SYSCOMP.REPORT_APP_VIEWER_EXCEL";

    /// <summary>
    /// Id значения Microsoft Word.
    /// </summary>
    private const string MicrosoftWord = "SYSRES_SYSCOMP.REPORT_APP_VIEWER_WORD";

    /// <summary>
    /// Id значения Встроенный редактор.
    /// </summary>
    private const string Internal = "SYSRES_SYSCOMP.REPORT_APP_VIEWER_INTERNAL";

    #endregion

    #region FolderHandlerBase

    protected override string FolderName => "Viewers";

    protected override string CardModelRootNode => "Viewer";

    protected override IEnumerable<Viewer> ReadComponents(ComponentModel model, string componentFolderPath)
    {
      var entity = PackageHandlerUtils.CreateEntity<Viewer>(model);

      var extensionReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == ExtensionReqName);
      if (extensionReq != null)
        entity.Extension = extensionReq.Value;

      var viewerTypeReqName = model.Card.Requisites
        .FirstOrDefault(r => r.Code == ViewerTypeReqName);
      if (viewerTypeReqName != null)
        switch (viewerTypeReqName.ValueLocalizeID)
        {
          case CrystalReports:
            entity.ViewerType = ViewerType.CrystalReports;
            break;
          case MicrosoftExcel:
            entity.ViewerType = ViewerType.Excel;
            break;
          case MicrosoftWord:
            entity.ViewerType = ViewerType.Word;
            break;
          case Internal:
            entity.ViewerType = ViewerType.Internal;
            break;
        }

      yield return entity;
    }

    #endregion
  }
}
