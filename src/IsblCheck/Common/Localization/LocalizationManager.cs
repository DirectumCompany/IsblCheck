using IsblCheck.Common.Patterns;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace IsblCheck.Common.Localization
{
  /// <summary>
  /// Менеджер локализации.
  /// </summary>
  public class LocalizationManager : Singleton<LocalizationManager>
  {
    #region Поля и свойства

    /// <summary>
    /// Текущая культура.
    /// </summary>
    public CultureInfo CurrentCulture
    {
      get { return Thread.CurrentThread.CurrentCulture; }
      set
      {
        if (Equals(value, Thread.CurrentThread.CurrentUICulture))
          return;
        Thread.CurrentThread.CurrentCulture = value;
        Thread.CurrentThread.CurrentUICulture = value;
        CultureInfo.DefaultThreadCurrentCulture = value;
        CultureInfo.DefaultThreadCurrentUICulture = value;
        this.OnCultureChanged();
      }
    }

    /// <summary>
    /// Культуры.
    /// </summary>
    public IEnumerable<CultureInfo> Cultures
    {
      get
      {
        if (this.LocalizationProvider == null || this.LocalizationProvider.Cultures == null)
          return Enumerable.Empty<CultureInfo>();

        return this.LocalizationProvider.Cultures;
      }
    }

    /// <summary>
    /// Провайдер локализации.
    /// </summary>
    public ILocalizationProvider LocalizationProvider { get; set; }

    #endregion

    #region События

    /// <summary>
    /// Событие изменения культуры.
    /// </summary>
    public event EventHandler CultureChanged;

    /// <summary>
    /// Сгенерировать событие изменения культуры.
    /// </summary>
    private void OnCultureChanged()
    {
      EventHandler handler = this.CultureChanged;
      if (handler != null)
        handler.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Методы

    /// <summary>
    /// Получить локализацию по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Локализация.</returns>
    public string LocalizeString(string key)
    {
      if (string.IsNullOrEmpty(key))
        return "[NULL]";

      if (LocalizationProvider == null)
        return string.Format("[{0}]", key);

      var localizedValue = this.LocalizationProvider.LocalizeString(key);
      if (localizedValue == null)
        return string.Format("[{0}]", key);

      return localizedValue;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected LocalizationManager()
    {
    }

    #endregion
  }
}
