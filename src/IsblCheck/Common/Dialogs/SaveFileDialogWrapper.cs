using Microsoft.Win32;
using System;
using System.Windows;

namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Обертка диалогового окна сохранения файла.
  /// </summary>
  public sealed class SaveFileDialogWrapper
  {
    #region Поля и свойства

    /// <summary>
    /// Настройки.
    /// </summary>
    private readonly SaveFileDialogSettings settings;

    /// <summary>
    /// Диалог.
    /// </summary>
    private readonly SaveFileDialog saveFileDialog;

    #endregion

    #region Методы

    /// <summary>
    /// Показать диалог.
    /// </summary>
    /// <param name="owner">Родительское окно.</param>
    /// <returns>true, в случае если получен результат, иначе false.</returns>
    public bool? ShowDialog(Window owner)
    {
      if (owner == null)
        throw new ArgumentNullException("owner");

      bool? result = this.saveFileDialog.ShowDialog(owner);
      this.settings.FileName = this.saveFileDialog.FileName;
      this.settings.FileNames = this.saveFileDialog.FileNames;
      return result;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="settings">Настройки.</param>
    public SaveFileDialogWrapper(SaveFileDialogSettings settings)
    {
      if (settings == null)
        throw new ArgumentNullException("settings");

      this.settings = settings;
      this.saveFileDialog = new SaveFileDialog
      {
        AddExtension = settings.AddExtension,
        CheckFileExists = settings.CheckFileExists,
        CheckPathExists = settings.CheckPathExists,
        CreatePrompt = settings.CreatePrompt,
        DefaultExt = settings.DefaultExt,
        FileName = settings.FileName,
        Filter = settings.Filter,
        InitialDirectory = settings.InitialDirectory,
        OverwritePrompt = settings.OverwritePrompt,
        Title = settings.Title
      };
    }

    #endregion
  }
}
