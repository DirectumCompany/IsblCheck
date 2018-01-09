using IsblCheck.Common.Dialogs;
using IsblCheck.Common.Windows;
using System.Windows;

namespace IsblCheck.Services
{
  /// <summary>
  /// Интерфейс сервиса представлений.
  /// </summary>
  public interface IViewService
  {
    /// <summary>
    /// Закрыть окно.
    /// </summary>
    /// <param name="viewModel">Презентер.</param>
    void CloseWindow(IWindow viewModel);

    /// <summary>
    /// Показать окно.
    /// </summary>
    /// <param name="viewModel">Презентер.</param>
    void Show(IWindow viewModel);

    /// <summary>
    /// Показать окно.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="viewModel">Презентер.</param>
    void Show(IWindow ownerViewModel, IWindow viewModel);

    /// <summary>
    /// Показать диалог.
    /// </summary>
    /// <param name="viewModel">Презентер.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    bool? ShowDialog(IDialog viewModel);

    /// <summary>
    /// Показать диалог.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="viewModel">Презентер.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    bool? ShowDialog(IWindow ownerViewModel, IDialog viewModel);

    /// <summary>
    /// Показать сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="caption">Заголовок.</param>
    /// <param name="button">Кнопки.</param>
    /// <param name="icon">Иконка.</param>
    /// <param name="defaultResult">Результат по умолчанию.</param>
    /// <returns>Результат выполнения диалога.</returns>
    MessageBoxResult ShowMessageBox(string message, string caption = "",
      MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
      MessageBoxResult defaultResult = MessageBoxResult.None);

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
    MessageBoxResult ShowMessageBox(IWindow ownerViewModel, string message, string caption = "",
      MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
      MessageBoxResult defaultResult = MessageBoxResult.None);

    /// <summary>
    /// Показать окно открытия файла.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="settings">Настройки диалога.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    bool? ShowOpenFileDialog(IWindow ownerViewModel, OpenFileDialogSettings settings);

    /// <summary>
    /// Показать окно сохранения файла.
    /// </summary>
    /// <param name="ownerViewModel">Родительский презентер.</param>
    /// <param name="settings">Настройки диалога.</param>
    /// <returns>true, в случае получения результата, иначе false.</returns>
    bool? ShowSaveFileDialog(IWindow ownerViewModel, SaveFileDialogSettings settings);
  }
}
