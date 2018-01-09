using IsblCheck.Common.Patterns;
using System;

namespace IsblCheck.Common.Settings
{
  /// <summary>
  /// Менеджер настроек.
  /// </summary>
  public class SettingsManager : Singleton<SettingsManager>
  {
    #region Поля и свойства

    /// <summary>
    /// Провайдер локализации.
    /// </summary>
    public ISettingsProvider SettingsProvider { get; set; }

    #endregion

    #region События

    /// <summary>
    /// Событие изменения настройки.
    /// </summary>
    public event EventHandler<string> SettingChanged;


    /// <summary>
    /// Сгенерировать событие изменения настройки.
    /// </summary>
    /// <param name="settingKey">Имя настройки.</param>
    private void OnSettingChanged(string settingKey)
    {
      EventHandler<string> handler = this.SettingChanged;
      if (handler != null)
        handler.Invoke(this, settingKey);
    }

    #endregion

    #region Методы

    /// <summary>
    /// Получить значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ настройки.</param>
    /// <returns>Значение.</returns>
    public T GetValue<T>(string key)
    {
      if (this.SettingsProvider == null)
        return default(T);

      return this.SettingsProvider.GetValue<T>(key);
    }

    /// <summary>
    /// Установить значение.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    public void SetValue<T>(string key, T value)
    {
      if (this.SettingsProvider == null)
        return;

      this.SettingsProvider.SetValue(key, value);
      this.SettingsProvider.Save();
      this.OnSettingChanged(key);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected SettingsManager()
    {
    }

    #endregion
  }
}
