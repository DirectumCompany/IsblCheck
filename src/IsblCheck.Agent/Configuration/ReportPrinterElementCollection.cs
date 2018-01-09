using System;
using System.Configuration;

namespace IsblCheck.Agent.Configuration
{
  /// <summary>
  /// Коллекция конфигураций принтеров отчетов.
  /// </summary>
  [ConfigurationCollection(typeof(ReportPrinterElement), AddItemName = "reportPrinter")]
  public class ReportPrinterElementCollection : ConfigurationElementCollection
  {
    /// <summary>
    /// Создать новый элемент коллекции.
    /// </summary>
    /// <returns>Новый элемент коллекции.</returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new ReportPrinterElement();
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
