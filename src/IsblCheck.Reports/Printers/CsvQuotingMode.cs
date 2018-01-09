namespace IsblCheck.Reports.Printers
{
  /// <summary>
  /// Режим экранирования для CSV файла.
  /// </summary>
  public enum CsvQuotingMode
  {
    /// <summary>
    /// Экранировать все колонки.
    /// </summary>
    All,

    /// <summary>
    /// Ничего не экранировать.
    /// </summary>
    Nothing,

    /// <summary>
    /// Экранировать, если есть недопустимый символ.
    /// </summary>
    Auto
  }
}
