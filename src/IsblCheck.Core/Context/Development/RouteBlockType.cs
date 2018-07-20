namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Тип блока типового маршрута.
  /// </summary>
  public enum RouteBlockType
  {
    /// <summary>
    /// Начало.
    /// </summary>
    Start = 0,
    /// <summary>
    /// Конец.
    /// </summary>
    Finish,
    /// <summary>
    /// Уведомление.
    /// </summary>
    Notice,
    /// <summary>
    /// Задание.
    /// </summary>
    Step,
    /// <summary>
    /// Условие.
    /// </summary>
    Decision,
    /// <summary>
    /// Ожидание.
    /// </summary>
    Wait,
    /// <summary>
    /// Мониторинг.
    /// </summary>
    Monitor,
    /// <summary>
    /// Сценарий.
    /// </summary>
    Script,
    /// <summary>
    /// Коннектор.
    /// </summary>
    Connector,
    /// <summary>
    /// Подзадача.
    /// </summary>
    SubTask,
    /// <summary>
    /// Жизненный цикла документа.
    /// </summary>
    LifeCycleStage,
    /// <summary>
    /// Пауза.
    /// </summary>
    Pause,
    /// <summary>
    /// Неизвестный тип блока.
    /// </summary>
    Unknown
  }
}
