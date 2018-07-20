using System;
using System.Windows;
using Microsoft.Win32;

namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Обертка диалогового окна открытия файла.
  /// </summary>
  public sealed class OpenFileDialogWrapper
  {
    #region Поля и свойства

    /// <summary>
    /// Настройки.
    /// </summary>
    private readonly OpenFileDialogSettings settings;

    /// <summary>
    /// Диалог.
    /// </summary>
    private readonly OpenFileDialog openFileDialog;

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
        throw new ArgumentNullException(nameof(owner));

      var result = this.openFileDialog.ShowDialog(owner);
      this.settings.FileName = this.openFileDialog.FileName;
      this.settings.FileNames = this.openFileDialog.FileNames;
      return result;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="settings">Настройки.</param>
    public OpenFileDialogWrapper(OpenFileDialogSettings settings)
    {
      if (settings == null)
        throw new ArgumentNullException(nameof(settings));

      this.settings = settings;
      this.openFileDialog = new OpenFileDialog
      {
        AddExtension = settings.AddExtension,
        CheckFileExists = settings.CheckFileExists,
        CheckPathExists = settings.CheckPathExists,
        DefaultExt = settings.DefaultExt,
        FileName = settings.FileName,
        Filter = settings.Filter,
        InitialDirectory = settings.InitialDirectory,
        Multiselect = settings.Multiselect,
        Title = settings.Title
      };
    }

    #endregion
  }
}
