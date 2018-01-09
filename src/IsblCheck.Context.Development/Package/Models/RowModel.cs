using System.Collections.Generic;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Package.Models
{
  /// <summary>
  /// Модель записи детального раздела.
  /// </summary>
  public class RowModel
  {
    /// <summary>
    /// Реквизиты записи.
    /// </summary>
    [XmlElement("Requisite")]
    public List<ComponentRequisiteModel> Requisites { get; set; }
  }
}
