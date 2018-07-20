namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Инфо правила.
  /// </summary>
  public class RuleInfo : IRuleInfo
  {
    #region IRuleInfo

    /// <summary>
    /// Код.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Имя правила.</param>
    /// <param name="description">Описание.</param>
    public RuleInfo(string name, string description)
    {
      this.Name = name;
      this.Description = description;
    }
  }
}
