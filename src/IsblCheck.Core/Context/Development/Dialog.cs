using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Диалог.
  /// </summary>
  public class Dialog : Component
  {
    /// <summary>
    /// Форма карточки.
    /// </summary>
    public string CardForm { get; set; }

    /// <summary>
    /// Действия.
    /// </summary>
    public IList<ActionWithHandler> Actions { get; private set; } = new List<ActionWithHandler>();

    /// <summary>
    /// Прикладные методы.
    /// </summary>
    public IList<Method> Methods { get; private set; } = new List<Method>();

    /// <summary>
    /// События.
    /// </summary>
    public IList<Event> Events { get; private set; } = new List<Event>();

    /// <summary>
    /// Реквизиты.
    /// </summary>
    public IList<CardRequisite> Requisites { get; private set; } = new List<CardRequisite>();
  }
}
