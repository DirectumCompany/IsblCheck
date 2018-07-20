namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Тип компоненты.
  /// </summary>
  public enum ComponentType
  {
    /// <summary>
    /// Неизвестный.
    /// </summary>
    Unknown,

    /// <summary>
    /// Основной отчет.
    /// </summary>
    CommonReport,

    /// <summary>
    /// Диалог.
    /// </summary>
    Dialog,

    /// <summary>
    /// ТКЭД.
    /// </summary>
    DocumentCardType,
    
    /// <summary>
    /// Функция.
    /// </summary>
    Function,

    /// <summary>
    /// Интегрированный отчет.
    /// </summary>
    IntegratedReport,

    /// <summary>
    /// Управляемая папка.
    /// </summary>
    ManagedFolder,

    /// <summary>
    /// Тип справочника.
    /// </summary>
    ReferenceType,

    /// <summary>
    /// Сценарий.
    /// </summary>
    Script,

    /// <summary>
    /// Блок типового маршрута.
    /// </summary>
    RouteBlock,

    /// <summary>
    /// Типовой маршрут.
    /// </summary>
    StandardRoute,

    /// <summary>
    /// Мастер действия.
    /// </summary>
    Wizard
  }
}
