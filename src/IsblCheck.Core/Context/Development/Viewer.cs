namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Приложение-просмотрщик.
  /// </summary>
  public class Viewer : Component
  {
    /// <summary>
    /// Расширение.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public ViewerType ViewerType { get; set; }
  }
}
