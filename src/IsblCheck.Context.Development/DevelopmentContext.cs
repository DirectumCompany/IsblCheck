using IsblCheck.Core.Context.Development;
using System.Collections.Generic;

namespace IsblCheck.Context.Development
{
  /// <summary>
  /// Контекст разработки.
  /// </summary>
  internal class DevelopmentContext : IDevelopmentContext
  {
    /// <summary>
    /// Общие отчеты.
    /// </summary>
    public IList<CommonReport> CommonReports { get; internal set; }

    /// <summary>
    /// Список констант.
    /// </summary>
    public IList<Constant> Constants { get; internal set; }

    /// <summary>
    /// Список реквизитов диалогов.
    /// </summary>
    public IList<DialogRequisite> DialogRequisites { get; internal set; }

    /// <summary>
    /// Список типов диалогов.
    /// </summary>
    public IList<Dialog> Dialogs { get; internal set; }

    /// <summary>
    /// Список типов карточек документов.
    /// </summary>
    public IList<DocumentCardType> DocumentCardTypes { get; internal set; }

    /// <summary>
    /// Список реквизитов документов.
    /// </summary>
    public IList<DocumentRequisite> DocumentRequisites { get; internal set; }

    /// <summary>
    /// Список функций.
    /// </summary>
    public IList<Function> Functions { get; internal set; }

    /// <summary>
    /// Интегрированные отчеты.
    /// </summary>
    public IList<IntegratedReport> IntegratedReports { get; internal set; }

    /// <summary>
    /// Список строк локализаций.
    /// </summary>
    public IList<LocalizationString> LocalizationStrings { get; internal set; }

    /// <summary>
    /// Список реквизитов справочников.
    /// </summary>
    public IList<ReferenceRequisite> ReferenceRequisites { get; internal set; }

    /// <summary>
    /// Список типов справочников.
    /// </summary>
    public IList<ReferenceType> ReferenceTypes { get; internal set; }

    /// <summary>
    /// Список блоков типовых маршрутов.
    /// </summary>
    public IList<RouteBlock> RouteBlocks { get; internal set; }

    /// <summary>
    /// Список сценариев.
    /// </summary>
    public IList<Script> Scripts { get; internal set; }

    /// <summary>
    /// Типовые маршруты.
    /// </summary>
    public IList<StandardRoute> StandardRoutes { get; internal set; }

    /// <summary>
    /// Список приложений просмотрщиков.
    /// </summary>
    public IList<Viewer> Viewers { get; internal set; }

    /// <summary>
    /// Мастера действий.
    /// </summary>
    public IList<Wizard> Wizards { get; internal set; }
  }
}
