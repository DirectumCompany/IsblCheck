using System.Configuration;

namespace IsblCheck.Agent.Configuration
{
  /// <summary>
  /// Конфигурация провайдера контекста разработки.
  /// </summary>
  public class ContextProviderElement : BaseKeyElement
  {
    /// <summary>
    /// Тип провайдера.
    /// </summary>
    [ConfigurationProperty("provider", IsRequired = true)]
    public ContextProviderType Provider
    {
      get { return (ContextProviderType)this["provider"]; }
      set { this["provider"] = value; }
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

    /// <summary>
    /// Путь к папке.
    /// </summary>
    [ConfigurationProperty("folderPath")]
    public string FolderPath
    {
      get { return (string)this["folderPath"]; }
      set { this["folderPath"] = value; }
    }

    /// <summary>
    /// Строка подключения.
    /// </summary>
    [ConfigurationProperty("connectionString")]
    public string ConnectionString
    {
      get { return (string)this["connectionString"]; }
      set { this["connectionString"] = value; }
    }
  }
}
