using System.Collections.Generic;
using System.Linq;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик диалогов.
  /// </summary>
  internal class RouteBlockPackageHandler : IPackageHandler<RouteBlock>
  {
    #region Константы

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    /// <summary>
    /// Активная.
    /// </summary>
    private const string Active = "SYSRES_SYSCOMP.OPERATING_RECORD_FLAG_VALUE_MASCULINE";

    /// <summary>
    /// Тип базового блока.
    /// </summary>
    private const string BaseBlockTypeReqName = "BaseBlockType";

    /// <summary>
    /// Свойства блока.
    /// </summary>
    private const string BlockPropertiesReqName = "Properties"; 

    #endregion

    #region IPackageHandler

    public IEnumerable<RouteBlock> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.RouteBlocks)
      {
        var entity = PackageHandlerUtils.CreateEntity<RouteBlock>(model);

        var stateReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == StateReqName);
        if (stateReq != null)
          entity.State = stateReq.ValueLocalizeID == Active ? ComponentState.Active : ComponentState.Closed;

        var baseBlockTypeReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == BaseBlockTypeReqName);
        if (baseBlockTypeReq != null)
          entity.BaseBlockType = RouteBlockUtils.GetBaseBlockType(baseBlockTypeReq.ValueLocalizeID);
        else
          entity.BaseBlockType = RouteBlockType.Unknown;

        var blockPropertiesReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == BlockPropertiesReqName);
        if (blockPropertiesReq != null)
        {
          var blockProperties = blockPropertiesReq.DecodedText;
          if (!string.IsNullOrWhiteSpace(blockProperties))
          {
            var description = WorkflowDescriptionParser.Parse(blockProperties);
            entity.WorkflowBlock = description.Blocks.FirstOrDefault();
          }
        }

        yield return entity;
      }
    }

    #endregion
  }
}
