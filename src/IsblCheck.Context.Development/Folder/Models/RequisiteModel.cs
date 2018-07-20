using System;
using System.Text;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Folder.Models
{
  /// <summary>
  /// Модель реквизита.
  /// </summary>
  public class RequisiteModel
  {
    /// <summary>
    /// Имя.
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; }


    /// <summary>
    /// Значение.
    /// </summary>
    [XmlAttribute("Value")]
    public string Value { get; set; }

    /// <summary>
    /// Номер строки в детальном разделе.
    /// </summary>
    [XmlAttribute("NumStr")]
    public string Number { get; set; }

    /// <summary>
    /// Запись справочника.
    /// </summary>
    [XmlElement("RecordRef")]
    public RecordRefModel Record { get; set; }

    /// <summary>
    /// Тип элемента.
    /// </summary>
    [XmlAttribute("Текст")]
    public string TypeRu { get; set; }

    /// <summary>
    /// Тип элемента для английского пакета.
    /// </summary>
    [XmlAttribute("Text")]
    public string TypeEn { get; set; }

    /// <summary>
    /// Тип элемента.
    /// </summary>
    [XmlIgnore]
    public string Type
    {
      get
      {
        if (!string.IsNullOrEmpty(this.TypeRu))
          return this.TypeRu;
        return this.TypeEn;
      }
    }

    /// <summary>
    /// Текст.
    /// </summary>
    [XmlText]
    public string Text { get; set; }

    /// <summary>
    /// Декодированное значение текстового поля.
    /// </summary>
    [XmlIgnore]
    public string DecodedValue
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.Text))
          return string.Empty;
        var encoding = Encoding.GetEncoding(1251);
        return encoding.GetString(Convert.FromBase64String(this.Text));
      }
    }
  }
}
