using Common.Logging;
using IsblCheck.Common.Dialogs;
using IsblCheck.Common.Localization;
using IsblCheck.Common.Panels;
using IsblCheck.Common.Settings;
using IsblCheck.Common.Windows;
using IsblCheck.Services;
using IsblCheck.ViewModels.Containers;
using IsblCheck.ViewModels.Dialogs;
using IsblCheck.ViewModels.Panels;
using IsblCheck.Views;
using IsblCheck.Views.Dialogs;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace IsblCheck
{
  /// <summary>
  /// Логика взаимодействия для App.xaml
  /// </summary>
  public partial class App
  {
    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<App>();

    /// <summary>
    /// Сервис представлений.
    /// </summary>
    private IViewService viewService = new ViewService();

    /// <summary>
    /// Обработчик события старта приложения.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void ApplicationStartupHandler(object sender, StartupEventArgs e)
    {
      this.DispatcherUnhandledException += this.DispatcherUnhandledExceptionHandler;
      AppDomain.CurrentDomain.UnhandledException += this.UnhandledExceptionHandler;

      // Инициализируем менеджеры.
      LocalizationManager.Instance.LocalizationProvider = new ResxLocalizationProvider();
      SettingsManager.Instance.SettingsProvider = new SettingsProvider();

      // Регистрируем диалоги.
      DialogManager.Instance
        .BeginConfig()
          .RegisterService<ViewService, IViewService>()
          .RegisterDialog<AboutViewModel>()
          .RegisterDialog<DatabaseCredentialViewModel>()
        .EndConfig();

      // Регистрируем панели.
      PanelManager.Instance
        .BeginConfig()
          .RegisterService<CodeCheckerService, ICodeCheckerService>()
          .RegisterService<ViewService, IViewService>()
          .RegisterContainer<PanelContainerViewModel>()
          .RegisterNamedPanel<SourceExplorerViewModel>("SOURCE_EXPLORER")
          .RegisterNamedPanel<ReportViewerViewModel>("REPORT_VIEWER")
          .RegisterNamedPanel<StartPageViewModel>("START_PAGE")
          .RegisterPanel<DocumentViewerViewModel>()
        .EndConfig();

      // Регистрируем окна.
      WindowManager.Instance.Register<AboutViewModel, AboutWindow>();
      WindowManager.Instance.Register<DatabaseCredentialViewModel, DatabaseCredentialWindow>();
      WindowManager.Instance.Register<PanelContainerViewModel, PanelContainerWindow>();

      // Показываем окно.
      this.viewService.Show(PanelManager.Instance.PanelContainer);

      // Создаем и показываем панель стартовой страницы.
      var startPage = PanelManager.Instance.GetPanel<StartPageViewModel>();
      if (startPage == null)
        startPage = PanelManager.Instance.CreatePanel<StartPageViewModel>();

      startPage.Show();
    }

    /// <summary>
    /// Обработчик события завершения приложения.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void ApplicationExitHandler(object sender, ExitEventArgs e)
    {
      this.DispatcherUnhandledException -= this.DispatcherUnhandledExceptionHandler;
      AppDomain.CurrentDomain.UnhandledException -= this.UnhandledExceptionHandler;
    }

    /// <summary>
    /// Обработчик события возникновения исключения от UI-потока.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void DispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      this.HandleUnhandledException(e.Exception);
      e.Handled = true;
    }

    /// <summary>
    /// Обработчик события возникновения исключения из AppDomain.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
      var exception = e.ExceptionObject as Exception;
      this.HandleUnhandledException(exception);
      if (e.IsTerminating)
        log.Error("ISBL Check is terminating due to an unhandled exception in a secondary thread.", exception);
    }

    /// <summary>
    /// Обработать исключение.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    private void HandleUnhandledException(Exception exception)
    {
      string message = string.Empty;
      try
      {
        AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
        message = string.Format("Unhandled exception in {0}. {1}", assemblyName, exception.Message);
        this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Error);
      }
      catch (Exception ex)
      {
        log.Error("Exception in unhandled exception handler.", ex);
      }
      finally
      {
        log.Error(message, exception);
      }
    }
  }
}
