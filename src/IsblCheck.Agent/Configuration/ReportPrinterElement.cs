using System.Configuration;

namespace IsblCheck.Agent.Configuration
{
  /// <summary>
  /// Конфигурация фабрики притеров отчетов.
  /// </summary>
  public class ReportPrinterElement : BaseKeyElement
  {
    /// <summary>
    /// Тип провайдера.
    /// </summary>
    [ConfigurationProperty("printer", IsRequired = true)]
    public ReportPrinterType Printer
    {
      get { return (ReportPrinterType)this["printer"]; }
      set { this["printer"] = value; }
    }

    /// <summary>
    /// Путь к файлу.
    /// </summary>
    [ConfigurationProperty("filePath")]
    public string FilePath
    {
      get { return (string)this["filePath"]; }
      set { this["filePath"] = value; }
    }
  }
}
