using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace IsblCheck.ViewModels.Tree
{
  /// <summary>
  /// Узел контейнер.
  /// </summary>
  public class ContainerTreeNode : TreeNode
  {
    /// <summary>
    /// Дочерние элементы.
    /// </summary>
    public ICollection<TreeNode> Items { get; private set; }

    /// <summary>
    /// Установить фильтр.
    /// </summary>
    /// <param name="predicate">Предикат фильтрации.</param>
    public void SetFilter(Func<TreeNode, bool> predicate)
    {
      if (predicate == null)
        this.ItemsView.Filter = null;
      else
        this.ItemsView.Filter = (obj) => predicate((TreeNode)obj);
      this.ItemsView.Refresh();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ContainerTreeNode()
    {
      this.Items = new ObservableCollection<TreeNode>();
      this.ItemsView = CollectionViewSource.GetDefaultView(this.Items);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="items">Элементы.</param>
    public ContainerTreeNode(IEnumerable<TreeNode> items)
    {
      this.Items = new ObservableCollection<TreeNode>(items);
      this.ItemsView = CollectionViewSource.GetDefaultView(this.Items);
      this.ItemsView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
    }
  }
}
