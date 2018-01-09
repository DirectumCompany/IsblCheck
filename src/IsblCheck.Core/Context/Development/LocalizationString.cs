using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Строка локализации.
  /// </summary>
  public class LocalizationString : Component
  {
    /// <summary>
    /// Группа.
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Значения строки.
    /// </summary>
    public IList<LocalizationValue> Values { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LocalizationString()
    {
      this.Values = new List<LocalizationValue>();
    }
  }
}
