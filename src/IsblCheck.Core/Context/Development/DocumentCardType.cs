using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Тип карточки электоронного документа.
  /// </summary>
  public class DocumentCardType : Component
  {
    /// <summary>
    /// Состояние.
    /// </summary>
    public ComponentState State { get; set; }

    /// <summary>
    /// Метод нумерации.
    /// </summary>
    public NumerationMethod NumerationMethod { get; set; }

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

    /// <summary>
    /// Реквизиты.
    /// </summary>
    public IList<View> Views { get; private set; } = new List<View>();
  }
}
