using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal abstract class ReportPackageHandler
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

    #region Методы

    /// <summary>
    /// Прочитать отчет из модели.
    /// </summary>
    /// <typeparam name="T">Тип отчета.</typeparam>
    /// <param name="model">Модель.</param>
    /// <returns>Отчет.</returns>
    protected T Read<T>(ComponentModel model) where T : Report
    {
      var entity = PackageHandlerUtils.CreateEntity<T>(model);

      var viewerReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == ViewerReqName);
      if (viewerReq != null)
        entity.Viewer = viewerReq.DecodedText;

      var stateReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == StateReqName);
      if (stateReq != null)
        entity.State = stateReq.ValueLocalizeID == Active ? ComponentState.Active : ComponentState.Closed;

      var calculationTextReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == CalculationTextReqName);
      if (calculationTextReq != null)
        entity.CalculationText = calculationTextReq.DecodedText;

      var templateTextReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == TemplateTextReqName);
      if (templateTextReq != null)
        entity.TemplateText = templateTextReq.DecodedText;

      return entity;
    }

    #endregion
  }
}
