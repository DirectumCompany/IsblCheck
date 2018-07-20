using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Settings;
using IsblCheck.Services;

namespace IsblCheck.ViewModels.Panels
{
  // TODO: Добавить последние открытые папки.

  /// <summary>
  /// Презентер стартовой страницы.
  /// </summary>
  public class StartPageViewModel : PanelViewModelBase
  {
    #region Константы

    /// <summary>
    /// Заголовок панели.
    /// </summary>
    private const string PanelTitle = "START_PAGE";

    /// <summary>
    /// Имя настройки для последних открытых пакетов.
    /// </summary>
    private const string LastOpenedPackagesSettingsKey = "LastOpenedPackages";

    #endregion

    #region Поля и свойства
    
    /// <summary>
    /// Последние открытые пакеты.
    /// </summary>
    private readonly ObservableCollection<string> lastOpenedPackages = new ObservableCollection<string>();

    /// <summary>
    /// Представление последних открытых исходных данных.
    /// </summary>
    public ICollectionView LastOpenedPackagesView { get; private set; }

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
    /// Команда открытия файла пакета.
    /// </summary>
    public ICommand OpenSpecifiedPackageCommand { get; private set; }

    #endregion

    #region Методы

    /// <summary>
    /// Сгенерировать событие закрытия презентера.
    /// </summary>
    protected override void OnClosed()
    {
      // Отписываемся от событий.
      SettingsManager.Instance.SettingChanged -= this.SettingChangedHandler;

      base.OnClosing();
    }

    /// <summary>
    /// Загрузить последние открытые пакеты.
    /// </summary>
    private void LoadLastOpenedPackages()
    {
      var settingValue = SettingsManager.Instance.GetValue<List<string>>(LastOpenedPackagesSettingsKey);
      if (settingValue == null)
        return;

      this.lastOpenedPackages.Clear();
      foreach (var value in settingValue)
        this.lastOpenedPackages.Add(value);
    }

    /// <summary>
    /// Обработчик события изменения настройки.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void SettingChangedHandler(object sender, string e)
    {
      if (e != LastOpenedPackagesSettingsKey)
        return;

      this.LoadLastOpenedPackages();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="viewService">Сервис представлений.</param>
    public StartPageViewModel(IViewService viewService) : base(viewService)
    {
      this.TitleLocalizationKey = PanelTitle;
      this.CanClose = true;
      
      this.LastOpenedPackagesView = CollectionViewSource.GetDefaultView(this.lastOpenedPackages);

      this.LoadLastOpenedPackages();

      SettingsManager.Instance.SettingChanged += this.SettingChangedHandler;

      this.OpenSpecifiedPackageCommand = new RelayCommand<string>(this.openCommandHandlers.OpenSpecifiedPackage);
      this.OpenPackageCommand = new RelayCommand(this.openCommandHandlers.OpenPackage);
      this.OpenDatabaseCommand = new RelayCommand(this.openCommandHandlers.OpenDatabase);
      this.OpenFolderCommand = new RelayCommand(this.openCommandHandlers.OpenFolder);
    }

    #endregion
  }
}
