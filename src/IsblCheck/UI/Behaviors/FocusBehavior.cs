using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace IsblCheck.UI.Behaviors
{
  /// <summary>
  /// Расширение для работы с фокусом UI-элемента.
  /// </summary>
  // Взято отсюда: http://stackoverflow.com/questions/1356045/set-focus-on-textbox-in-wpf-from-view-model-c
  public static class FocusBehavior
  {
    public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(FocusBehavior),
          new UIPropertyMetadata(false, null, OnCoerceValue));

    /// <summary>
    /// Получить значение AttachedProperty.
    /// </summary>
    /// <param name="element">UI-элемент.</param>
    /// <returns>Значение свойства.</returns>
    public static bool? GetIsFocused(DependencyObject element)
    {
      if (element == null)
        throw new ArgumentNullException("element");

      return (bool?)element.GetValue(IsFocusedProperty);
    }

    /// <summary>
    /// Установить значение AttachedProperty.
    /// </summary>
    /// <param name="element">UI-элемент.</param>
    /// <param name="value">Значение.</param>
    public static void SetIsFocused(DependencyObject element, bool? value)
    {
      if (element == null)
        throw new ArgumentNullException("element");

      element.SetValue(IsFocusedProperty, value);
    }

    /// <summary>
    /// Callback для подавления значения.
    /// </summary>
    /// <param name="d">Свойство зависимости.</param>
    /// <param name="baseValue">Базовое значение.</param>
    /// <returns>Исправленное значение.</returns>
    private static object OnCoerceValue(DependencyObject d, object baseValue)
    {
      // WPF не вызывает PropertyChanged, если значение свойства не менялось, поэтому устанавливаем фокус здесь.
      if ((bool)baseValue)
      {
        Dispatcher.CurrentDispatcher.BeginInvoke(
          DispatcherPriority.Input,
          new Action(() => Keyboard.Focus((UIElement)d)));
      }

      return ((bool)baseValue);
    }
  }
}
