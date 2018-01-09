using IsblCheck.Common.Dialogs;
using IsblCheck.Common.Localization;
using IsblCheck.Common.Panels;
using IsblCheck.Services;
using IsblCheck.ViewModels.Dialogs;
using IsblCheck.ViewModels.Panels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace IsblCheck.ViewModels
{
  /// <summary>
  /// Класс обработчиков для команд открытия.
  /// </summary>
  public class OpenCommandHandlers
  {
    #region Константы

    /// <summary>
    /// Фильтр ISX файлов.
    /// </summary>
    protected const string IsxFileFilter = "ISX File(*.isx)|*.isx";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Сервис представлений.
    /// </summary>
    private IViewService viewService;

    #endregion

    #region Методы

    /// <summary>
    /// Открыть пакет разработки из диалогового окна.
    /// </summary>
    public void OpenPackage()
    {
      var settings = new OpenFileDialogSettings
      {
        Filter = IsxFileFilter,
        InitialDirectory = Environment.CurrentDirectory,
      };

      var result = this.viewService.ShowOpenFileDialog(PanelManager.Instance.PanelContainer, settings);
      if (!result.HasValue || !result.Value)
        return;

      var filename = settings.FileName;
      if (string.IsNullOrEmpty(filename))
        return;

      this.OpenSpecifiedPackage(filename);
    }

    /// <summary>
    /// Открыть папку с разработкой.
    /// </summary>
    public void OpenFolder()
    {
      using (var openFolderDialog = new CommonOpenFileDialog())
      {
        openFolderDialog.Title = LocalizationManager.Instance.LocalizeString("OPEN_FOLDER");
        openFolderDialog.IsFolderPicker = true;
        openFolderDialog.AddToMostRecentlyUsedList = false;
        openFolderDialog.AllowNonFileSystemItems = false;
        openFolderDialog.DefaultDirectory = Environment.CurrentDirectory;
        openFolderDialog.EnsureFileExists = true;
        openFolderDialog.EnsurePathExists = true;
        openFolderDialog.EnsureReadOnly = false;
        openFolderDialog.EnsureValidNames = true;
        openFolderDialog.Multiselect = false;
        openFolderDialog.ShowPlacesList = true;
        if (openFolderDialog.ShowDialog() != CommonFileDialogResult.Ok)
          return;
        var sourceExplorer = PanelManager.Instance.GetPanel<SourceExplorerViewModel>();
        if (sourceExplorer == null)
          sourceExplorer = PanelManager.Instance.CreatePanel<SourceExplorerViewModel>(true);
        sourceExplorer.Show();
        sourceExplorer.OpenFolderAsync(openFolderDialog.FileName);
      }
    }

    /// <summary>
    /// Открыть базу данных.
    /// </summary>
    public void OpenDatabase()
    {
      var openDatabaseDialog = DialogManager.Instance.CreateDialog<DatabaseCredentialViewModel>();

      var result = this.viewService.ShowDialog(PanelManager.Instance.PanelContainer, openDatabaseDialog);
      if (!result.HasValue || !result.Value)
        return;

      var connectionString = openDatabaseDialog.GetConnectionString();
      var sqlCredential = openDatabaseDialog.GetSqlCredential();

      var sourceExplorer = PanelManager.Instance.GetPanel<SourceExplorerViewModel>();
      if (sourceExplorer == null)
        sourceExplorer = PanelManager.Instance.CreatePanel<SourceExplorerViewModel>(true);

      sourceExplorer.Show();
      sourceExplorer.OpenDatabaseAsync(connectionString, sqlCredential);
    }

    /// <summary>
    /// Открыть пакет разработки.
    /// </summary>
    /// <param name="file">Файл.</param>
    public void OpenSpecifiedPackage(string filename)
    {
      var sourceExplorer = PanelManager.Instance.GetPanel<SourceExplorerViewModel>();
      if (sourceExplorer == null)
        sourceExplorer = PanelManager.Instance.CreatePanel<SourceExplorerViewModel>(true);

      sourceExplorer.Show();
      sourceExplorer.OpenPackageAsync(filename);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="viewService">Сервис представлений.</param>
    public OpenCommandHandlers(IViewService viewService)
    {
      this.viewService = viewService;
    }

    #endregion

  }
}
