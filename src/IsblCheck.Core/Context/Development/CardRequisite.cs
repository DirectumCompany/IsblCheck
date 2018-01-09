using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Реквизит карточки.
  /// </summary>
  public class CardRequisite
  {
    /// <summary>
    /// Номер.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Секция.
    /// </summary>
    public RequisiteSection Section { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Признак обязательности.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Признак фильтратора.
    /// </summary>
    public bool IsFilter { get; set; }

    /// <summary>
    /// Признак лидируемости.
    /// </summary>
    public bool IsLeading { get; set; }

    /// <summary>
    /// Признак контроля.
    /// </summary>
    public bool IsControl { get; set; }

    /// <summary>
    /// Признак сохранения предыдущего значения.
    /// </summary>
    public bool IsLastValueSaved { get; set; }

    /// <summary>
    /// Признак ограничения выбора.
    /// </summary>
    public bool IsSelectionConstrained { get; set; }

    /// <summary>
    /// События.
    /// </summary>
    public IList<Event> Events { get; set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CardRequisite()
    {
      this.Events = new List<Event>();
    }
  }
}
