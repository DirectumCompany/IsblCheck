using System.Collections.Generic;
using System.Globalization;

namespace IsblCheck.Common.Localization
{
  /// <summary>
  /// Интерфейс провайдера локализации.
  /// </summary>
  public interface ILocalizationProvider
  {
    /// <summary>
    /// Культуры.
    /// </summary>
    IEnumerable<CultureInfo> Cultures { get; }

    /// <summary>
    /// Получить локализацию по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Локализация.</returns>
    string LocalizeString(string key);
  }
}
