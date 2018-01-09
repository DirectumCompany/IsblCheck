using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using IsblCheck.UI.Editor;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace IsblCheck.UI.Behaviors
{
  /// <summary>
  /// Поведение для отрисовки маркеров.
  /// </summary>
  public class TextMarkerViewBehavior : Behavior<TextEditor>
  {
    #region Поля и свойства

    /// <summary>
    /// Рендерер маркеров.
    /// </summary>
    private TextMarkerViewRenderer renderer = new TextMarkerViewRenderer();

    /// <summary>
    /// Декоратор скролбара.
    /// </summary>
    private TextMarkerTrackAdorner trackAdorner;

    /// <summary>
    /// Сервис подсказок.
    /// </summary>
    private TextMarkerTooltipService tooltipService;

    /// <summary>
    /// Маркеры.
    /// </summary>
    public static DependencyProperty MarkersProperty =
      DependencyProperty.RegisterAttached("Markers", typeof(TextSegmentCollection<TextMarker>),
        typeof(TextMarkerViewBehavior), new UIPropertyMetadata(null, MarkersPropertyChangedHandler));

    /// <summary>
    /// Маркеры.
    /// </summary>
    public TextSegmentCollection<TextMarker> Markers
    {
      get { return (TextSegmentCollection<TextMarker>)this.GetValue(MarkersProperty); }
      set { this.SetValue(MarkersProperty, value); }
    }

    #endregion

    #region Методы

    /// <summary>
    /// Обработчик события изменения 
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void MarkersPropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as TextMarkerViewBehavior;
      if (behavior == null)
        return;

      behavior.renderer.Markers = e.NewValue as TextSegmentCollection<TextMarker>;
      if (behavior.trackAdorner != null)
        behavior.trackAdorner.Markers = e.NewValue as TextSegmentCollection<TextMarker>;
      if (behavior.tooltipService != null)
        behavior.tooltipService.Markers = e.NewValue as TextSegmentCollection<TextMarker>;
    }

    /// <summary>
    /// Обработчик присоединения поведения.
    /// </summary>
    protected override void OnAttached()
    {
      this.AssociatedObject.Loaded += this.AssociatedObjectLoadedHandler;
      this.AssociatedObject.Unloaded += this.AssociatedObjectUnloadedHandler;
      base.OnAttached();
    }

    /// <summary>
    /// Обработчик отсоединения поведения.
    /// </summary>
    protected override void OnDetaching()
    {
      this.AssociatedObject.Loaded -= this.AssociatedObjectLoadedHandler;
      this.AssociatedObject.Unloaded -= this.AssociatedObjectUnloadedHandler;
      base.OnDetaching();
    }

    /// <summary>
    /// Обработчик события загрузки контрола.
    /// </summary>
    /// <param name="sender">Обработчик события.</param>
    /// <param name="e">Аргументы события.</param>
    private void AssociatedObjectLoadedHandler(object sender, RoutedEventArgs e)
    {
      var textEditor = this.AssociatedObject;

      textEditor.TextArea.TextView.BackgroundRenderers.Add(this.renderer);
      textEditor.TextArea.TextView.LineTransformers.Add(this.renderer);

      this.tooltipService = new TextMarkerTooltipService(this.AssociatedObject);
      this.tooltipService.Markers = this.Markers;

      textEditor.ApplyTemplate();
      var scrollViewer = (ScrollViewer)textEditor.Template.FindName("PART_ScrollViewer", textEditor);
      if (scrollViewer == null)
        return;

      scrollViewer.ApplyTemplate();
      var vScrollBar = (ScrollBar)scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer);
      if (vScrollBar == null)
        return;

      var track = (Track)vScrollBar.Template.FindName("PART_Track", vScrollBar);
      if (track == null)
        return;

      var grid = VisualTreeHelper.GetParent(track) as Grid;
      if (grid == null)
        return;

      var layer = AdornerLayer.GetAdornerLayer(grid);
      if (layer == null)
        return;

      this.trackAdorner = new TextMarkerTrackAdorner(grid, textEditor);
      this.trackAdorner.Markers = this.Markers;
      layer.Add(this.trackAdorner);
    }

    /// <summary>
    /// Обработчик события выгрузки контрола.
    /// </summary>
    /// <param name="sender">Обработчик события.</param>
    /// <param name="e">Аргументы события.</param>
    private void AssociatedObjectUnloadedHandler(object sender, RoutedEventArgs e)
    {
      var textEditor = this.AssociatedObject;

      textEditor.TextArea.TextView.BackgroundRenderers.Remove(this.renderer);
      textEditor.TextArea.TextView.LineTransformers.Remove(this.renderer);

      var scrollViewer = (ScrollViewer)textEditor.Template.FindName("PART_ScrollViewer", textEditor);
      if (scrollViewer == null)
        return;

      var vScrollBar = (ScrollBar)scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer);
      if (vScrollBar == null)
        return;

      var track = (Track)vScrollBar.Template.FindName("PART_Track", vScrollBar);
      if (track == null)
        return;

      var grid = VisualTreeHelper.GetParent(track) as Grid;
      if (grid == null)
        return;

      var layer = AdornerLayer.GetAdornerLayer(grid);
      if (layer == null)
        return;

      layer.Remove(this.trackAdorner);
    }

    #endregion
  }
}
