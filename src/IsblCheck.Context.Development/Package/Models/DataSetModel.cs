using System.Collections.Generic;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Package.Models
{
  /// <summary>
  /// Модель детального раздела.
  /// </summary>
  public class DataSetModel
  {
    /// <summary>
    /// Записи детального раздела.
    /// </summary>
    [XmlElement("Requisites")]
    public List<RowModel> Rows { get; set; }
  }
}
