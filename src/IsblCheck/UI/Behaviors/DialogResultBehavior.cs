using System.Windows;
using System.Windows.Interactivity;

namespace IsblCheck.UI.Behaviors
{
  /// <summary>
  /// Поведение результата диалога.
  /// </summary>
  public class DialogResultBehavior : Behavior<Window>
  {
    /// <summary>
    /// Зависимое свойство результата диалога.
    /// </summary>
    public static readonly DependencyProperty DialogResultProperty =
      DependencyProperty.Register("DialogResult", typeof(bool?),
        typeof(DialogResultBehavior), new PropertyMetadata(DialogResultPropertyChangedHandler));

    /// <summary>
    /// Результат диалога.
    /// </summary>
    public bool? DialogResult
    {
      get { return (bool?)this.GetValue(DialogResultProperty); }
      set { this.SetValue(DialogResultProperty, value); }
    }

    /// <summary>
    /// Обработчик события изменения свойства Результат диалога.
    /// </summary>
    /// <param name="d">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private static void DialogResultPropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as DialogResultBehavior;
      if (behavior == null)
        return;

      behavior.AssociatedObject.DialogResult = e.NewValue as bool?;
    }
  }
}
