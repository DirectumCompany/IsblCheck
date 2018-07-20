using System;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace IsblCheck.UI.Behaviors
{
  /// <summary>
  /// Поведение положения курсора.
  /// </summary>
  public class CaretOffsetBehavior : Behavior<TextEditor>
  {
    /// <summary>
    /// Каретка текущего представления.
    /// </summary>
    private Caret caret;

    /// <summary>
    /// Изменение исходным объектом.
    /// </summary>
    private bool isSourceChanging;

    /// <summary>
    /// Положение каретки.
    /// </summary>
    public static DependencyProperty CaretOffsetProperty =
      DependencyProperty.RegisterAttached("CaretOffset", typeof(int),
        typeof(CaretOffsetBehavior), new UIPropertyMetadata(0, CaretOffsetPropertyChangedHandler));

    /// <summary>
    /// Положение каретки.
    /// </summary>
    public int CaretOffset
    {
      get { return (int)this.GetValue(CaretOffsetProperty); }
      set { this.SetValue(CaretOffsetProperty, value); }
    }

    /// <summary>
    /// Обработчик события изменения п
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void CaretOffsetPropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as CaretOffsetBehavior;

      if (behavior?.caret == null)
        return;

      if (behavior.isSourceChanging)
        return;

      behavior.caret.PositionChanged -= behavior.CaretPositionChangedHandler;
      behavior.caret.Offset = (int)e.NewValue;
      behavior.AssociatedObject.ScrollToLine(behavior.caret.Line);
      behavior.caret.PositionChanged += behavior.CaretPositionChangedHandler;
    }

    /// <summary>
    /// Обработчик события присоединения поведения.
    /// </summary>
    protected override void OnAttached()
    {
      base.OnAttached();

      this.AssociatedObject.Loaded += this.AssociatedObjectLoadedHandler;
      this.AssociatedObject.Unloaded += this.AssociatedObjectUnloadedHandler;
    }

    /// <summary>
    /// Обработчик события отсоединения поведения.
    /// </summary>
    protected override void OnDetaching()
    {
      base.OnDetaching();

      this.AssociatedObject.Loaded -= this.AssociatedObjectLoadedHandler;
      this.AssociatedObject.Unloaded -= this.AssociatedObjectUnloadedHandler;
    }

    /// <summary>
    /// Обработчик события загрузки контрола.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void AssociatedObjectLoadedHandler(object sender, RoutedEventArgs e)
    {
      if (this.AssociatedObject.TextArea == null)
        return;

      this.caret = this.AssociatedObject.TextArea.Caret;
      if (this.caret == null)
        return;

      this.caret.Offset = this.CaretOffset;
      this.AssociatedObject.ScrollToLine(this.caret.Line);
      this.caret.PositionChanged += this.CaretPositionChangedHandler;
    }

    /// <summary>
    /// Обработчик события выгрузки контрола.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void AssociatedObjectUnloadedHandler(object sender, RoutedEventArgs e)
    {
      if (this.caret != null)
        this.caret.PositionChanged -= this.CaretPositionChangedHandler;
    }

    /// <summary>
    /// Обработчик события изменения позиции каретки.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void CaretPositionChangedHandler(object sender, EventArgs e)
    {
      this.isSourceChanging = true;
      this.CaretOffset = this.caret.Offset;
      this.isSourceChanging = false;
    }
  }
}
