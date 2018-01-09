using IsblCheck.Common.Localization;
using System;
using System.Windows.Data;

namespace IsblCheck.UI.Localization
{
  /// <summary>
  /// Биндинг локализации.
  /// </summary>
  public class LocalizationBinding
  {
    #region Поля и свойства

    /// <summary>
    /// Выражение для биндинга.
    /// </summary>
    private BindingExpressionBase bindingExpression;

    #endregion

    #region Методы

    /// <summary>
    /// Задать биндинг.
    /// </summary>
    /// <param name="bindingExpression">Выражение для биндинга.</param>
    public void SetBinding(BindingExpressionBase bindingExpression)
    {
      this.bindingExpression = bindingExpression;
    }

    /// <summary>
    /// Обработчик события изменения культуры.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события</param>
    private void CultureChangedHandler(object sender, EventArgs e)
    {
      try
      {
        if (this.bindingExpression != null)
          this.bindingExpression.UpdateTarget();
      }
      catch
      {
      }
    }

    #endregion

    #region Конструкторы и деструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LocalizationBinding()
    {
      LocalizationManager.Instance.CultureChanged += CultureChangedHandler;
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~LocalizationBinding()
    {
      LocalizationManager.Instance.CultureChanged -= CultureChangedHandler;
    }

    #endregion
  }
}
