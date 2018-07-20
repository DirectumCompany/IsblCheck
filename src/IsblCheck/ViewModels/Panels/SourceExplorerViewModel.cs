using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Localization;
using IsblCheck.Common.Panels;
using IsblCheck.Common.Settings;
using IsblCheck.Context.Development.Database;
using IsblCheck.Context.Development.Folder;
using IsblCheck.Context.Development.Package;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Development;
using IsblCheck.Services;
using IsblCheck.ViewModels.Tree;

namespace IsblCheck.ViewModels.Panels
{
  /// <summary>
  /// Презентер проводника исходного кода.
  /// </summary>
  public class SourceExplorerViewModel : PanelViewModelBase
  {
    #region Константы

    /// <summary>
    /// Заголовок панели.
    /// </summary>
    private const string PanelTitle = "SOURCE_EXPLORER";

    /// <summary>
    /// Имя настройки для последних открытых пакетов.
    /// </summary>
    private const string LastOpenedPackagesSettingsKey = "LastOpenedPackages";

    /// <summary>
    /// Имя настройки для последних открытых папок.
    /// </summary>
    private const string LastOpenedFoldersSettingsKey = "LastOpenedFolders";

    /// <summary>
    /// Максимальное число сохраняемых данных по последним открытым элементам.
    /// </summary>
    private const int MaxCountLastOpenedItems = 5;

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Сервис проверки кода.
    /// </summary>
    private readonly ICodeCheckerService codeCheckerService;

    /// <summary>
    /// Генератор узлов дерева.
    /// </summary>
    private readonly SourceTreeGenerator sourceTreeGenerator = new SourceTreeGenerator();

    /// <summary>
    /// Открытые исходные данные.
    /// </summary>
    private readonly ObservableCollection<ContainerTreeNode> componentTypes = new ObservableCollection<ContainerTreeNode>();

    /// <summary>
    /// Представление открытых исходных данных.
    /// </summary>
    public ICollectionView ComponentTypesView { get; private set; }

    /// <summary>
    /// Фильтр компонент.
    /// </summary>
    public string ComponentFilterText
    {
      get { return this.componentFilterText; }
      set
      {
        if (this.componentFilterText == value)
          return;
        this.componentFilterText = value;
        this.RaisePropertyChanged();
      }
    }
    private string componentFilterText;

    #endregion

    #region Команды

    /// <summary>
    /// Команда проверки всех элементов.
    /// </summary>
    public ICommand CheckAllCommand { get; private set; }

    /// <summary>
    /// Команда проверки документов.
    /// </summary>
    public ICommand CheckDocumentsCommand { get; private set; }

    /// <summary>
    /// Отфильтровать список компонент.
    /// </summary>
    public ICommand FiltrateComponentsCommand { get; private set; }

    /// <summary>
    /// Открыть документ.
    /// </summary>
    public ICommand OpenDocumentCommand { get; private set; }

    #endregion

    #region Методы

    /// <summary>
    /// Загрузить пакет.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    public async void OpenPackageAsync(string fileName)
    {
      try
      {
        var provider = new PackageProvider(fileName);
        await this.LoadDevelopmentAsync(provider, fileName);
      }
      catch
      {
        var message = LocalizationManager.Instance.LocalizeString("ERROR_PACKAGE_READ");
        this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Error);
      }
      SaveLastOpenedPackage(fileName);
    }

    /// <summary>
    /// Загрузить базу данных.
    /// </summary>
    /// <param name="connectionString">Строка подключения.</param>
    /// <param name="credential">Учетные данные.</param>
    public async void OpenDatabaseAsync(string connectionString, SqlCredential credential)
    {
      try
      {
        var provider = new DatabaseProvider(connectionString, credential);
        await this.LoadDevelopmentAsync(provider, connectionString);
      }
      catch
      {
        var message = LocalizationManager.Instance.LocalizeString("ERROR_DATABASE_READ");
        this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Error);
      }
    }

    public async void OpenFolderAsync(string workspacePath)
    {
      try
      {
        var provider = new FolderProvider(workspacePath);
        await this.LoadDevelopmentAsync(provider, workspacePath);
      }
      catch
      {
        var message = LocalizationManager.Instance.LocalizeString("ERROR_FOLDER_READ");
        this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Error);
      }
      SaveLastOpenedFolder(workspacePath);
    }

    private static void SaveLastOpenedPackage(string fileName)
    {
      var lastOpenedPackages = SettingsManager.Instance.GetValue<List<string>>(LastOpenedPackagesSettingsKey);
      if (lastOpenedPackages == null)
        lastOpenedPackages = new List<string>();

      var packageFileIndex = lastOpenedPackages.FindIndex(f => f.ToLower() == fileName.ToLower());
      if (packageFileIndex >= 0)
        lastOpenedPackages.RemoveAt(packageFileIndex);

      lastOpenedPackages.Insert(0, fileName);

      if (lastOpenedPackages.Count > MaxCountLastOpenedItems)
        lastOpenedPackages.RemoveRange(MaxCountLastOpenedItems, lastOpenedPackages.Count - MaxCountLastOpenedItems);

      SettingsManager.Instance.SetValue(LastOpenedPackagesSettingsKey, lastOpenedPackages);
    }

    private static void SaveLastOpenedFolder(string folderName)
    {
      var lastOpenedFolders = SettingsManager.Instance.GetValue<List<string>>(LastOpenedFoldersSettingsKey);
      if (lastOpenedFolders == null)
        lastOpenedFolders = new List<string>();

      var folderIndex = lastOpenedFolders.FindIndex(f => f.ToLower() == folderName.ToLower());
      if (folderIndex >= 0)
        lastOpenedFolders.RemoveAt(folderIndex);

      lastOpenedFolders.Insert(0, folderName);

      if (lastOpenedFolders.Count > MaxCountLastOpenedItems)
        lastOpenedFolders.RemoveRange(MaxCountLastOpenedItems, lastOpenedFolders.Count - MaxCountLastOpenedItems);

      SettingsManager.Instance.SetValue(LastOpenedFoldersSettingsKey, lastOpenedFolders);
    }

    private async Task LoadDevelopmentAsync(IDevelopmentContextProvider provider, string rootNodeTitle)
    {
      PanelManager.Instance.PanelContainer.BusyIndicatorCaption = 
        LocalizationManager.Instance.LocalizeString("BUSY_INDICATOR_LOAD_DEVELOPMENT_CAPTION");
      PanelManager.Instance.PanelContainer.IsBusyIndicatorVisible = true;
      try
      {
        await Task.Run(() =>
          {
            this.codeCheckerService.ClearProviders();
            this.codeCheckerService.AddProvider(provider);
          });

        this.componentTypes.Clear();
        // TODO: Слишком сложно добраться до контекста разработки. Подумать, как сделать удобнее.
        var componentTypeNodes = this.sourceTreeGenerator.Generate(rootNodeTitle, this.codeCheckerService.CodeChecker.ContextManager.Context.Development);
        foreach (var componentTypeNode in componentTypeNodes)
          this.componentTypes.Add(componentTypeNode);
      }
      finally
      {
        PanelManager.Instance.PanelContainer.IsBusyIndicatorVisible = false;
      }
    }

    /// <summary>
    /// Проверить возможность выполнения проверки всех элементов.
    /// </summary>
    /// <returns>true, если проверку можно выполнить.</returns>
    private bool CanCheckAll()
    {
      return this.componentTypes.Any();
    }

    /// <summary>
    /// Проверить возможность выполнения проверки документов.
    /// </summary>
    /// <param name="node">Узел.</param>
    /// <returns>true, если проверку можно выполнить.</returns>
    private static bool CanCheckDocuments(TreeNode node)
    {
      return node != null;
    }

    /// <summary>
    /// Проверить возможность открытия документа.
    /// </summary>
    /// <param name="document">Документ.</param>
    /// <returns>true, если открытие возможно.</returns>
    private static bool CanOpenDocument(IDocument document)
    {
      return document != null;
    }

    /// <summary>
    /// Проверить все документы.
    /// </summary>
    private async void CheckAll()
    {
      var documents = new List<IDocument>();
      foreach (var componentType in this.componentTypes)
        documents.AddRange(GetDocuments(componentType));

      PanelManager.Instance.PanelContainer.ProgressMaximum = documents.Count;
      var progress = new Progress<int>(value =>
      {
        Application.Current.Dispatcher.Invoke(() =>
        {
          PanelManager.Instance.PanelContainer.ProgressValue = value;
          if (value >= PanelManager.Instance.PanelContainer.ProgressMaximum)
            PanelManager.Instance.PanelContainer.IsProgressVisible = false;
        });
      });
      PanelManager.Instance.PanelContainer.IsProgressVisible = true;

      var report = await this.codeCheckerService.Check(documents, progress);

      Application.Current.Dispatcher.Invoke(() =>
      {
        var reportViewer = PanelManager.Instance.GetPanel<ReportViewerViewModel>();
        if (reportViewer == null)
          reportViewer = PanelManager.Instance.CreatePanel<ReportViewerViewModel>();

        reportViewer.Show();
        reportViewer.ShowReport(report);

        var documentViewers = PanelManager.Instance.GetPanels<DocumentViewerViewModel>();
        foreach (var documentViewer in documentViewers)
        {
          var messages = report.Messages
            .Where(m => m.Document == documentViewer.Document);
          documentViewer.ClearReportMessages();
          documentViewer.AddReportMessages(messages);
        }
      });
    }

    /// <summary>
    /// Проверить документы.
    /// </summary>
    /// <param name="node">Узел.</param>
    private async void CheckDocuments(TreeNode node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof(node));

      var documents = GetDocuments(node);

      PanelManager.Instance.PanelContainer.ProgressMaximum = documents.Count();
      var progress = new Progress<int>(value =>
      {
        Application.Current.Dispatcher.Invoke(() =>
        {
          PanelManager.Instance.PanelContainer.ProgressValue = value;
          if (value >= PanelManager.Instance.PanelContainer.ProgressMaximum)
            PanelManager.Instance.PanelContainer.IsProgressVisible = false;
        });
      });
      PanelManager.Instance.PanelContainer.IsProgressVisible = true;

      var report = await this.codeCheckerService.Check(documents, progress);

      Application.Current.Dispatcher.Invoke(() =>
      {
        var reportViewer = PanelManager.Instance.GetPanel<ReportViewerViewModel>();
        if (reportViewer == null)
          reportViewer = PanelManager.Instance.CreatePanel<ReportViewerViewModel>();

        reportViewer.Show();
        reportViewer.ShowReport(report);

        var documentViewers = PanelManager.Instance.GetPanels<DocumentViewerViewModel>();
        foreach (var documentViewer in documentViewers)
        {
          var messages = report.Messages
            .Where(m => m.Document == documentViewer.Document);
          documentViewer.ClearReportMessages();
          documentViewer.AddReportMessages(messages);
        }
      });
    }

    /// <summary>
    /// Получить все документы из узла.
    /// </summary>
    /// <param name="rootNode">Узел.</param>
    /// <returns>Документы.</returns>
    private static IEnumerable<IDocument> GetDocuments(TreeNode rootNode)
    {
      var result = new List<IDocument>();

      var queue = new Queue<TreeNode>();
      queue.Enqueue(rootNode);

      while (queue.Count != 0)
      {
        var node = queue.Dequeue();

        if (node is DocumentTreeNode)
        {
          var documentNode = node as DocumentTreeNode;
          result.Add(documentNode.Document);
        }
        else if (node is ContainerTreeNode)
        {
          var containerNode = node as ContainerTreeNode;
          foreach (var childNode in containerNode.Items)
            queue.Enqueue(childNode);
        }
      }

      return result;
    }

    /// <summary>
    /// Отфильтровать компонеты.
    /// </summary>
    private void FiltrateComponents()
    {
      var text = this.ComponentFilterText;
      foreach (var componentTypeNode in this.componentTypes)
      {
        if (!string.IsNullOrEmpty(this.ComponentFilterText))
        {
          componentTypeNode.SetFilter(node => node.Title.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0);
          var itemsExist = componentTypeNode.Items
            .Any(node => node.Title.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0);
          if (itemsExist)
            componentTypeNode.IsExpanded = true;
          else
            componentTypeNode.IsExpanded = false;
        }
        else
        {
          componentTypeNode.SetFilter(null);
          componentTypeNode.IsExpanded = false;
        }
      }
    }

    /// <summary>
    /// Открыть документ.
    /// </summary>
    /// <param name="document">Документ.</param>
    private void OpenDocument(IDocument document)
    {
      if (document == null)
        throw new ArgumentNullException(nameof(document));

      var documentViewer = PanelManager.Instance
        .GetPanels<DocumentViewerViewModel>()
        .FirstOrDefault(p => p.Document == document);
      if (documentViewer == null)
        documentViewer = PanelManager.Instance.CreatePanel<DocumentViewerViewModel>(false, document);

      documentViewer.Show();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="codeCheckerService">Сервис проверки кода.</param>
    /// <param name="viewService">Сервис представлений.</param>
    public SourceExplorerViewModel(ICodeCheckerService codeCheckerService, IViewService viewService) : base(viewService)
    {
      this.codeCheckerService = codeCheckerService;

      this.TitleLocalizationKey = PanelTitle;
      this.CanClose = false;

      this.ComponentTypesView = CollectionViewSource.GetDefaultView(this.componentTypes);

      this.FiltrateComponentsCommand = new RelayCommand(this.FiltrateComponents);
      this.OpenDocumentCommand = new RelayCommand<IDocument>(this.OpenDocument, CanOpenDocument);
      this.CheckAllCommand = new RelayCommand(this.CheckAll, this.CanCheckAll);
      this.CheckDocumentsCommand = new RelayCommand<TreeNode>(this.CheckDocuments, CanCheckDocuments);
    }

    #endregion
  }
}
