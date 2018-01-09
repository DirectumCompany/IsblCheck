namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Представление справочника.
  /// </summary>
  public class View
  {
    /// <summary>
    /// Имя представления.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Признак главного представления.
    /// </summary>
    public bool IsMain { get; set; }

    /// <summary>
    /// Описание представления.
    /// </summary>
    public string ListForm { get; set; }

    /// <summary>
    /// Описание представления.
    /// </summary>
    public string CardForm { get; set; }
  }
}
