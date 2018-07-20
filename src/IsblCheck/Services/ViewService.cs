using System;
using System.Windows;
using System.Windows.Interop;
using IsblCheck.Common.Dialogs;
using IsblCheck.Common.Native;
using IsblCheck.Common.Windows;

namespace IsblCheck.Services
{
  /// <summary>
  /// Сервис представлений.
  /// </summary>
  public class ViewService : IViewService
  {
    /// <summary>
    /// Закрыть окно.
    /// </summary>
    /// <param name="viewModel">Презентер.</param>
    public void CloseWindow(IWindow viewModel)
    {
      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var window = WindowManager.Instance.FindWindowByViewModel(viewModel);
      window?.Close();
    }

    /// <summary>
    /// Показать окно.
    /// </summary>
    /// <param name="viewModel">Презентер.</param>
    public void Show(IWindow viewModel)
    {
      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var window = WindowManager.Instance.FindWindowByViewModel(viewModel);
      if (window == null)
      {
        window = WindowManager.Instance.CreateWindow(viewModel);
        window.Show();
      }
      else
      {
        if (window.WindowState == WindowState.Minimized)
        {
          var helper = new WindowInteropHelper(window);
          NativeMethods.ShowWindow(helper.Handle, ShowWindowCommands.Restore);
        }
        window.Activate();
      }
    }

    /// <summary>
    /// Показать окно.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="viewModel">Презентер.</param>
    public void Show(IWindow ownerViewModel, IWindow viewModel)
    {
      if (ownerViewModel == null)
        throw new ArgumentNullException(nameof(ownerViewModel));

      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var window = WindowManager.Instance.FindWindowByViewModel(viewModel);
      if (window == null)
      {
        window = WindowManager.Instance.CreateWindow(ownerViewModel, viewModel);
        window.Show();
      }
      else
      {
        if (window.WindowState == WindowState.Minimized)
        {
          var helper = new WindowInteropHelper(window);
          NativeMethods.ShowWindow(helper.Handle, ShowWindowCommands.Restore);
        }
        window.Activate();
      }
    }

    /// <summary>
    /// Показать диалог.
    /// </summary>
    /// <param name="viewModel">Презентер.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    public bool? ShowDialog(IDialog viewModel)
    {
      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var window = WindowManager.Instance.FindWindowByViewModel(viewModel);
      if (window != null)
        return false;

      window = WindowManager.Instance.CreateWindow(viewModel);
      return window.ShowDialog();
    }

    /// <summary>
    /// Показать диалог.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="viewModel">Презентер.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    public bool? ShowDialog(IWindow ownerViewModel, IDialog viewModel)
    {
      if (ownerViewModel == null)
        throw new ArgumentNullException(nameof(ownerViewModel));

      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var window = WindowManager.Instance.FindWindowByViewModel(viewModel);
      if (window != null)
        return false;

      window = WindowManager.Instance.CreateWindow(ownerViewModel, viewModel);
      return window.ShowDialog();
    }

    /// <summary>
    /// Показать сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="caption">Заголовок.</param>
    /// <param name="button">Кнопки.</param>
    /// <param name="icon">Иконка.</param>
    /// <param name="defaultResult">Результат по умолчанию.</param>
    /// <returns>Результат выполнения диалога.</returns>
    public MessageBoxResult ShowMessageBox(string message, string caption = "",
      MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
      MessageBoxResult defaultResult = MessageBoxResult.None)
    {
      if (message == null)
        throw new ArgumentNullException(nameof(message));

      return MessageBox.Show(message, caption, button, icon, defaultResult);
    }

    /// <summary>
    /// Показать сообщение.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="message">Сообщение.</param>
    /// <param name="caption">Заголовок.</param>
    /// <param name="button">Кнопки.</param>
    /// <param name="icon">Иконка.</param>
    /// <param name="defaultResult">Результат по умолчанию.</param>
    /// <returns>Результат выполнения диалога.</returns>
    public MessageBoxResult ShowMessageBox(IWindow ownerViewModel, string message, string caption = "",
      MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
      MessageBoxResult defaultResult = MessageBoxResult.None)
    {
      if (ownerViewModel == null)
        throw new ArgumentNullException(nameof(ownerViewModel));

      if (message == null)
        throw new ArgumentNullException(nameof(message));

      var ownerWindow = WindowManager.Instance.FindWindowByViewModel(ownerViewModel);
      if (ownerWindow == null)
        throw new ArgumentException("Could not found window associated with current view model", nameof(ownerViewModel));

      return MessageBox.Show(ownerWindow, message, caption, button, icon, defaultResult);
    }

    /// <summary>
    /// Показать окно открытия файла.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="settings">Настройки диалога.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    public bool? ShowOpenFileDialog(IWindow ownerViewModel, OpenFileDialogSettings settings)
    {
      if (ownerViewModel == null)
        throw new ArgumentNullException(nameof(ownerViewModel));

      if (settings == null)
        throw new ArgumentNullException(nameof(settings));

      var ownerWindow = WindowManager.Instance.FindWindowByViewModel(ownerViewModel);
      if (ownerWindow == null)
        throw new ArgumentException("Could not found window associated with current view model", nameof(ownerViewModel));

      var wrapper = new OpenFileDialogWrapper(settings);
      return wrapper.ShowDialog(ownerWindow);
    }

    /// <summary>
    /// Показать окно сохранения файла.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="settings">Настройки диалога.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    public bool? ShowSaveFileDialog(IWindow ownerViewModel, SaveFileDialogSettings settings)
    {
      if (ownerViewModel == null)
        throw new ArgumentNullException(nameof(ownerViewModel));

      if (settings == null)
        throw new ArgumentNullException(nameof(settings));

      var ownerWindow = WindowManager.Instance.FindWindowByViewModel(ownerViewModel);
      if (ownerWindow == null)
        throw new ArgumentException("Could not found window associated with current view model", nameof(ownerViewModel));

      var wrapper = new SaveFileDialogWrapper(settings);
      return wrapper.ShowDialog(ownerWindow);
    }
  }
}
