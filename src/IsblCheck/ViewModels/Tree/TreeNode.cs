using System.ComponentModel;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace IsblCheck.ViewModels.Tree
{
  /// <summary>
  /// Базовый элемент дерева.
  /// </summary>
  public abstract class TreeNode : ViewModelBase
  {
    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title
    {
      get { return this.title; }
      set
      {
        if (this.title == value)
          return;
        this.title = value;
        this.RaisePropertyChanged();
      }
    }
    private string title;

    /// <summary>
    /// Строка локализации для заголовка.
    /// </summary>
    public string TitleLocalizationKey
    {
      get { return this.titleLocalizationKey; }
      set
      {
        if (this.titleLocalizationKey == value)
          return;
        this.titleLocalizationKey = value;
        this.RaisePropertyChanged();
        this.RaisePropertyChanged("HasTitleLocalization");
      }
    }
    private string titleLocalizationKey;

    /// <summary>
    /// Есть строка локализации для заголовка.
    /// </summary>
    public bool HasTitleLocalization => !string.IsNullOrEmpty(this.TitleLocalizationKey);

    /// <summary>
    /// Иконка.
    /// </summary>
    public ImageSource IconSource
    {
      get { return this.iconSource; }
      set
      {
        if (this.iconSource == value)
          return;
        this.iconSource = value;
        this.RaisePropertyChanged();
        this.RaisePropertyChanged("HasIconSource");
      }
    }
    private ImageSource iconSource;

    /// <summary>
    /// Есть иконка.
    /// </summary>
    public bool HasIconSource => this.IconSource != null;

    /// <summary>
    /// Признак выделения.
    /// </summary>
    public bool IsSelected
    {
      get { return this.isSelected; }
      set
      {
        if (this.isSelected == value)
          return;
        this.isSelected = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isSelected;

    /// <summary>
    /// Признак развернутости.
    /// </summary>
    public bool IsExpanded
    {
      get { return this.isExpanded; }
      set
      {
        if (this.isExpanded == value)
          return;
        this.isExpanded = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isExpanded;

    /// <summary>
    /// Признак показа дочерних элементов.
    /// </summary>
    public bool IsItemsCountVisible
    {
      get { return this.isItemsCountVisible; }
      set
      {
        if (this.isItemsCountVisible == value)
          return;
        this.isItemsCountVisible = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isItemsCountVisible;

    /// <summary>
    /// Представление дочерних элементов.
    /// </summary>
    public ICollectionView ItemsView { get; protected set; }
  }
}
