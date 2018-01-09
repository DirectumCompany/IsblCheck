using System.Collections.Generic;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Package.Models
{
  /// <summary>
  /// Модель списка компонент.
  /// </summary>
  [XmlRoot("Components")]
  public class ComponentsModel
  {
    /// <summary>
    /// Модель констант.
    /// </summary>
    [XmlArray("Constants")]
    [XmlArrayItem("Constants")]
    public List<ComponentModel> Constants { get; set; }

    /// <summary>
    /// Модель реквизитов диалогов.
    /// </summary>
    [XmlArray("DialogRequisites")]
    [XmlArrayItem("DialogRequisites")]
    public List<ComponentModel> DialogRequisites { get; set; }

    /// <summary>
    /// Модель диалогов.
    /// </summary>
    [XmlArray("Dialogs")]
    [XmlArrayItem("Dialogs")]
    public List<ComponentModel> Dialogs { get; set; }

    /// <summary>
    /// Модели типов карточек электронных документов.
    /// </summary>
    [XmlArray("EDCardTypes")]
    [XmlArrayItem("EDCardTypes")]
    public List<ComponentModel> DocumentCardTypes { get; set; }

    /// <summary>
    /// Модели реквизитов документов.
    /// </summary>
    [XmlArray("EDocRequisites")]
    [XmlArrayItem("EDocRequisites")]
    public List<ComponentModel> DocumentRequisites { get; set; }

    /// <summary>
    /// Модели групп функций.
    /// </summary>
    [XmlArray("GrFunctions")]
    [XmlArrayItem("GrFunctions")]
    public List<ComponentModel> FunctionGroups { get; set; }

    /// <summary>
    /// Модели функций.
    /// </summary>
    [XmlArray("Functions")]
    [XmlArrayItem("Functions")]
    public List<ComponentModel> Functions { get; set; }

    /// <summary>
    /// Модели строк локализации.
    /// </summary>
    [XmlArray("LocalizedStrings")]
    [XmlArrayItem("LocalizedStrings")]
    public List<ComponentModel> LocalizationStrings { get; set; }

    /// <summary>
    /// Модели модулей.
    /// </summary>
    [XmlArray("Modules")]
    [XmlArrayItem("Modules")]
    public List<ComponentModel> Modules { get; set; }

    /// <summary>
    /// Модели реквизитов справочников.
    /// </summary>
    [XmlArray("RefRequisites")]
    [XmlArrayItem("RefRequisites")]
    public List<ComponentModel> ReferenceRequisites { get; set; }

    /// <summary>
    /// Модели типов справочников.
    /// </summary>
    [XmlArray("RefTypes")]
    [XmlArrayItem("RefTypes")]
    public List<ComponentModel> ReferenceTypes { get; set; }

    /// <summary>
    /// Модели отчетов.
    /// </summary>
    [XmlArray("Reports")]
    [XmlArrayItem("Reports")]
    public List<ComponentModel> Reports { get; set; }

    /// <summary>
    /// Модели групп блоков типовых маршрутов.
    /// </summary>
    [XmlArray("WorkflowBlockGroups")]
    [XmlArrayItem("WorkflowBlockGroups")]
    public List<ComponentModel> RouteBlockGroups { get; set; }

    /// <summary>
    /// Модели блоков типовых маршрутов.
    /// </summary>
    [XmlArray("WorkflowBlocks")]
    [XmlArrayItem("WorkflowBlocks")]
    public List<ComponentModel> RouteBlocks { get; set; }

    /// <summary>
    /// Модели сценариев.
    /// </summary>
    [XmlArray("Scripts")]
    [XmlArrayItem("Scripts")]
    public List<ComponentModel> Scripts { get; set; }

    /// <summary>
    /// Модели приложений просмотрщиков.
    /// </summary>
    [XmlArray("Viewers")]
    [XmlArrayItem("Viewers")]
    public List<ComponentModel> Viewers { get; set; }
  }
}
