namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Событие маршрута / блока маршрута.
  /// </summary>
  public class WorkflowEvent
  {
    /// <summary>
    /// Имя события.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Текст вычисления.
    /// </summary>
    public string CalculationText { get; set; }
  }
}
