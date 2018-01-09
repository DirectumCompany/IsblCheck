namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Событие.
  /// </summary>
  public class Event
  {
    /// <summary>
    /// Имя события.
    /// </summary>
    public EventType EventType { get; set; }

    /// <summary>
    /// Текст вычисления.
    /// </summary>
    public string CalculationText { get; set; }
  }
}
