namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Позиция в тексте.
  /// </summary>
  public struct TextPosition
  {
    /// <summary>
    /// Индекс начальной позиции.
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    /// Индекс конечной позиции.
    /// </summary>
    public int EndIndex { get; set; }

    /// <summary>
    /// Линия.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Колонка.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length => this.EndIndex - this.StartIndex + 1;
  }
}
