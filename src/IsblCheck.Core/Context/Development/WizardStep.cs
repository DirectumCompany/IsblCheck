using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Этап мастера действий.
  /// </summary>
  public class WizardStep
  {
    /// <summary>
    /// Имя этапа.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Заголовок этапа.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Действия этапа.
    /// </summary>
    public List<WizardStepAction> Actions { get; } = new List<WizardStepAction>();

    /// <summary>
    /// События этапа.
    /// </summary>
    public List<WizardEvent> Events { get; } = new List<WizardEvent>();
  }
}
