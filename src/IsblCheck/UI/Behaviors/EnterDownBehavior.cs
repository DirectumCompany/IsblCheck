using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IsblCheck.UI.Behaviors
{
  /// <summary>
  /// Класс поведения при нажатии клавиши Enter.
  /// </summary>
  public class EnterDownBehavior
  {
    /// <summary>
    /// Команда.
    /// </summary>
    public static DependencyProperty CommandProperty =
      DependencyProperty.RegisterAttached("Command", typeof(ICommand),
        typeof(EnterDownBehavior), new UIPropertyMetadata(CommandChangedHandler));

    /// <summary>
    /// Параметры команды.
    /// </summary>
    public static DependencyProperty CommandParameterProperty =
      DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
        typeof(EnterDownBehavior), new UIPropertyMetadata(null));

    /// <summary>
    /// Получить команду.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    /// <returns>Команда.</returns>
    public static ICommand GetCommand(DependencyObject target)
    {
      return (ICommand)target.GetValue(CommandProperty);
    }

    /// <summary>
    /// Установить команду.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    /// <param name="value">Команда.</param>
    public static void SetCommand(DependencyObject target, ICommand value)
    {
      target.SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Получить параметры команды.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    /// <returns>Параметры команды.</returns>
    public static object GetCommandParameter(DependencyObject target)
    {
      return target.GetValue(CommandParameterProperty);
    }

    /// <summary>
    /// Установить параметры команды.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    /// <param name="value">Параметры команды</param>
    public static void SetCommandParameter(DependencyObject target, object value)
    {
      target.SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Обработчик события изменения команды.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    /// <param name="e">Аргументы события.</param>
    private static void CommandChangedHandler(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      var control = target as Control;
      if (control == null)
        return;

      if ((e.NewValue != null) && (e.OldValue == null))
      {
        control.KeyDown += KeyDownHandler;
      }
      else if ((e.NewValue == null) && (e.OldValue != null))
      {
        control.KeyDown -= KeyDownHandler;
      }
    }

    /// <summary>
    /// Обработчик события нажатия клавиши.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private static void KeyDownHandler(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
        return;

      var control = sender as Control;
      var command = (ICommand)control.GetValue(CommandProperty);
      var commandParameter = control.GetValue(CommandParameterProperty);
      if (command.CanExecute(commandParameter))
      {
        var action = new Action<object>(command.Execute);
        control.Dispatcher.BeginInvoke(action, commandParameter);
      }
    }
  }
}
