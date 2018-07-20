using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using IsblCheck.Context.Development.Folder.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal class StandardRouteFolderHandler : FolderHandlerBase<StandardRoute, RecordRefModel>
  {
    #region Константы

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    private const string StateActiveRequisiteValue = "Действующая";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Имена файлов событий.
    /// </summary>
    private static readonly Dictionary<string, string> blockEventFileNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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

    /// <summary>
    /// Имена файлов событий ТМ.
    /// </summary>
    private static readonly Dictionary<string, string> eventFileNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      { "InitScript", "TaskBeforeSelection.isbl" },
      { "Script", "TaskAfterSelection.isbl" },
      { "TaskStart", "TaskStartPossibility.isbl" },
      { "TaskAbortPossibility", "TaskAbortPossibility.isbl" },
      { "TaskAbort", "TaskAbort.isbl" },
      { "OnTaskFormShow", "OnTaskFormShow.isbl" },
      { "OnTaskFormHide", "OnTaskFormHide.isbl" }
    };

    private static readonly ILog log = LogManager.GetLogger<StandardRouteFolderHandler>();

    #endregion

    #region FolderHandlerBase

    protected override string FolderName => "Routes";

    protected override string CardModelRootNode => "RouteRecord";

    protected override IEnumerable<StandardRoute> ReadComponents(RecordRefModel model, string componentFolderPath)
    {
      var route = new StandardRoute
      {
        Name = model.Code,
        Title = model.Name
      };
      var stateReq = model.Requisites
        .FirstOrDefault(r => r.Name == StateReqName);
      if (stateReq != null)
        route.State = stateReq.Value == StateActiveRequisiteValue ? ComponentState.Active : ComponentState.Closed;
      var descriptionFile = Path.Combine(componentFolderPath, "Properties.xml");
      if (File.Exists(descriptionFile))
      {
        var descriptionData = File.ReadAllText(descriptionFile, Encoding.GetEncoding(1251));
        route.WorkflowDescription = WorkflowDescriptionParser.Parse(descriptionData);
        ReadActions(route.WorkflowDescription, componentFolderPath);
        ReadBlocks(route.WorkflowDescription, componentFolderPath);
        ReadEvents(route.WorkflowDescription, componentFolderPath);
      }
      else
        log.Warn($"File not found {descriptionFile}");

      yield return route;
    }

    #endregion

    private static void ReadEvents(WorkflowDescription description, string componentFolderPath)
    {
      foreach (var @event in description.Events)
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

    private static void ReadBlocks(WorkflowDescription description, string componentFolderPath)
    {
      foreach (var block in description.Blocks)
      {
        var blockFolderPath = Path.Combine(componentFolderPath, "Blocks", block.Id);
        ReadActions(block, blockFolderPath);
        ReadEvents(block, blockFolderPath);
        ReadProperties(block, blockFolderPath);
      }
    }

    private static void ReadActions(WorkflowDescription description, string componentFolderPath)
    {
      foreach (var action in description.Actions)
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

    private static void ReadActions(WorkflowBlock block, string blocksFolderPath)
    {
      foreach (var action in block.Actions)
      {
        var actionFile = Path.Combine(blocksFolderPath, "Actions", $"{action.Name}.isbl");
        if (File.Exists(actionFile))
        {
          action.CalculationText = File.ReadAllText(actionFile, Encoding.GetEncoding(1251));
        }
        else
          log.Warn($"File not found {actionFile}");
      }
    }

    private static void ReadEvents(WorkflowBlock block, string blocksFolderPath)
    {
      foreach (var @event in block.Events)
      {
        if (blockEventFileNames.TryGetValue(@event.Name, out string eventFileName))
        {
          var eventFile = Path.Combine(blocksFolderPath, "Events", eventFileName);
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

    private static void ReadProperties(WorkflowBlock block, string blocksFolderPath)
    {
      foreach (var property in block.IsblProperties)
      {
        if (blockEventFileNames.TryGetValue(property.Name, out string propertyFileName))
        {
          var propertyCalculationFile = Path.Combine(blocksFolderPath, "Events", propertyFileName);
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
  }
}
