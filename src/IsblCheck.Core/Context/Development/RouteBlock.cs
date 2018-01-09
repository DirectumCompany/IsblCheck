namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Блок типового маршрута.
  /// </summary>
  public class RouteBlock : Component
  {
    /// <summary>
    /// Состояние.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// Тип базового блока.
    /// </summary>
    public RouteBlockType BaseBlockType { get; set; }

    /// <summary>
    /// Блок схемы маршрута.
    /// </summary>
    /// <remarks>Может быть null, если описание схемы кривое.</remarks>
    public WorkflowBlock WorkflowBlock { get; set; }
  }
}
