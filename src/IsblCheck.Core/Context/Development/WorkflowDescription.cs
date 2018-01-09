using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Описание маршрута.
  /// </summary>
  public class WorkflowDescription
  {
    /// <summary>
    /// Действия маршрута.
    /// </summary>
    public List<Action> Actions { get; } = new List<Action>();

    /// <summary>
    /// Блоки маршрута.
    /// </summary>
    public List<WorkflowBlock> Blocks { get; } = new List<WorkflowBlock>();

    /// <summary>
    /// События маршрута.
    /// </summary>
    public List<WorkflowEvent> Events { get; } = new List<WorkflowEvent>();
  }
}
