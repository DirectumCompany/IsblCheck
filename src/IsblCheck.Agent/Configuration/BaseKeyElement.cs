using System;
using System.Configuration;

namespace IsblCheck.Agent.Configuration
{
  /// <summary>
  /// Базовый элемент с ключом.
  /// </summary>
  public class BaseKeyElement : ConfigurationElement
  {
    /// <summary>
    /// Ключ.
    /// </summary>
    public string Key = Guid.NewGuid().ToString();
  }
}
