namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Инфо правила.
  /// </summary>
  public interface IRuleInfo
  {
    /// <summary>
    /// Имя правила.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Описание.
    /// </summary>
    string Description { get; }
  }
}
