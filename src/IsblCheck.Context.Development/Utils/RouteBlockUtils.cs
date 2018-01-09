using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Utils
{
  static class RouteBlockUtils
  {
    /// <summary>
    /// Тип базового блока - уведомление.
    /// </summary>
    private const string BaseBlockTypeNoticeReqValue = "SYSRES_SBINTF.NOTICE_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Задание.
    /// </summary>
    private const string BaseBlockTypeJobReqValue = "SYSRES_SBINTF.JOB_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Условие.
    /// </summary>
    private const string BaseBlockTypeDecisionReqValue = "SYSRES_SBINTF.CONDITION_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Ожидание.
    /// </summary>
    private const string BaseBlockTypeWaitReqValue = "SYSRES_SBINTF.WAITING_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Мониторинг.
    /// </summary>
    private const string BaseBlockTypeMonitorReqValue = "SYSRES_SBINTF.MONITORING_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Сценарий.
    /// </summary>
    private const string BaseBlockTypeScriptReqValue = "SYSRES_SBINTF.SCRIPT_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Подзадача.
    /// </summary>
    private const string BaseBlockTypeSubTaskReqValue = "SYSRES_SBINTF.SUBTASK_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Жизненный цикл документа.
    /// </summary>
    private const string BaseBlockTypeLifeCycleStageReqValue = "SYSRES_SBINTF.LIFE_CYCLE_STAGE_BLOCK_DESCRIPTION";

    /// <summary>
    /// Тип базового блока - Пауза.
    /// </summary>
    private const string BaseBlockTypePauseReqValue = "SYSRES_SBINTF.PAUSE_BLOCK_DESCRIPTION";

    /// <summary>
    /// Получить тип блока ТМ из кода строки локализации.
    /// </summary>
    public static RouteBlockType GetBaseBlockType(string valueLocalizeID)
    {
      switch (valueLocalizeID)
      {
        case BaseBlockTypeNoticeReqValue:
          return RouteBlockType.Notice;
        case BaseBlockTypeJobReqValue:
          return RouteBlockType.Step;
        case BaseBlockTypeDecisionReqValue:
          return RouteBlockType.Decision;
        case BaseBlockTypeWaitReqValue:
          return RouteBlockType.Wait;
        case BaseBlockTypeMonitorReqValue:
          return RouteBlockType.Monitor;
        case BaseBlockTypeScriptReqValue:
          return RouteBlockType.Script;
        case BaseBlockTypeSubTaskReqValue:
          return RouteBlockType.SubTask;
        case BaseBlockTypeLifeCycleStageReqValue:
          return RouteBlockType.LifeCycleStage;
        case BaseBlockTypePauseReqValue:
          return RouteBlockType.Pause;
        default:
          return RouteBlockType.Unknown;
      }
    }
  }
}
