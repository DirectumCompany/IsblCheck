using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик диалогов.
  /// </summary>
  internal class RouteBlockFolderHandler : FolderHandlerBase<RouteBlock, ComponentModel>
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

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Имена файлов событий.
    /// </summary>
    private static readonly Dictionary<string, string> eventFileNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      { "BeforeStart", "BeforeStart.isbl" },
      { "AfterFinish", "AfterFinish.isbl" },
      { "OnCreateJobs", "OnCreateJobs.isbl" },
      { "SubTaskCreate", "SubTaskCreate.isbl" },
      { "SubtaskInit", "SubtaskInit.isbl" },
      { "SubtaskStart", "SubtaskStart.isbl" },
      { "BeforeQueryParams", "BeforeQueryParams.isbl" },
      { "AfterQueryParams", "AfterQueryParams.isbl" },
      { "OnCreateNotices", "OnCreateNotices.isbl" },
      { "OnFormShow", "OnFormShow.isbl" },
      { "OnFormHide", "OnFormHide.isbl" },
      { "SearchScript", "Monitoring.isbl" },
      { "Script", "Calculation.isbl" },
      { "ISBL", "Condition.isbl" }
    };

    private static readonly ILog log = LogManager.GetLogger<RouteBlockFolderHandler>();

    #endregion

    #region FolderHandlerBase

    protected override string FolderName => "RouteBlocks";

    protected override string CardModelRootNode => "RouteBlock";

    protected override IEnumerable<RouteBlock> ReadComponents(ComponentModel model, string componentFolderPath)
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

      var blockPropertiesFile = Path.Combine(componentFolderPath, "Properties.xml");
      if (File.Exists(blockPropertiesFile))
      {
        var blockProperties = File.ReadAllText(blockPropertiesFile, Encoding.GetEncoding(1251));
        var description = WorkflowDescriptionParser.Parse(blockProperties);
        entity.WorkflowBlock = description.Blocks.FirstOrDefault();
        if (entity.WorkflowBlock != null)
        {
          ReadActions(entity.WorkflowBlock, componentFolderPath);
          ReadEvents(entity.WorkflowBlock, componentFolderPath);
          ReadProperties(entity.WorkflowBlock, componentFolderPath);
        }
        else
          log.Warn($"Cannot read properties for block {entity.Name}");
      }
      else
        log.Warn($"File not found {blockPropertiesFile}");

      yield return entity;
    }

    #endregion

    #region Методы

    private static void ReadActions(WorkflowBlock block, string componentFolderPath)
    {
      foreach (var action in block.Actions)
      {
        var actionFile = Path.Combine(componentFolderPath, "Actions", $"{action.Name}.isbl");
        if (File.Exists(actionFile))
        {
          action.CalculationText = File.ReadAllText(actionFile, Encoding.GetEncoding(1251));
        }
        else
          log.Warn($"File not found {actionFile}");
      }
    }

    private static void ReadEvents(WorkflowBlock block, string componentFolderPath)
    {
      foreach(var @event in block.Events)
      {
        if (eventFileNames.TryGetValue(@event.Name, out string eventFileName))
        {
          var eventFile = Path.Combine(componentFolderPath, "Events", eventFileName);
          if (File.Exists(eventFile))
          {
            @event.CalculationText = File.ReadAllText(eventFile, Encoding.GetEncoding(1251));
          }
          //else
          //  log.Warn($"File not found {eventFile}");
        }
        else
          log.Warn($"Unknown event {@event.Name}");
      }
    }

    private static void ReadProperties(WorkflowBlock block, string componentFolderPath)
    {
      foreach (var property in block.IsblProperties)
      {
        if (eventFileNames.TryGetValue(property.Name, out string propertyFileName))
        {
          var propertyCalculationFile = Path.Combine(componentFolderPath, "Events", propertyFileName);
          if (File.Exists(propertyCalculationFile))
          {
            property.CalculationText = File.ReadAllText(propertyCalculationFile, Encoding.GetEncoding(1251));
          }
          //else
          //  log.Warn($"File not found {propertyCalculationFile}");
        }
        else
          log.Warn($"Unknown block ISBL property {property.Name}");
      }
    }

    #endregion
  }
}
