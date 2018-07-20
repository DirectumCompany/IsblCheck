using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

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

      var lineStart = line.Offset;
      var lineEnd = lineStart + line.Length;

      var lineMarkers = this.Markers.FindOverlappingSegments(lineStart, line.Length);
      foreach (var marker in lineMarkers)
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
    public KnownLayer Layer => KnownLayer.Selection;

    /// <summary>
    /// Нарисовать.
    /// </summary>
    /// <param name="textView">Текстовое представление.</param>
    /// <param name="drawingContext">Контекст отрисовки.</param>
    public void Draw(TextView textView, DrawingContext drawingContext)
    {
      if (textView == null)
        throw new ArgumentNullException(nameof(textView));

      if (drawingContext == null)
        throw new ArgumentNullException(nameof(drawingContext));

      if (this.Markers == null || !textView.VisualLinesValid)
        return;

      var visualLines = textView.VisualLines;
      if (visualLines.Count == 0)
        return;

      var viewStart = visualLines.First().FirstDocumentLine.Offset;
      var viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
      var viewMarkers = this.Markers.FindOverlappingSegments(viewStart, viewEnd - viewStart);

      foreach (var marker in viewMarkers)
      {
        // Отрисовка фона.
        if (marker.Background != null)
        {
          var geometryBuilder = new BackgroundGeometryBuilder
          {
            AlignToWholePixels = true,
            CornerRadius = 3
          };
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
            var startPoint = segmentRect.BottomLeft;
            var endPoint = segmentRect.BottomRight;

            Brush usedBrush = new SolidColorBrush(marker.MarkerColor);
            usedBrush.Freeze();

            // Подчеркивание волнистой линией.
            if ((marker.MarkerType & TextMarkerType.SquigglyUnderline) != 0)
            {
              var usedPen = new Pen(usedBrush, 1);
              usedPen.Freeze();

              var geometry = CreateSquigglyLine(startPoint, endPoint);
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
              var usedPen = new Pen(usedBrush, 1) { DashStyle = DashStyles.Dot };
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
    private static Geometry CreateSquigglyLine(Point start, Point end)
    {
      var offset = 2.5;
      var count = Math.Max((int)((end.X - start.X) / offset) + 1, 4);

      var geometry = new StreamGeometry();
      using (var ctx = geometry.Open())
      {
        ctx.BeginFigure(start, false, false);
        var points = new List<Point>();
        for (var i = 0; i < count; i++)
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
