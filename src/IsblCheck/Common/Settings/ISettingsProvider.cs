namespace IsblCheck.Common.Settings
{
  /// <summary>
  /// Интерфейс провайдера настроек.
  /// </summary>
  public interface ISettingsProvider
  {
    /// <summary>
    /// Получить значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ настройки.</param>
    /// <returns>Значение.</returns>
    T GetValue<T>(string key);

    /// <summary>
    /// Сохранить настройки.
    /// </summary>
    /// <returns>true, в случае успешного сохранения, иначе false.</returns>
    void Save();

    /// <summary>
    /// Установить значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    void SetValue<T>(string key, T value);
  }
}
