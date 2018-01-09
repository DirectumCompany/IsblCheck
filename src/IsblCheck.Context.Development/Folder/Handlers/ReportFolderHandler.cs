using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal abstract class ReportFolderHandler<T> : FolderHandlerBase<T, ComponentModel> where T: Report
  {
    #region Константы

    /// <summary>
    /// Расчет.
    /// </summary>
    private const string CalculationTextReqName = "Расчет";

    /// <summary>
    /// Шаблон.
    /// </summary>
    private const string TemplateTextReqName = "Шаблон";

    /// <summary>
    /// Приложение.
    /// </summary>
    private const string ViewerReqName = "Код приложения";

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    /// <summary>
    /// Активная.
    /// </summary>
    private const string Active = "SYSRES_SYSCOMP.OPERATING_RECORD_FLAG_VALUE_MASCULINE";

    #endregion

    #region Поля и свойства

    private static readonly ILog log = LogManager.GetLogger<ReportFolderHandler<T>>();

    #endregion

    #region FolderHandlerBase

    protected override IEnumerable<T> ReadComponents(ComponentModel model, string componentFolderPath)
    {
      var entity = PackageHandlerUtils.CreateEntity<T>(model);

      var viewerReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == ViewerReqName);
      if (viewerReq != null)
        entity.Viewer = viewerReq.Value;

      var stateReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == StateReqName);
      if (stateReq != null)
        entity.State = stateReq.ValueLocalizeID == Active ? ComponentState.Active : ComponentState.Closed;

      var calculationFile = Path.Combine(componentFolderPath, "Calculation.isbl");
      if (File.Exists(calculationFile))
        entity.CalculationText = File.ReadAllText(calculationFile, Encoding.GetEncoding(1251));
      else
        log.Warn($"File not found {calculationFile}");

      var templateFile = Path.Combine(componentFolderPath, "Template");
      if (File.Exists(templateFile))
        entity.TemplateText = File.ReadAllText(templateFile, Encoding.GetEncoding(1251));
      else
        log.Warn($"File not found {templateFile}");

      yield return entity;
    }

    #endregion
  }
}
