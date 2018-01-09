using IsblCheck.Core.Checker;

namespace IsblCheck.ViewModels.Tree
{
  /// <summary>
  /// Узел документа.
  /// </summary>
  public class DocumentTreeNode : TreeNode
  {
    /// <summary>
    /// Документ.
    /// </summary>
    public IDocument Document { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="document">Документ.</param>
    /// <param name="openDocument">Действие открытия документа.</param>
    public DocumentTreeNode(IDocument document)
    {
      this.Document = document;
    }
  }
}
