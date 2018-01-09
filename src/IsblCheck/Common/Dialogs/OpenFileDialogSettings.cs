namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Настройки диалога открытия файла.
  /// </summary>
  public class OpenFileDialogSettings : FileDialogSettings
  {
    /// <summary>
    /// Мультивыбор.
    /// </summary>
    public bool Multiselect { get; set; }
  }
}
