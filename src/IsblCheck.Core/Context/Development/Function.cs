using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Функция.
  /// </summary>
  public class Function : Component
  {
    /// <summary>
    /// Признак системной функции.
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Аргументы функции.
    /// </summary>
    public IList<FunctionArgument> Arguments { get; private set; } = new List<FunctionArgument>();

    /// <summary>
    /// Текст вычисления функции.
    /// </summary>
    public string CalculationText { get; set; }

    /// <summary>
    /// Справка по функции.
    /// </summary>
    public string Help { get; set; }

    /// <summary>
    /// Комментарий к функции.
    /// </summary>
    public string Comment { get; set; }
  }
}
