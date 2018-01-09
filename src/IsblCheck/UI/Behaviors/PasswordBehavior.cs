using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace IsblCheck.UI.Behaviors
{
  /// <summary>
  /// Класс поведение для пароля.
  /// </summary>
  public class PasswordBehavior : Behavior<PasswordBox>
  {
    #region Поля и свойства

    /// <summary>
    /// Зависимое свойство для пароля.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty =
      DependencyProperty.Register("Password", typeof(SecureString),
        typeof(PasswordBehavior), new PropertyMetadata(null));

    /// <summary>
    /// Пароль.
    /// </summary>
    public SecureString Password
    {
      get { return (SecureString)this.GetValue(PasswordProperty); }
      set { this.SetValue(PasswordProperty, value); }
    }

    #endregion

    #region Методы

    /// <summary>
    /// Обработчик события присоединения поведения.
    /// </summary>
    protected override void OnAttached()
    {
      this.AssociatedObject.PasswordChanged += this.PasswordChangedHandler;

      base.OnAttached();
    }

    /// <summary>
    /// Обработчик события отсоединения поведения.
    /// </summary>
    protected override void OnDetaching()
    {
      this.AssociatedObject.PasswordChanged -= this.PasswordChangedHandler;

      base.OnDetaching();
    }

    /// <summary>
    /// Обработчик события изменения пароля.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void PasswordChangedHandler(object sender, RoutedEventArgs e)
    {
      var binding = BindingOperations.GetBindingExpression(this, PasswordProperty);
      if (binding != null)
      {
        var property = binding.DataItem.GetType()
           .GetProperty(binding.ParentBinding.Path.Path);
        if (property != null)
          property.SetValue(binding.DataItem, this.AssociatedObject.SecurePassword, null);
      }
    }

    #endregion
  }
}
