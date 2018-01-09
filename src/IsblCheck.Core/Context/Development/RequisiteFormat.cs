namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Формат реквизита.
  /// </summary>
  public enum RequisiteFormat
  {
    /// <summary>
    /// Без форматирования.
    /// </summary>
    None,

    /// <summary>
    /// Дата.
    /// </summary>
    Date,

    /// <summary>
    /// Дата и время.
    /// </summary>
    DateAndTime,

    /// <summary>
    /// По левому краю.
    /// </summary>
    Left,

    /// <summary>
    /// По правому краю.
    /// </summary>
    Right
  }
}
