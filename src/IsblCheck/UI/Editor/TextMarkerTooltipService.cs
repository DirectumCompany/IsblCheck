using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System.Windows.Controls;
using System.Windows.Input;

namespace IsblCheck.UI.Editor
{
  /// <summary>
  /// Сервис подсказок.
  /// </summary>
  public class TextMarkerTooltipService
  {
    #region Поля и свойства

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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EditorToolTipOpeningHandler(object sender, ToolTipEventArgs e)
    {
      if (this.Markers == null)
        return;

      var point = Mouse.GetPosition(this.editor);
      var position = this.editor.GetPositionFromPoint(point);
      if (position == null && !position.HasValue)
        return;
      var offset = this.editor.Document.GetOffset(position.Value.Location);
      var marker = this.Markers.FindFirstSegmentWithStartAfter(offset);
      if (marker != null && marker.ToolTip != null)
        this.editor.TextArea.ToolTip = marker.ToolTip;
      else
        e.Handled = true;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="adornedElement">Декорируемый элемент.</param>
    /// <param name="editor">Редактор.</param>
    public TextMarkerTooltipService(TextEditor editor)
    {
      this.editor = editor;
      this.editor.TextArea.ToolTip = string.Empty;
      this.editor.TextArea.ToolTipOpening += this.EditorToolTipOpeningHandler;
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~TextMarkerTooltipService()
    {
    }

    #endregion
  }
}
