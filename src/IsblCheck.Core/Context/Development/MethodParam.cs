using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Параметр метода.
  /// </summary>
  public class MethodParam
  {
    /// <summary>
    /// Номер.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public MethodParamType Type { get; set; }

    /// <summary>
    /// Признак наличия значения по умолчанию.
    /// </summary>
    public bool HasDefaultValue { get; set; }

    /// <summary>
    /// Значение по умолчанию.
    /// </summary>
    public string DefaultValue { get; set; }
  }
}
