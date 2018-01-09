using ICSharpCode.AvalonEdit.Search;

namespace IsblCheck.Views.Panels
{
  /// <summary>
  /// Представление документа.
  /// </summary>
  public partial class DocumentViewerView
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    public DocumentViewerView()
    {
      this.InitializeComponent();
      SearchPanel.Install(this.TextEditor);
    }
  }
}
