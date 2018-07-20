using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Управляемая папка.
  /// </summary>
  public class ManagedFolder : Component
  {
    /// <summary>
    /// Состояние записи.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// Действия.
    /// </summary>
    public IList<ActionWithHandler> Actions { get; private set; } = new List<ActionWithHandler>();

    /// <summary>
    /// Прикладные методы.
    /// </summary>
    public IList<Method> Methods { get; private set; } = new List<Method>();

    /// <summary>
    /// Описание поиска.
    /// </summary>
    public SearchDescription SearchDescription { get; set; }
  }
}
