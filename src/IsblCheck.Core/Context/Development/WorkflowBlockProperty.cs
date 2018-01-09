using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Свойство блока типового маршрута.
  /// </summary>
  public class WorkflowBlockProperty
  {
    /// <summary>
    /// Имя свойства.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Наименование свойства.
    /// </summary>
    public string Title { get; set; }
  }

  /// <summary>
  /// Вычисляемое свойство блока типового маршрута.
  /// </summary>
  public class WorklflowIsblProperty : WorkflowBlockProperty
  {
    /// <summary>
    /// Текст вычисления свойства.
    /// </summary>
    public string CalculationText { get; set; }
  }
}
