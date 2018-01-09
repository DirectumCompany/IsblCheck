namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Действие с обработчиком вычисления.
  /// </summary>
  public class ActionWithHandler : Action
  {
    /// <summary>
    /// Обработчик выполнения действия.
    /// </summary>
    public Method ExecutionHandler { get; set; }
  }
}
