using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;

namespace IsblCheck.UI.Editor
{
  /// <summary>
  /// Текстовый маркер.
  /// </summary>
  public sealed class TextMarker : TextSegment
  {
    #region Поля и свойства

    /// <summary>
    /// Цвет заднего фона.
    /// </summary>
    public Color? Background { get; set; }

    /// <summary>
    /// Цвет текста.
    /// </summary>
    public Color? Foreground { get; set; }

    /// <summary>
    /// Ширина шрифта.
    /// </summary>
    public FontWeight? FontWeight { get; set; }

    /// <summary>
    /// Стиль шрифта.
    /// </summary>
    public FontStyle? FontStyle { get; set; }

    /// <summary>
    /// Цвет маркера.
    /// </summary>
    public Color MarkerColor { get; set; }

    /// <summary>
    /// Тип маркера.
    /// </summary>
    public TextMarkerType MarkerType { get; set; }

    /// <summary>
    /// Тег.
    /// </summary>
    public object Tag { get; set; }

    /// <summary>
    /// Подсказка.
    /// </summary>
    public object ToolTip { get; set; }

    #endregion

    #region Конструкторы

    public TextMarker(int startOffset, int length)
    {
      this.StartOffset = startOffset;
      this.Length = length;
      this.MarkerType = TextMarkerType.None;
    }

    #endregion
  }
}
