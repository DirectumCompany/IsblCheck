namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Тип реквизита.
  /// </summary>
  public enum RequisiteType
  {
    /// <summary>
    /// Дата.
    /// </summary>
    Date,

    /// <summary>
    /// Дробное число.
    /// </summary>
    Float,

    /// <summary>
    /// Выбор.
    /// </summary>
    Pick,

    /// <summary>
    /// Справочник.
    /// </summary>
    Reference,

    /// <summary>
    /// Строка.
    /// </summary>
    String,

    /// <summary>
    /// Текст.
    /// </summary>
    Text,

    /// <summary>
    /// Целое число.
    /// </summary>
    Integer,

    /// <summary>
    /// Большое целое число.
    /// </summary>
    BigInteger,

    /// <summary>
    /// Документ.
    /// </summary>
    Document
  }
}
