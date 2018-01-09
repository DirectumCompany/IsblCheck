namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Настройки диалога сохранения файла.
  /// </summary>
  public class SaveFileDialogSettings : FileDialogSettings
  {
    /// <summary>
    /// Запрашивать создание файла.
    /// </summary>
    public bool CreatePrompt { get; set; }

    /// <summary>
    /// Запрашивать перезапись файла.
    /// </summary>
    public bool OverwritePrompt { get; set; }
  }
}
