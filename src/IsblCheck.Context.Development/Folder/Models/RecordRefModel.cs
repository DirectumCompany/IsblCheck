using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Folder.Models
{
  /// <summary>
  /// Модель записи справочника.
  /// </summary>
  public class RecordRefModel
  {
    /// <summary>
    /// Имя справочника.
    /// </summary>
    [XmlAttribute("Vid")]
    public string ReferenceName { get; set; }

    /// <summary>
    /// Код записи.
    /// </summary>
    [XmlAttribute("Kod")]
    public string Code { get; set; }

    /// <summary>
    /// Имя записи.
    /// </summary>
    [XmlAttribute("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Реквизиты.
    /// </summary>
    [XmlElement("Requisite")]
    public List<RequisiteModel> Requisites { get; set; }
  }
}
