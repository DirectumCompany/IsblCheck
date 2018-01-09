namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Компонента.
  /// </summary>
  public abstract class Component
  {
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string Title { get; set; }
  }
}
