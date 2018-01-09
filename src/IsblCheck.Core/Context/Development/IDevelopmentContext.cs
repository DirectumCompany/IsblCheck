using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Интерфейс контекста прикладной разработки.
  /// </summary>
  public interface IDevelopmentContext
  {
    /// <summary>
    /// Общие отчеты.
    /// </summary>
    IList<CommonReport> CommonReports { get; }

    /// <summary>
    /// Список констант.
    /// </summary>
    IList<Constant> Constants { get; }

    /// <summary>
    /// Список реквизитов диалогов.
    /// </summary>
    IList<DialogRequisite> DialogRequisites { get; }

    /// <summary>
    /// Список типов диалогов.
    /// </summary>
    IList<Dialog> Dialogs { get; }

    /// <summary>
    /// Список типов карточек документов.
    /// </summary>
    IList<DocumentCardType> DocumentCardTypes { get; }

    /// <summary>
    /// Список реквизитов документов.
    /// </summary>
    IList<DocumentRequisite> DocumentRequisites { get; }

    /// <summary>
    /// Список функций.
    /// </summary>
    IList<Function> Functions { get; }

    /// <summary>
    /// Интегрированные отчеты.
    /// </summary>
    IList<IntegratedReport> IntegratedReports { get; }

    /// <summary>
    /// Список строк локализаций.
    /// </summary>
    IList<LocalizationString> LocalizationStrings { get; }

    /// <summary>
    /// Список реквизитов справочников.
    /// </summary>
    IList<ReferenceRequisite> ReferenceRequisites { get; }

    /// <summary>
    /// Список типов справочников.
    /// </summary>
    IList<ReferenceType> ReferenceTypes { get; }

    /// <summary>
    /// Список блоков типовых маршрутов.
    /// </summary>
    IList<RouteBlock> RouteBlocks { get; }

    /// <summary>
    /// Список сценариев.
    /// </summary>
    IList<Script> Scripts { get; }

    /// <summary>
    /// Список приложений просмотрщиков.
    /// </summary>
    IList<Viewer> Viewers { get; }

    /// <summary>
    /// Типовые маршруты.
    /// </summary>
    IList<StandardRoute> StandardRoutes { get; }

    /// <summary>
    /// Мастера действий.
    /// </summary>
    IList<Wizard> Wizards { get; }
  }
}
