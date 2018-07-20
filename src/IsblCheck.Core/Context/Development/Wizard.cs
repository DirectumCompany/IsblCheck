using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Мастер действия.
  /// </summary>
  public class Wizard : Component
  {
    /// <summary>
    /// Состояние записи.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// События мастера.
    /// </summary>
    public List<WizardEvent> Events { get; } = new List<WizardEvent>();

    /// <summary>
    /// Этапы мастера.
    /// </summary>
    public List<WizardStep> Steps { get; } = new List<WizardStep>();
  }
}
