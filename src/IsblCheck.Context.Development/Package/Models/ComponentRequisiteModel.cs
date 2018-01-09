using System;
using System.Text;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Package.Models
{
  /// <summary>
  /// Реквизит компоненты.
  /// </summary>
  public class ComponentRequisiteModel
  {
    /// <summary>
    /// Код реквизита.
    /// </summary>
    [XmlAttribute("Code")]
    public string Code { get; set; }

    /// <summary>
    /// Значение реквизита.
    /// </summary>
    [XmlAttribute("Value")]
    public string Value { get; set; }

    /// <summary>
    /// Локализованное значение реквизита.
    /// </summary>
    [XmlAttribute("ValueLocalizeID")]
    public string ValueLocalizeID { get; set; }

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
    public string DecodedText
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
