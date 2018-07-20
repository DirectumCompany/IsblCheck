using System;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Panels;
using IsblCheck.Services;

namespace IsblCheck.ViewModels.Panels
{
  /// <summary>
  /// Презентер панели.
  /// </summary>
  public abstract class PanelViewModelBase : ViewModelBase, IPanel
  {
    #region Поля и свойства
    
    /// <summary>
    /// Сервис представлений.
    /// </summary>
    protected IViewService viewService;

    /// <summary>
    /// Команды и обработчики открытия базы/пакета/папки.
    /// </summary>
    protected readonly OpenCommandHandlers openCommandHandlers;

    /// <summary>
    /// Идентификатор контента.
    /// </summary>
    public string ContentId
    {
      get { return this.contentId; }
      set
      {
        if (contentId == value)
          return;
        this.contentId = value;
        this.RaisePropertyChanged();
      }
    }
    private string contentId;

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title
    {
      get { return this.title; }
      protected set
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
      protected set
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
    /// Признак активности.
    /// </summary>
    public bool IsActive
    {
      get { return this.isActive; }
      set
      {
        if (this.isActive == value)
          return;
        this.isActive = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isActive;

    /// <summary>
    /// Признак видимости.
    /// </summary>
    public bool IsVisible
    {
      get { return this.isVisible; }
      set
      {
        if (this.isVisible == value)
          return;
        this.isVisible = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isVisible = true;

    /// <summary>
    /// Признак возможности закрытия вкладки.
    /// </summary>
    public bool CanClose
    {
      get { return this.canClose; }
      protected set
      {
        if (this.canClose == value)
          return;
        this.canClose = value;
        this.RaisePropertyChanged();
      }
    }
    private bool canClose = true;

    /// <summary>
    /// Иконка.
    /// </summary>
    public ImageSource IconSource
    {
      get { return this.iconSource; }
      protected set
      {
        if (this.iconSource == value)
          return;
        this.iconSource = value;
        this.RaisePropertyChanged();
      }
    }
    private ImageSource iconSource;

    #endregion

    #region События

    /// <summary>
    /// Событие до закрытия презентера.
    /// </summary>
    public event EventHandler Closing;

    /// <summary>
    /// Сгенерировать событие закрытия презентера.
    /// </summary>
    protected virtual void OnClosing()
    {
      var handler = this.Closing;
      handler?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Событие закрытия презентера.
    /// </summary>
    public event EventHandler Closed;

    /// <summary>
    /// Сгенерировать событие закрытия презентера.
    /// </summary>
    protected virtual void OnClosed()
    {
      var handler = this.Closed;
      handler?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Команды

    /// <summary>
    /// Команда закрытия.
    /// </summary>
    public ICommand CloseCommand { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Выполнить уничтожение.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Показать панель.
    /// </summary>
    public virtual void Show()
    {
      this.IsVisible = true;
      this.IsSelected = true;
      this.IsActive = true;
    }

    /// <summary>
    /// Выполнить уничтожение.
    /// </summary>
    /// <param name="disposing">Уничтожить управляемые ресурсы.</param>
    protected virtual void Dispose(bool disposing)
    {
      // TODO: нужно безопасно удалить иконку.
    }

    /// <summary>
    /// Закрыть вкладку.
    /// </summary>
    private void Close()
    {
      if (!this.CanClose)
        return;

      this.OnClosing();
      this.Dispose();
      this.OnClosed();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected PanelViewModelBase(IViewService viewService)
    {
      this.CloseCommand = new RelayCommand(this.Close);
      this.viewService = viewService;
      this.openCommandHandlers = new OpenCommandHandlers(viewService);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected PanelViewModelBase()
    {
      this.CloseCommand = new RelayCommand(this.Close);
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~PanelViewModelBase()
    {
      this.Dispose(false);
    }

    #endregion
  }
}
