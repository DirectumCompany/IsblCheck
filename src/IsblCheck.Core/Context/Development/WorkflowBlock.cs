using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Блок типового маршрута.
  /// </summary>
  public class WorkflowBlock
  {
    /// <summary>
    /// Идентификатор блока.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Тип базового блока.
    /// </summary>
    public RouteBlockType BaseBlockType { get; set; }

    /// <summary>
    /// События.
    /// </summary>
    public List<WorkflowEvent> Events { get; } = new List<WorkflowEvent>();

    /// <summary>
    /// Действия.
    /// </summary>
    public List<Action> Actions { get; } = new List<Action>();

    /// <summary>
    /// Вычисляемые свойства.
    /// </summary>
    public List<WorklflowIsblProperty> IsblProperties { get; } = new List<WorklflowIsblProperty>();
  }
}
