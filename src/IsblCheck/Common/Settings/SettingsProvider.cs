namespace IsblCheck.Common.Settings
{
  /// <summary>
  /// Провайдер настроек.
  /// </summary>
  public class SettingsProvider : ISettingsProvider
  {
    /// <summary>
    /// Получить значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ настройки.</param>
    /// <returns>Значение.</returns>
    public T GetValue<T>(string key)
    {
      var value = Properties.Settings.Default[key];
      if (value == null)
        return default(T);
      return (T)value;
    }

    /// <summary>
    /// Сохранить настройки.
    /// </summary>
    /// <returns>true, в случае успешного сохранения, иначе false.</returns>
    public void Save()
    {
      Properties.Settings.Default.Save();
    }

    /// <summary>
    /// Установить значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    public void SetValue<T>(string key, T value)
    {
      Properties.Settings.Default[key] = value;
    }
  }
}
