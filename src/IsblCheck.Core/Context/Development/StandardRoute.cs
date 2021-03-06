﻿namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Типовой маршрут.
  /// </summary>
  public class StandardRoute : Component
  {
    /// <summary>
    /// Состояние записи.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// Описание схемы маршрута.
    /// </summary>
    public WorkflowDescription WorkflowDescription { get; set; }
  }
}
