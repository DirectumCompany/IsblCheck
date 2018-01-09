using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Panels;
using IsblCheck.Services;
using IsblCheck.ViewModels.Panels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IsblCheck.ViewModels.Toolbars
{
  /// <summary>
  /// Презентер стандартной панели.
  /// </summary>
  public class StandardToolbarViewModel : ViewModelBase
  {
    #region Поля и свойства

    /// <summary>
    /// Команды и обработчики открытия базы/пакета/папки.
    /// </summary>
    private readonly OpenCommandHandlers openCommandHandlers;

    /// <summary>
    /// Сервис проверки кода.
    /// </summary>
    private readonly ICodeCheckerService codeCheckerService;

    #endregion

    #region Команды

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

    /// <summary>
    /// Команда проверки активного документа.
    /// </summary>
    public ICommand CheckCommand { get; private set; }

    /// <summary>
    /// Команда проверки всех документов.
    /// </summary>
    public ICommand CheckAllCommand { get; private set; }

    #endregion

    #region Методы

    /// <summary>
    /// Проверить активный документ.
    /// </summary>
    private async void Check()
    {
      var activeDocumentViewer = PanelManager.Instance
        .GetPanels<DocumentViewerViewModel>()
        .FirstOrDefault(dv => dv.IsActive);

      PanelManager.Instance.PanelContainer.ProgressMaximum = 1;
      var progress = new Progress<int>((value) =>
      {
        Application.Current.Dispatcher.Invoke(() =>
        {
          PanelManager.Instance.PanelContainer.ProgressValue = value;
          if (value >= PanelManager.Instance.PanelContainer.ProgressMaximum)
            PanelManager.Instance.PanelContainer.IsProgressVisible = false;
        });
      });
      PanelManager.Instance.PanelContainer.IsProgressVisible = true;

      var report = await this.codeCheckerService.Check(activeDocumentViewer.Document, progress);

      var reportViewer = PanelManager.Instance.GetPanel<ReportViewerViewModel>();
      if (reportViewer == null)
        reportViewer = PanelManager.Instance.CreatePanel<ReportViewerViewModel>(true);

      reportViewer.Show();
      reportViewer.ShowReport(report);

      activeDocumentViewer.ClearReportMessages();
      activeDocumentViewer.AddReportMessages(report.Messages);
    }

    /// <summary>
    /// Проверить возможность проверить активный документ.
    /// </summary>
    private bool CanCheck()
    {
      return PanelManager.Instance
        .GetPanels<DocumentViewerViewModel>()
        .Any(dv => dv.IsActive);
    }

    /// <summary>
    /// Проверить все активные документы.
    /// </summary>
    private async void CheckAll()
    {
      var openedDocumentViewers = PanelManager.Instance
        .GetPanels<DocumentViewerViewModel>();
      var openedDocuments = openedDocumentViewers
        .Select(p => p.Document)
        .ToList();

      PanelManager.Instance.PanelContainer.ProgressMaximum = openedDocuments.Count;
      var progress = new Progress<int>((value) =>
      {
        Application.Current.Dispatcher.Invoke(() =>
        {
          PanelManager.Instance.PanelContainer.ProgressValue = value;
          if (value >= PanelManager.Instance.PanelContainer.ProgressMaximum)
            PanelManager.Instance.PanelContainer.IsProgressVisible = false;
        });
      });
      PanelManager.Instance.PanelContainer.IsProgressVisible = true;

      var report = await this.codeCheckerService.Check(openedDocuments, progress);

      var reportViewer = PanelManager.Instance.GetPanel<ReportViewerViewModel>();
      if (reportViewer == null)
        reportViewer = PanelManager.Instance.CreatePanel<ReportViewerViewModel>(true);

      reportViewer.Show();
      reportViewer.ShowReport(report);

      foreach (var openedDocumentViewer in openedDocumentViewers)
      {
        var messages = report.Messages
          .Where(m => m.Document == openedDocumentViewer.Document);
        openedDocumentViewer.ClearReportMessages();
        openedDocumentViewer.AddReportMessages(messages);
      }
    }

    /// <summary>
    /// Проверить возможность проверить все активные документы.
    /// </summary>
    private bool CanCheckAll()
    {
      return PanelManager.Instance
        .GetPanels<DocumentViewerViewModel>()
        .Any();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="codeCheckerService">Сервис проверки кода.</param>
    /// <param name="viewService">Сервис представлений.</param>
    public StandardToolbarViewModel(ICodeCheckerService codeCheckerService, IViewService viewService)
    {
      this.codeCheckerService = codeCheckerService;
      this.openCommandHandlers = new OpenCommandHandlers(viewService);
      this.CheckCommand = new RelayCommand(this.Check, this.CanCheck);
      this.CheckAllCommand = new RelayCommand(this.CheckAll, this.CanCheckAll);
      this.OpenPackageCommand = new RelayCommand(this.openCommandHandlers.OpenPackage);
      this.OpenDatabaseCommand = new RelayCommand(this.openCommandHandlers.OpenDatabase);
      this.OpenFolderCommand = new RelayCommand(this.openCommandHandlers.OpenFolder);
    }

    #endregion
  }
}
