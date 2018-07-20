using System.Collections.Generic;
using System.Globalization;
using IsblCheck.Properties;

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
    private readonly IList<CultureInfo> cultures = new List<CultureInfo>
    {
      new CultureInfo("ru-RU"),
      new CultureInfo("en-US")
    };

    /// <summary>
    /// Список культур.
    /// </summary>
    public IEnumerable<CultureInfo> Cultures => this.cultures;

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
