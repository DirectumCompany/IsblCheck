using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IsblCheck.Context.Development.Utils
{
  /// <summary>
  /// Парсер описания типового маршрута.
  /// </summary>
  public class WorkflowDescriptionParser
  {
    #region Константы

    /// <summary>
    /// Заголовки событий типового маршрута.
    /// </summary>
    private static readonly Dictionary<string, string> RouteEventTitles = new Dictionary<string, string>
    {
      ["InitScript"] = "Начало выбора",
      ["Script"] = "Завершение выбора",
      ["TaskStart"] = "Возможность старта",
      ["TaskAbortPossibility"] = "Возможность прекращения",
      ["TaskAbort"] = "Прекращение"
    };

    /// <summary>
    /// Имена свойств-событий для различных типов блоков.
    /// </summary>
    private static readonly Dictionary<RouteBlockType, string[]> BlockEventNames = new Dictionary<RouteBlockType, string[]>
    {
      [RouteBlockType.Start] = new[]
        {
          "BeforeStart",
          "AfterFinish"
        },
      [RouteBlockType.Finish] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      },
      [RouteBlockType.Notice] = new[]
      {
        "BeforeStart",
        "AfterFinish",
        "OnCreateNotices",
        "OnFormShow",
        "OnFormHide"
      },
      [RouteBlockType.Step] = new[]
      {
        "BeforeStart",
        "AfterFinish",
        "OnCreateJobs",
        "BeforeQueryParams",
        "AfterQueryParams",
        "OnFormShow",
        "OnFormHide"
      },
      [RouteBlockType.Decision] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      },
      [RouteBlockType.Wait] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      },
      [RouteBlockType.Monitor] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      },
      [RouteBlockType.Script] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      },
      [RouteBlockType.Connector] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      },
      [RouteBlockType.SubTask] = new[]
      {
        "BeforeStart",
        "AfterFinish",
        "SubTaskCreate",
        "SubtaskInit",
        "SubtaskStart"
      },
      [RouteBlockType.Pause] = new[]
      {
        "BeforeStart",
        "AfterFinish"
      }
    };

    /// <summary>
    /// Уникальный идентификатор-признак для маркировки текста, закодированного в MIME.
    /// </summary>
    private const string MimeEncodedTextMarker = "{5314B05F-CF9F-4F66-99EC-24992A5FB114}";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// XML описания маршрута.
    /// </summary>
    private readonly XDocument description;

    #endregion


    #region Методы

    /// <summary>
    /// Распарсить описание типового маршрута.
    /// </summary>
    /// <param name="workflowDescriptionXml">XML описания маршрута.</param>
    /// <returns>Описание типового маршрута.</returns>
    public static WorkflowDescription Parse(string workflowDescriptionXml)
    {
      var workflowDescription = new WorkflowDescription();

      var parser = new WorkflowDescriptionParser(workflowDescriptionXml);
      workflowDescription.Actions.AddRange(parser.GetActions());
      workflowDescription.Blocks.AddRange(parser.GetBlocks());
      workflowDescription.Events.AddRange(parser.GetEvents());

      return workflowDescription;
    }

    private IEnumerable<WorkflowBlock> GetBlocks()
    {
      var blocks = this.description.Root?.Element("Blocks")?.Elements("Block");
      if (blocks != null)
      {
        foreach (var block in blocks)
        {
          yield return ParseBlock(block);
        }
      }
    }

    private IEnumerable<Core.Context.Development.Action> GetActions()
    {
      var actions = this.description.Root?.Element("RouteActions")?.Elements("Action");
      if (actions != null)
      {
        foreach (var action in actions)
        {
          yield return ParseAction(action);
        }
      }
    }

    private IEnumerable<WorkflowEvent> GetEvents()
    {
      var events = this.description.Root?.Element("Event")?.Elements();
      if (events != null)
      {
        foreach (var p in events)
        {
          var name = p.Name.LocalName;
          string title;
          if (!RouteEventTitles.TryGetValue(name, out title))
            title = name;
          var calculationText = GetTextValue(p.Value) ?? string.Empty;
          yield return new WorkflowEvent
          {
            Name = name,
            Title = title,
            CalculationText = calculationText
          };
        }
      }
    }

    private static Core.Context.Development.Action ParseAction(XElement actionElement)
    {
      var name = actionElement.Attribute("Code").Value;
      var isblText = actionElement.Element("ISBLText")?.Value;
      var calculation = isblText == null ? string.Empty : DecodeMimeText(isblText);
      return new Core.Context.Development.Action
      {
        Name = name,
        CalculationText = calculation
      };
    }

    private static WorkflowBlock ParseBlock(XElement blockElement)
    {
      var block = new WorkflowBlock();

      block.Id = blockElement.Attribute("ID")?.Value ?? string.Empty;

      RouteBlockType baseBlockType;
      if (!Enum.TryParse(blockElement.Attribute("SystemType")?.Value, out baseBlockType))
        baseBlockType = RouteBlockType.Unknown;
      block.BaseBlockType = baseBlockType;

      var properties = blockElement.Element("Properties")?.Elements("Property");
      if (properties != null)
      {
        var nameProperty = properties.FirstOrDefault(p => p.Attribute("Name")?.Value == "Name");
        if (nameProperty != null)
          block.Name = GetTextPropertyValue(nameProperty) ?? string.Empty;
        else
          block.Name = string.Empty;

        foreach (var p in properties.Where(p => p.Attribute("ValueType")?.Value == "2"))
        {
          var propName = p.Attribute("Name").Value;
          var propTitle = p.Attribute("Description").Value;
          var calculationText = GetTextPropertyValue(p) ?? string.Empty;
          if (IsBlockEvent(propName, baseBlockType))
          {
            block.Events.Add(new WorkflowEvent
            {
              Name = propName,
              Title = propTitle,
              CalculationText = calculationText
            });
          }
          else
          {
            block.IsblProperties.Add(new WorklflowIsblProperty
            {
              Name = propName,
              Title = propTitle,
              CalculationText = calculationText
            });
          }
        }
      }

      var actions = blockElement.Element("Actions")?.Elements("Action");
      if (actions != null)
      {
        foreach (var action in actions)
        {
          block.Actions.Add(ParseAction(action));
        }
      }

      return block;
    }

    private static bool IsBlockEvent(string propertyName, RouteBlockType blockType)
    {
      string[] eventNames;
      if (BlockEventNames.TryGetValue(blockType, out eventNames))
        return eventNames.Contains(propertyName, StringComparer.OrdinalIgnoreCase);
      return false;
    }

    private static string GetTextPropertyValue(XElement p)
    {
      return GetTextValue(p.Element("Value")?.Element("Value")?.Value);
    }

    private static string GetTextValue(string value)
    {
      return value != null && value.StartsWith(MimeEncodedTextMarker) ? DecodeMimeText(value.Substring(MimeEncodedTextMarker.Length)) : value;
    }

    private static string DecodeMimeText(string encodedValue)
    {
      return Encoding.GetEncoding(1251).GetString(Convert.FromBase64String(encodedValue));
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Приватный конструктор.
    /// </summary>
    /// <param name="workflowDescriptionXml">XML описания маршрута.</param>
    private WorkflowDescriptionParser(string workflowDescriptionXml)
    {
      this.description = XDocument.Parse(workflowDescriptionXml);
    }

    #endregion
  }
}
