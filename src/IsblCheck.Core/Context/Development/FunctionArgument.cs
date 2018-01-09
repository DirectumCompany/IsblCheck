namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Аргумент функции.
  /// </summary>
  public class FunctionArgument
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
    public FunctionArgumentType Type { get; set; }

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
