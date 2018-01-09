namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Cценарий.
  /// </summary>
  public class Script : Component
  {
    /// <summary>
    /// Состояние.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// Текст вычисления сценария.
    /// </summary>
    public string CalculationText { get; set; }
  }
}
