namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Событие мастера действий.
  /// </summary>
  public class WizardEvent
  {
    /// <summary>
    /// Имя события. В качестве имени используется значение типа события.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Заголовок события.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Текст вычисления.
    /// </summary>
    public string CalculationText { get; set; }
  }
}
