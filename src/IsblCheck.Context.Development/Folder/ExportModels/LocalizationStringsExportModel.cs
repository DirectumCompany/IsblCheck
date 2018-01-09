using IsblCheck.Context.Development.Package.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Folder.ExportModels
{
  /// <summary>
  /// Модель экспортируемых строк локализации.
  /// </summary>
  [XmlRoot("LocalizationStrings")]
  public class LocalizationStringsExportModel
  {
    /// <summary>
    /// Список строк локализации.
    /// </summary>
    [XmlElement("LocalizationString")]
    public List<ComponentModel> LocalizationStrings { get; set; }

    public LocalizationStringsExportModel()
    {
      this.LocalizationStrings = new List<ComponentModel>();
    }
  }
}
