namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Настройки файлового диалога.
  /// </summary>
  public abstract class FileDialogSettings
  {
    #region Поля и свойства

    /// <summary>
    /// Добавлять расширение.
    /// </summary>
    public bool AddExtension { get; set; }

    /// <summary>
    /// Проверять существование файла.
    /// </summary>
    public bool CheckFileExists { get; set; }

    /// <summary>
    /// Проверять существование пути.
    /// </summary>
    public bool CheckPathExists { get; set; }

    /// <summary>
    /// Расширение по умолчанию.
    /// </summary>
    public string DefaultExt { get; set; }

    /// <summary>
    /// Имя выбранного файла.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Имя выбранных файлов.
    /// </summary>
    public string[] FileNames { get; set; }

    /// <summary>
    /// Фильтр.
    /// </summary>
    public string Filter { get; set; }

    /// <summary>
    /// Первоначальная директория.
    /// </summary>
    public string InitialDirectory { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; set; }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected FileDialogSettings()
    {
      this.AddExtension = true;
      this.CheckFileExists = true;
      this.CheckPathExists = true;
      this.DefaultExt = string.Empty;
      this.FileName = string.Empty;
      this.FileNames = new[]
      {
        string.Empty
      };
      this.Filter = string.Empty;
      this.InitialDirectory= string.Empty;
      this.Title = string.Empty;
    }

    #endregion
  }
}
