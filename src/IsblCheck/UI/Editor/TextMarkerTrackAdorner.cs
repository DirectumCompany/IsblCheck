using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;

namespace IsblCheck.UI.Editor
{
  /// <summary>
  /// Декоратор для рисования маркеров.
  /// </summary>
  public class TextMarkerTrackAdorner : Adorner
  {
    #region Поля и свойства

    /// <summary>
    /// Геометрия треугольника.
    /// </summary>
    private static readonly Lazy<Geometry> triangleGeometry = new Lazy<Geometry>(CreateTriangleGeometry);

    /// <summary>
    /// Редактор.
    /// </summary>
    private readonly TextEditor editor;

    /// <summary>
    /// Маркеры.
    /// </summary>
    public TextSegmentCollection<TextMarker> Markers { get; set; }

    #endregion

    #region Методы

    /// <summary>
    /// Создать геометрию треугольника.
    /// </summary>
    /// <returns>Геометрия треугольника.</returns>
    private static Geometry CreateTriangleGeometry()
    {
      var geometry = new StreamGeometry();
      using (var ctx = geometry.Open())
      {
        const double triangleSize = 6.5;
        const double right = triangleSize * 0.866 / 2;
        const double left = -right;
        ctx.BeginFigure(new Point(left, triangleSize / 2), true, true);
        ctx.LineTo(new Point(left, -triangleSize / 2), true, false);
        ctx.LineTo(new Point(right, 0), true, false);
      }
      geometry.Freeze();
      return geometry;
    }

    /// <summary>
    /// Получить кисть с указанным цветом.
    /// </summary>
    /// <param name="color">Цвет.</param>
    /// <returns>Кисть.</returns>
    private static Brush GetBrush(Color color)
    {
      var brush = new SolidColorBrush(color);
      brush.Freeze();
      return brush;
    }

    /// <summary>
    /// Проверить видимость маркера.
    /// </summary>
    /// <param name="marker">Маркер.</param>
    /// <returns>true, если маркер видимый, иначе false.</returns>
    private static bool IsVisibleOnAdorner(TextMarker marker)
    {
      return (marker.MarkerType & (
        TextMarkerType.ScrollBarLeftTriangle | 
        TextMarkerType.ScrollBarRightTriangle |
        TextMarkerType.LineInScrollBar |
        TextMarkerType.CircleInScrollBar)) != 0;
    }

    /// <summary>
    /// Найти следующий маркер.
    /// </summary>
    /// <param name="mousePosition">Позиция курсора мыши.</param>
    /// <returns>Маркер текста.</returns>
    private TextMarker FindNextMarker(Point mousePosition)
    {
      if (this.Markers == null)
        return null;

      var renderSize = this.RenderSize;
      var document = this.editor.Document;
      var textView = this.editor.TextArea.TextView;
      var documentHeight = textView.DocumentHeight;

      var bestMarker = default(TextMarker);
      var bestDistance = double.PositiveInfinity;
      foreach (var marker in this.Markers)
      {
        if (!IsVisibleOnAdorner(marker))
          continue;

        var location = document.GetLocation(marker.StartOffset);
        var visualTop = textView.GetVisualTopByDocumentLine(location.Line);
        var renderPos = visualTop / documentHeight * renderSize.Height;
        var distance = Math.Abs(renderPos - mousePosition.Y);
        if (distance < bestDistance)
        {
          bestDistance = distance;
          bestMarker = marker;
        }
      }
      return bestMarker;
    }

    /// <summary>
    /// Действия выполняемые при нажатии кнопки мыши.
    /// </summary>
    /// <param name="e">Аргументы события.</param>
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);

      var marker = this.FindNextMarker(e.GetPosition(this));
      if (marker == null)
        return;

      var location = this.editor.Document.GetLocation(marker.StartOffset);
      this.editor.ScrollTo(location.Line, location.Column);
      e.Handled = true;
    }

    /// <summary>
    /// Действия выполняемые при отрисовке контрола.
    /// </summary>
    /// <param name="drawingContext">Контекст отрисовки.</param>
    protected override void OnRender(DrawingContext drawingContext)
    {
      if (this.Markers == null)
        return;

      var renderSize = this.RenderSize;
      var document = this.editor.Document;
      var textView = this.editor.TextArea.TextView;
      var documentHeight = textView.DocumentHeight;
      foreach (var marker in this.Markers)
      {
        if (!IsVisibleOnAdorner(marker))
          continue;

        var location = document.GetLocation(marker.StartOffset);
        var visualTop = textView.GetVisualTopByDocumentLine(location.Line);
        var renderPos = visualTop / documentHeight * renderSize.Height;
        var brush = GetBrush(marker.MarkerColor);
        var isMarkerShown = false;

        if ((marker.MarkerType & TextMarkerType.LineInScrollBar) != 0)
        {
          drawingContext.DrawRectangle(brush, null, new Rect(3, renderPos - 1, renderSize.Width - 6, 2));
          isMarkerShown = true;
        }

        if ((marker.MarkerType & TextMarkerType.CircleInScrollBar) != 0)
        {
          const double radius = 3;
          drawingContext.DrawEllipse(brush, null, new Point(renderSize.Width / 2, renderPos), radius, radius);
          isMarkerShown = true;
        }

        if (!isMarkerShown)
        {
          var translateTransform = new TranslateTransform(6, renderPos);
          translateTransform.Freeze();
          drawingContext.PushTransform(translateTransform);

          if ((marker.MarkerType & TextMarkerType.ScrollBarLeftTriangle) != 0)
          {
            var scaleTransform = new ScaleTransform(-1, 1);
            scaleTransform.Freeze();
            drawingContext.PushTransform(scaleTransform);
            drawingContext.DrawGeometry(brush, null, triangleGeometry.Value);
            drawingContext.Pop();
          }

          if ((marker.MarkerType & TextMarkerType.ScrollBarRightTriangle) != 0)
          {
            drawingContext.DrawGeometry(brush, null, triangleGeometry.Value);
          }

          drawingContext.Pop();
        }
      }
    }

    /// <summary>
    /// Действие выполняемые при открытии тултипа.
    /// </summary>
    /// <param name="e">Аргументы события.</param>
    protected override void OnToolTipOpening(ToolTipEventArgs e)
    {
      base.OnToolTipOpening(e);

      var marker = this.FindNextMarker(Mouse.GetPosition(this));
      if (marker?.ToolTip != null)
        this.ToolTip = marker.ToolTip;
      else
        e.Handled = true;
    }

    /// <summary>
    /// Обработчик события изменения визуальных линий.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void VisualLinesChangedHandler(object sender, EventArgs e)
    {
      this.InvalidateVisual();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="adornedElement">Декорируемый элемент.</param>
    /// <param name="editor">Редактор.</param>
    public TextMarkerTrackAdorner(UIElement adornedElement, TextEditor editor) : base(adornedElement)
    {
      this.editor = editor;
      this.editor.TextArea.TextView.VisualLinesChanged += this.VisualLinesChangedHandler;

      this.Cursor = Cursors.Hand;
      this.ToolTip = string.Empty;
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~TextMarkerTrackAdorner()
    {
      this.editor.TextArea.TextView.VisualLinesChanged -= this.VisualLinesChangedHandler;
    }

    #endregion
  }
}
