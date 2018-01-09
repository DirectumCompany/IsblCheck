using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик сценариев.
  /// </summary>
  internal class ScriptPackageHandler : IPackageHandler<Script>
  {
    #region Константы

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    /// <summary>
    /// Исходный код сценария.
    /// </summary>
    private const string CalculationTextReqName = "Текст";

    /// <summary>
    /// Активная.
    /// </summary>
    private const string Active = "SYSRES_SYSCOMP.OPERATING_RECORD_FLAG_VALUE_MASCULINE";

    #endregion

    #region IPackageHandler

    public IEnumerable<Script> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.Scripts)
      {
        var entity = PackageHandlerUtils.CreateEntity<Script>(model);

        var stateReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == StateReqName);
        if (stateReq != null)
          entity.State = stateReq.ValueLocalizeID == Active ? ComponentState.Active : ComponentState.Closed;

        var calculationTextReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == CalculationTextReqName);
        if (calculationTextReq != null)
          entity.CalculationText = calculationTextReq.DecodedText;
        else
          entity.CalculationText = string.Empty;

        yield return entity;
      }
    }

    #endregion
  }
}
