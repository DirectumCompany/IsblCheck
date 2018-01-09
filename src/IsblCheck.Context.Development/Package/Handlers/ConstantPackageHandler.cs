using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик констант.
  /// </summary>
  internal class ConstantPackageHandler : IPackageHandler<Constant>
  {
    #region Константы

    /// <summary>
    /// Имя реквизита признака репликации константы.
    /// </summary>
    private const string IsReplicatedReqName = "ISBConstServerStatus";

    /// <summary>
    /// Имя реквизита признака общей константы.
    /// </summary>
    private const string IsCommonReqName = "ISBConstFirmStatus";

    /// <summary>
    /// Id значения реплицировать.
    /// </summary>
    private const string Replicate = "SYSRES_SYSCOMP.CONST_SERVER_STATUS_REPLICATE";

    /// <summary>
    /// Id значения общий.
    /// </summary>
    private const string Common = "SYSRES_SYSCOMP.CONST_FIRM_STATUS_COMMON";

    #endregion

    #region IPackageHandler

    public IEnumerable<Constant> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.Constants)
      {
        var entity = PackageHandlerUtils.CreateEntity<Constant>(model);

        var isReplicatedReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == IsReplicatedReqName);
        if (isReplicatedReq != null)
          entity.IsReplicated = isReplicatedReq.ValueLocalizeID == Replicate;

        var isCommonReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == IsCommonReqName);
        if (isCommonReq != null)
          entity.IsCommon = isCommonReq.ValueLocalizeID == Common;

        yield return entity;
      }
    }

    #endregion
  }
}
