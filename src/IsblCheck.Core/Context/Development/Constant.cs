namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Константа.
  /// </summary>
  public class Constant : Component
  {
    /// <summary>
    /// Признак репликации константы.
    /// </summary>
    public bool IsReplicated { get; set; }

    /// <summary>
    /// Признак общей константы.
    /// </summary>
    public bool IsCommon { get; set; }
  }
}
