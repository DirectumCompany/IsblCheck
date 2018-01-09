using System.Configuration;

namespace IsblCheck.Agent.Configuration
{
  /// <summary>
  /// Секция конфигурации агента.
  /// </summary>
  public class AgentConfigurationSection : ConfigurationSection
  {
    /// <summary>
    /// Папка где располагаются сборки правил.
    /// </summary>
    [ConfigurationProperty("ruleLibraryPath", DefaultValue = "Rules")]
    public string RuleLibraryPath
    {
      get { return (string)this["ruleLibraryPath"]; }
      set { this["ruleLibraryPath"] = value; }
    }

    /// <summary>
    /// Коллекция конфигураций провайдеров контекста разработки.
    /// </summary>
    [ConfigurationProperty("contextProviders")]
    public ContextProviderElementCollection ContextProviders
    {
      get { return (ContextProviderElementCollection)this["contextProviders"]; }
      set { this["contextProviders"] = value; }
    }

    /// <summary>
    /// Коллекция конфигураций принтеров отчетов.
    /// </summary>
    [ConfigurationProperty("reportPrinters")]
    public ReportPrinterElementCollection ReportPrinters
    {
      get { return (ReportPrinterElementCollection)this["reportPrinters"]; }
      set { this["reportPrinters"] = value; }
    }
  }
}
