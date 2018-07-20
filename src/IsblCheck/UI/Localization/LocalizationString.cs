using System;
using GalaSoft.MvvmLight;
using IsblCheck.Common.Localization;

namespace IsblCheck.UI.Localization
{
  /// <summary>
  /// Строка локализации.
  /// </summary>
  public class LocalizationString : ObservableObject
  {
    #region Поля и свойства

    /// <summary>
    /// Ключ.
    /// </summary>
    private readonly string key;

    /// <summary>
    /// Аргументы.
    /// </summary>
    private readonly object[] args;

    /// <summary>
    /// Значение.
    /// </summary>
    public string Value
    {
      get
      {
        var value = LocalizationManager.Instance.LocalizeString(key);
        if (args != null)
          value = string.Format(value, args);
        return value;
      }
      set { }
    }

    #endregion

    #region Методы

    /// <summary>
    /// Обработчик события изменения культуры.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события</param>
    private void CultureChangedHandler(object sender, EventArgs e)
    {
      this.RaisePropertyChanged("Value");
    }

    #endregion

    #region Конструкторы и деструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="args"></param>
    public LocalizationString(string key, object[] args)
    {
      this.key = key;
      this.args = args;
      LocalizationManager.Instance.CultureChanged += this.CultureChangedHandler;
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~LocalizationString()
    {
      LocalizationManager.Instance.CultureChanged -= this.CultureChangedHandler;
    }

    #endregion
  }
}
