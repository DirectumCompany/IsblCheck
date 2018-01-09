using IsblCheck.Properties;
using System.Collections.Generic;
using System.Globalization;

namespace IsblCheck.Common.Localization
{
  /// <summary>
  /// Провайдер локализации.
  /// </summary>
  public class ResxLocalizationProvider : ILocalizationProvider
  {
    /// <summary>
    /// Список культур.
    /// </summary>
    private IList<CultureInfo> cultures = new List<CultureInfo>
    {
      new CultureInfo("ru-RU"),
      new CultureInfo("en-US")
    };

    /// <summary>
    /// Список культур.
    /// </summary>
    public IEnumerable<CultureInfo> Cultures { get { return this.cultures; } }

    /// <summary>
    /// Получить локализацию по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Локализация.</returns>
    public string LocalizeString(string key)
    {
      return Resources.ResourceManager.GetString(key);
    }
  }
}
