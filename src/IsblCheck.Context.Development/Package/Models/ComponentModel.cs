using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Package.Models
{
  /// <summary>
  /// Модель компоненты.
  /// </summary>
  public class ComponentModel
  {
    /// <summary>
    /// Ключевое значение.
    /// </summary>
    [XmlAttribute("KeyValue")]
    public string KeyValue { get; set; }

    /// <summary>
    /// Отображаемое значение.
    /// </summary>
    [XmlAttribute("DisplayValue")]
    public string DisplayValue { get; set; }

    /// <summary>
    /// Тип связанного справочника.
    /// </summary>
    /// <remarks>
    /// Для интегрированных отчетов.
    /// </remarks>
    [XmlAttribute("ReferenceName")]
    public string ReferenceName { get; set; }

    /// <summary>
    /// Главный раздел компоненты.
    /// </summary>
    [XmlElement("Requisites")]
    public RowModel Card { get; set; }

    /// <summary>
    /// Детальные разделы компоненты.
    /// </summary>
    [XmlElement("DetailDataSet")]
    public DataSetsModel DetailDataSets { get; set; }
  }
}
