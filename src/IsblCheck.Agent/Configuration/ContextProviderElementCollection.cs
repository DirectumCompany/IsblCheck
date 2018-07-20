using System.Configuration;

namespace IsblCheck.Agent.Configuration
{
  /// <summary>
  /// Коллекция конфигураций провайдеров контекста разработки.
  /// </summary>
  [ConfigurationCollection(typeof(ContextProviderElement))]
  public class ContextProviderElementCollection : ConfigurationElementCollection
  {
    /// <summary>
    /// Создать новый элемент коллекции.
    /// </summary>
    /// <returns>Новый элемент коллекции.</returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new ContextProviderElement();
    }

    /// <summary>
    /// Получить ключ элемента.
    /// </summary>
    /// <param name="element">Элемент.</param>
    /// <returns>Ключ элемента.</returns>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return (element as BaseKeyElement).Key;
    }
  }
}
