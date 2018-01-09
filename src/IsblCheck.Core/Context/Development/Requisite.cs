using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Реквизит.
  /// </summary>
  public abstract class Requisite : Component
  {
    /// <summary>
    /// Раздел.
    /// </summary>
    public RequisiteSection Section { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public RequisiteType Type { get; set; }

    /// <summary>
    /// Форматирование.
    /// </summary>
    public RequisiteFormat Format { get; set; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Точность.
    /// </summary>
    public int Precission { get; set; }

    /// <summary>
    /// Тип справочника.
    /// </summary>
    public string ReferenceType { get; set; }

    /// <summary>
    /// Представление справочника.
    /// </summary>
    public string ReferenceView { get; set; }

    /// <summary>
    /// Значения реквизита.
    /// </summary>
    public IList<RequisitePickValue> PickValues { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected Requisite()
    {
      this.PickValues = new List<RequisitePickValue>();
    }
  }
}
