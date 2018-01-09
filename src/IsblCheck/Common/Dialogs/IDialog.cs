using IsblCheck.Common.Windows;

namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Интерфейс диалога.
  /// </summary>
  public interface IDialog : IWindow
  {
    /// <summary>
    /// Результат выполнения диалога.
    /// </summary>
    bool? DialogResult { get; }
  }
}
