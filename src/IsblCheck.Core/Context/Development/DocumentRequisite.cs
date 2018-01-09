namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Реквизит документа.
  /// </summary>
  public class DocumentRequisite : Requisite
  {
    /// <summary>
    /// Поле.
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Признак хранения значения.
    /// </summary>
    public bool IsStored { get; set; }

    /// <summary>
    /// Признак сгенерированности.
    /// </summary>
    public bool IsGenerated { get; set; }
  }
}
