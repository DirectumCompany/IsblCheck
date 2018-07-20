using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Dialogs;
using IsblCheck.Common.Localization;
using IsblCheck.Common.Panels;
using IsblCheck.Core.Reports;
using IsblCheck.Reports.Printers;
using IsblCheck.Services;

namespace IsblCheck.ViewModels.Panels
{
  /// <summary>
  /// Список ошибок.
  /// </summary>
  public class ReportViewerViewModel : PanelViewModelBase
  {
    #region Константы

    /// <summary>
    /// Заголовок панели.
    /// </summary>
    private const string PanelTitle = "REPORT_VIEWER";

    /// <summary>
    /// Фильтр CSV файлов.
    /// </summary>
    private const string CsvFileFilter = "CSV File(*.csv)|*.csv";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Отчет.
    /// </summary>
    public IReport Report { get; private set; }

    /// <summary>
    /// Признак показа ошибок.
    /// </summary>
    public bool IsErrorsShown
    {
      get { return this.isErrorsShown; }
      set
      {
        if (this.isErrorsShown == value)
          return;
        this.isErrorsShown = value;
        this.RaisePropertyChanged();

        this.ReportMessages?.Refresh();
      }
    }
    private bool isErrorsShown = true;

    /// <summary>
    /// Признак показа предупреждений.
    /// </summary>
    public bool IsWarningsShown
    {
      get { return this.isWarningsShown; }
      set
      {
        if (this.isWarningsShown == value)
          return;
        this.isWarningsShown = value;
        this.RaisePropertyChanged();
        this.ReportMessages?.Refresh();
      }
    }
    private bool isWarningsShown = true;

    /// <summary>
    /// Признак показа информации.
    /// </summary>
    public bool IsInformationsShown
    {
      get { return this.isInformationsShown; }
      set
      {
        if (this.isInformationsShown == value)
          return;
        this.isInformationsShown = value;
        this.RaisePropertyChanged();

        this.ReportMessages?.Refresh();
      }
    }
    private bool isInformationsShown = true;

    /// <summary>
    /// Количество ошибок.
    /// </summary>
    public int ErrorCount
    {
      get
      {
        return this.Report?.Messages.Count(m => m.Severity == Severity.Error) ?? 0;
      }
    }

    /// <summary>
    /// Количество предупреждений.
    /// </summary>
    public int WarningCount
    {
      get
      {
        return this.Report?.Messages.Count(m => m.Severity == Severity.Warning) ?? 0;
      }
    }

    /// <summary>
    /// Количество информации.
    /// </summary>
    public int InformationCount
    {
      get
      {
        return this.Report?.Messages.Count(m => m.Severity == Severity.Information) ?? 0;
      }
    }

    /// <summary>
    /// Представление коллекции сообщений.
    /// </summary>
    public ICollectionView ReportMessages
    {
      get { return this.reportMessages; }
      set
      {
        if (this.reportMessages == value)
          return;
        this.reportMessages = value;
        this.RaisePropertyChanged();
      }
    }
    private ICollectionView reportMessages;

    #endregion

    #region Команды

    /// <summary>
    /// Экспортировать как Csv.
    /// </summary>
    public ICommand ExportAsCsvCommand { get; private set; }

    /// <summary>
    /// Команда показа сообещния отчета.
    /// </summary>
    public ICommand ShowReportMessageCommand { get; private set; }

    #endregion

    #region Методы

    /// <summary>
    /// Показать отчет.
    /// </summary>
    /// <param name="report">Отчет.</param>
    public void ShowReport(IReport report)
    {
      this.Report = report;

      this.ReportMessages = CollectionViewSource.GetDefaultView(this.Report.Messages);
      this.ReportMessages.Filter = this.IsReportMessageShow;

      this.RaisePropertyChanged("ErrorCount");
      this.RaisePropertyChanged("WarningCount");
      this.RaisePropertyChanged("InformationCount");
    }

    /// <summary>
    /// Проверить возможность экспорта в Csv.
    /// </summary>
    /// <returns>true, если возможность есть, иначе false.</returns>
    private bool CanExportAsCsv()
    {
      return this.Report != null && this.Report.Messages.Any();
    }

    /// <summary>
    /// Экспортировать в Csv формат.
    /// </summary>
    private void ExportAsCsv()
    {
      var settings = new SaveFileDialogSettings
      {
        AddExtension = true,
        DefaultExt = ".csv",
        Filter = CsvFileFilter,
        InitialDirectory = Environment.CurrentDirectory,
        CheckFileExists = false,
        OverwritePrompt = true
      };
      
      var result = this.viewService.ShowSaveFileDialog(PanelManager.Instance.PanelContainer, settings);
      if (!result.HasValue || !result.Value)
        return;

      var filename = settings.FileName;
      if (string.IsNullOrEmpty(filename))
        return;

      var csvPrinter = new CsvReportPrinter(filename);
      csvPrinter.Print(this.Report);
      var message = LocalizationManager.Instance.LocalizeString("EXPORT_SUCCESS");
      this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Information);
    }

    /// <summary>
    /// Фильтр сообщений.
    /// </summary>
    /// <param name="item">Сообщение.</param>
    /// <returns>true, если сообщение нужно показать, иначе false.</returns>
    private bool IsReportMessageShow(object item)
    {
      var reportMessage = item as IReportMessage;
      var reportMessageShown = true;

      if (!this.IsErrorsShown && reportMessage.Severity == Severity.Error)
        reportMessageShown = false;

      if (!this.IsWarningsShown && reportMessage.Severity == Severity.Warning)
        reportMessageShown = false;

      if (!this.IsInformationsShown && reportMessage.Severity == Severity.Information)
        reportMessageShown = false;

      return reportMessageShown;
    }

    /// <summary>
    /// Показать сообщение отчета.
    /// </summary>
    /// <param name="message"></param>
    private void ShowReportMessage(IReportMessage message)
    {
      var documentViewer = PanelManager.Instance
        .GetPanels<DocumentViewerViewModel>()
        .FirstOrDefault(v => v.Document == message.Document);
      if (documentViewer == null)
      {
        documentViewer = PanelManager.Instance.CreatePanel<DocumentViewerViewModel>(false, message.Document);

        var messages = this.Report.Messages
          .Where(m => m.Document == message.Document);
        documentViewer.AddReportMessages(messages);
      }

      documentViewer.Show();
      documentViewer.CaretOffset = message.Position.StartIndex;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="viewService">Сервис представлений.</param>
    public ReportViewerViewModel(IViewService viewService) : base(viewService)
    {
      this.TitleLocalizationKey = PanelTitle;
      this.CanClose = false;

      this.ExportAsCsvCommand = new RelayCommand(this.ExportAsCsv, this.CanExportAsCsv);
      this.ShowReportMessageCommand = new RelayCommand<IReportMessage>(this.ShowReportMessage);
    }

    #endregion
  }
}
