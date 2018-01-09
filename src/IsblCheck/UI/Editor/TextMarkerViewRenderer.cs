using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace IsblCheck.UI.Editor
{
  /// <summary>
  /// Рендерер маркеров в текстовом представлении.
  /// </summary>
  public class TextMarkerViewRenderer : DocumentColorizingTransformer, IBackgroundRenderer
  {
    #region DocumentColorizingTransformer

    /// <summary>
    /// Выполнить подсветку линии.
    /// </summary>
    /// <param name="line">Линия.</param>
    protected override void ColorizeLine(DocumentLine line)
    {
      if (this.Markers == null)
        return;

      int lineStart = line.Offset;
      int lineEnd = lineStart + line.Length;

      var lineMarkers = this.Markers.FindOverlappingSegments(lineStart, line.Length);
      foreach (TextMarker marker in lineMarkers)
      {
        var startOffset = Math.Max(marker.StartOffset, lineStart);
        var endOffset = Math.Min(marker.EndOffset, lineEnd);

        this.ChangeLinePart(startOffset, endOffset, element =>
        {
          if (marker.Foreground != null)
          {
            var usedBrush = new SolidColorBrush(marker.Foreground.Value);
            usedBrush.Freeze();

            element.TextRunProperties.SetForegroundBrush(usedBrush);
          }

          var oldTypeFace = element.TextRunProperties.Typeface;
          var newTypeFace = new Typeface(oldTypeFace.FontFamily,
            marker.FontStyle ?? oldTypeFace.Style,
            marker.FontWeight ?? oldTypeFace.Weight,
            oldTypeFace.Stretch);

          element.TextRunProperties.SetTypeface(newTypeFace);
        });
      }
    }

    #endregion

    #region IBackgroundRenderer

    /// <summary>
    /// Слой, на котором будет выполняться отрисовка.
    /// </summary>
    public KnownLayer Layer
    {
      get
      {
        return KnownLayer.Selection;
      }
    }

    /// <summary>
    /// Нарисовать.
    /// </summary>
    /// <param name="textView">Текстовое представление.</param>
    /// <param name="drawingContext">Контекст отрисовки.</param>
    public void Draw(TextView textView, DrawingContext drawingContext)
    {
      if (textView == null)
        throw new ArgumentNullException("textView");

      if (drawingContext == null)
        throw new ArgumentNullException("drawingContext");

      if (this.Markers == null || !textView.VisualLinesValid)
        return;

      var visualLines = textView.VisualLines;
      if (visualLines.Count == 0)
        return;

      int viewStart = visualLines.First().FirstDocumentLine.Offset;
      int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
      var viewMarkers = this.Markers.FindOverlappingSegments(viewStart, viewEnd - viewStart);

      foreach (TextMarker marker in viewMarkers)
      {
        // Отрисовка фона.
        if (marker.Background != null)
        {
          var geometryBuilder = new BackgroundGeometryBuilder();
          geometryBuilder.AlignToWholePixels = true;
          geometryBuilder.CornerRadius = 3;
          geometryBuilder.AddSegment(textView, marker);

          var geometry = geometryBuilder.CreateGeometry();
          if (geometry != null)
          {
            var brush = new SolidColorBrush(marker.Background.Value);
            brush.Freeze();
            drawingContext.DrawGeometry(brush, null, geometry);
          }
        }

        // Отрисовка маркера.
        var underlineMarkerTypes = TextMarkerType.SquigglyUnderline | TextMarkerType.NormalUnderline | TextMarkerType.DottedUnderline;
        if ((marker.MarkerType & underlineMarkerTypes) != 0)
        {
          var segmentRects = BackgroundGeometryBuilder.GetRectsForSegment(textView, marker);
          foreach (var segmentRect in segmentRects)
          {
            Point startPoint = segmentRect.BottomLeft;
            Point endPoint = segmentRect.BottomRight;

            Brush usedBrush = new SolidColorBrush(marker.MarkerColor);
            usedBrush.Freeze();

            // Подчеркивание волнистой линией.
            if ((marker.MarkerType & TextMarkerType.SquigglyUnderline) != 0)
            {
              var usedPen = new Pen(usedBrush, 1);
              usedPen.Freeze();

              var geometry = this.CreateSquigglyLine(startPoint, endPoint);
              drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
            }

            // Подчеркивание сплошной линией.
            if ((marker.MarkerType & TextMarkerType.NormalUnderline) != 0)
            {
              var usedPen = new Pen(usedBrush, 1);
              usedPen.Freeze();

              drawingContext.DrawLine(usedPen, startPoint, endPoint);
            }

            // Подчеркивание точками.
            if ((marker.MarkerType & TextMarkerType.DottedUnderline) != 0)
            {
              var usedPen = new Pen(usedBrush, 1);
              usedPen.DashStyle = DashStyles.Dot;
              usedPen.Freeze();

              drawingContext.DrawLine(usedPen, startPoint, endPoint);
            }
          }
        }
      }
    }

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Маркеры.
    /// </summary>
    public TextSegmentCollection<TextMarker> Markers { get; set; }

    #endregion

    #region Методы

    /// <summary>
    /// Создать геометрию волнистой линии.
    /// </summary>
    /// <param name="start">Начальная позиция.</param>
    /// <param name="end">Конечная позиция.</param>
    /// <returns>Геометрия волнистой линии.</returns>
    private Geometry CreateSquigglyLine(Point start, Point end)
    {
      var offset = 2.5;
      int count = Math.Max((int)((end.X - start.X) / offset) + 1, 4);

      StreamGeometry geometry = new StreamGeometry();
      using (StreamGeometryContext ctx = geometry.Open())
      {
        ctx.BeginFigure(start, false, false);
        var points = new List<Point>();
        for (int i = 0; i < count; i++)
        {
          var point = new Point(start.X + i * offset, start.Y - ((i + 1) % 2 == 0 ? offset : 0));
          points.Add(point);
        }
        ctx.PolyLineTo(points, true, false);
      }
      geometry.Freeze();
      return geometry;
    }

    #endregion
  }
}
