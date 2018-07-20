using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Dialogs;
using IsblCheck.Common.Panels;
using IsblCheck.Services;
using IsblCheck.ViewModels.Dialogs;
using IsblCheck.ViewModels.Panels;
using IsblCheck.ViewModels.Toolbars;

namespace IsblCheck.ViewModels.Containers
{
  /// <summary>
  /// Презентер главного окна.
  /// </summary>
  public class PanelContainerViewModel : ViewModelBase, IPanelContainer
  {
    #region Поля и свойства
    
    /// <summary>
    /// Сервис представлений.
    /// </summary>
    private readonly IViewService viewService;

    /// <summary>
    /// Команды и обработчики открытия базы/пакета/папки.
    /// </summary>
    private readonly OpenCommandHandlers openCommandHandlers;

    /// <summary>
    /// Панели.
    /// </summary>
    public ObservableCollection<IPanel> Panels { get; }

    /// <summary>
    /// Закрепляемые панели.
    /// </summary>
    public ObservableCollection<IPanel> AnchorablePanels { get; }

    /// <summary>
    /// Представление панелей.
    /// </summary>
    public ICollectionView PanelsView { get; private set; }

    /// <summary>
    /// Представление закрепляемых панелей.
    /// </summary>
    public ICollectionView AnchorablePanelsView { get; private set; }

    /// <summary>
    /// Стандартный тулбар.
    /// </summary>
    public StandardToolbarViewModel StandardToolbar { get; private set; }

    /// <summary>
    /// Видимость прогресса.
    /// </summary>
    public bool IsProgressVisible
    {
      get { return this.isProgressVisible; }
      set
      {
        if (this.isProgressVisible == value)
          return;
        this.isProgressVisible = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isProgressVisible;

    /// <summary>
    /// Видимость индикатора загрузки.
    /// </summary>
    public bool IsBusyIndicatorVisible
    {
      get { return this.isBusyIndicatorVisible; }
      set
      {
        if (this.isBusyIndicatorVisible == value)
          return;
        this.isBusyIndicatorVisible = value;
        this.RaisePropertyChanged();
      }
    }
    private bool isBusyIndicatorVisible;

    /// <summary>
    /// Заголовок индикатора прогресса.
    /// </summary>
    public string BusyIndicatorCaption
    {
      get { return this.busyIndicatorCaption; }
      set
      {
        if (this.busyIndicatorCaption == value)
          return;
        this.busyIndicatorCaption = value;
        this.RaisePropertyChanged();
      }
    }
    private string busyIndicatorCaption;

    /// <summary>
    /// Значение прогресса.
    /// </summary>
    public int ProgressValue
    {
      get { return this.progressValue; }
      set
      {
        if (this.progressValue == value)
          return;
        this.progressValue = value;
        this.RaisePropertyChanged();
      }
    }
    private int progressValue;

    /// <summary>
    /// Максимальное значение прогресса.
    /// </summary>
    public int ProgressMaximum
    {
      get { return this.progressMaximum; }
      set
      {
        if (this.progressMaximum == value)
          return;
        this.progressMaximum = value;
        this.RaisePropertyChanged();
      }
    }
    private int progressMaximum;

    #endregion

    #region Команды

    /// <summary>
    /// Команда закрытия приложения.
    /// </summary>
    public ICommand ExitCommand { get; private set; }

    /// <summary>
    /// Команда показа проводника исходных данных.
    /// </summary>
    public ICommand ShowSourceExplorerCommand { get; private set; }

    /// <summary>
    /// Команда показа панели списка ошибок.
    /// </summary>
    public ICommand ShowReportViewerCommand { get; private set; }

    /// <summary>
    /// Команда показа стартовой страницы.
    /// </summary>
    public ICommand ShowStartPageCommand { get; private set; }

    /// <summary>
    /// Команда показа окна о программе.
    /// </summary>
    public ICommand ShowAboutCommand { get; private set; }

    /// <summary>
    /// Команда открытия пакета.
    /// </summary>
    public ICommand OpenPackageCommand { get; private set; }

    /// <summary>
    /// Команда открытия базы данных.
    /// </summary>
    public ICommand OpenDatabaseCommand { get; private set; }

    /// <summary>
    /// Команда открытия из папки.
    /// </summary>
    public ICommand OpenFolderCommand { get; private set; }

    #endregion

    #region Методы

    /// <summary>
    /// Уничтожить объект.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
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
    /// Выйти из приложения.
    /// </summary>
    private static void Exit()
    {
      Application.Current.Shutdown();
    }

    /// <summary>
    /// Показать проводник исходных данных.
    /// </summary>
    private static void ShowSourceExplorer()
    {
      var sourceExplorer = PanelManager.Instance.GetPanel<SourceExplorerViewModel>();
      if (sourceExplorer == null)
        sourceExplorer = PanelManager.Instance.CreatePanel<SourceExplorerViewModel>(true);

      sourceExplorer.Show();
    }

    /// <summary>
    /// Показать список ошибок.
    /// </summary>
    private static void ShowReportViewer()
    {
      var reportViewer = PanelManager.Instance.GetPanel<ReportViewerViewModel>();
      if (reportViewer == null)
        reportViewer = PanelManager.Instance.CreatePanel<ReportViewerViewModel>(true);

      reportViewer.Show();
    }

    /// <summary>
    /// Показать стартовую страницу.
    /// </summary>
    private static void ShowStartPage()
    {
      var startPage = PanelManager.Instance.GetPanel<StartPageViewModel>();
      if (startPage == null)
        startPage = PanelManager.Instance.CreatePanel<StartPageViewModel>();

      startPage.Show();
    }

    /// <summary>
    /// Показать окно о программе.
    /// </summary>
    private void ShowAbout()
    {
      var aboutDialog = DialogManager.Instance.CreateDialog<AboutViewModel>();
      this.viewService.ShowDialog(PanelManager.Instance.PanelContainer, aboutDialog);
    }

    /// <summary>
    /// Событие изменения коллекции панелей.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void PanelsCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        var panels = e.NewItems.OfType<IPanel>();
        foreach (var panel in panels)
          panel.Closing += this.PanelClosingHandler;
      }

      if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        var panels = e.OldItems.OfType<IPanel>();
        foreach (var panel in panels)
          panel.Closing -= this.PanelClosingHandler;
      }
    }

    /// <summary>
    /// Событие закрытия панели.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void PanelClosingHandler(object sender, EventArgs e)
    {
      var panel = sender as IPanel;
      if (panel == null)
        return;

      if (this.Panels.Contains(panel))
        this.Panels.Remove(panel);

      if (this.AnchorablePanels.Contains(panel))
        this.AnchorablePanels.Remove(panel);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="codeCheckerService">Сервис проверки кода.</param>
    /// <param name="viewService">Сервис представлений.</param>
    public PanelContainerViewModel(ICodeCheckerService codeCheckerService, IViewService viewService)
    {
      this.viewService = viewService;
      this.openCommandHandlers = new OpenCommandHandlers(viewService);
      this.OpenPackageCommand = new RelayCommand(this.openCommandHandlers.OpenPackage);
      this.OpenDatabaseCommand = new RelayCommand(this.openCommandHandlers.OpenDatabase);
      this.OpenFolderCommand = new RelayCommand(this.openCommandHandlers.OpenFolder);

      this.Panels = new ObservableCollection<IPanel>();
      this.Panels.CollectionChanged += this.PanelsCollectionChangedHandler;
      this.PanelsView = CollectionViewSource.GetDefaultView(this.Panels);

      this.AnchorablePanels = new ObservableCollection<IPanel>();
      this.AnchorablePanels.CollectionChanged += this.PanelsCollectionChangedHandler;
      this.AnchorablePanelsView = CollectionViewSource.GetDefaultView(this.AnchorablePanels);

      // TODO: Панельки должны добавляться в лист.
      this.StandardToolbar = new StandardToolbarViewModel(codeCheckerService, viewService);

      this.ExitCommand = new RelayCommand(Exit);
      this.ShowSourceExplorerCommand = new RelayCommand(ShowSourceExplorer);
      this.ShowReportViewerCommand = new RelayCommand(ShowReportViewer);
      this.ShowStartPageCommand = new RelayCommand(ShowStartPage);
      this.ShowAboutCommand = new RelayCommand(this.ShowAbout);
    }

    #endregion
  }
}