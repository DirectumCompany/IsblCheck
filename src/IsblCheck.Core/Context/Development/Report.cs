namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Отчет.
  /// </summary>
  public abstract class Report : Component
  {
    /// <summary>
    /// Приложение.
    /// </summary>
    public string Viewer { get; set; }

    /// <summary>
    /// Состояние.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// Текст расчета.
    /// </summary>
    public string CalculationText { get; set; }

    /// <summary>
    /// Текст шаблона.
    /// </summary>
    public string TemplateText { get; set; }
  }
}
