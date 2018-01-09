using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Прикладной метод.
  /// </summary>
  public class Method
  {
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Вычисление.
    /// </summary>
    public string CalculationText { get; set; }

    /// <summary>
    /// Параметры метода.
    /// </summary>
    public IList<MethodParam> Params { get; private set; } = new List<MethodParam>();
  }
}
