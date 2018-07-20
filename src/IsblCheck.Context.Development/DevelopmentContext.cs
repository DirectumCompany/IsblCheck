using System.Collections.Generic;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development
{
  /// <summary>
  /// Контекст разработки.
  /// </summary>
  internal class DevelopmentContext : IDevelopmentContext
  {
    #region IDevelopmentContext

    public IList<CommonReport> CommonReports { get; internal set; }

    public IList<Constant> Constants { get; internal set; }

    public IList<DialogRequisite> DialogRequisites { get; internal set; }

    public IList<Dialog> Dialogs { get; internal set; }

    public IList<DocumentCardType> DocumentCardTypes { get; internal set; }

    public IList<DocumentRequisite> DocumentRequisites { get; internal set; }

    public IList<Function> Functions { get; internal set; }

    public IList<IntegratedReport> IntegratedReports { get; internal set; }

    public IList<LocalizationString> LocalizationStrings { get; internal set; }

    public IList<ManagedFolder> ManagedFolders { get; internal set; }

    public IList<ReferenceRequisite> ReferenceRequisites { get; internal set; }

    public IList<ReferenceType> ReferenceTypes { get; internal set; }

    public IList<RouteBlock> RouteBlocks { get; internal set; }

    public IList<Script> Scripts { get; internal set; }

    public IList<StandardRoute> StandardRoutes { get; internal set; }

    public IList<Viewer> Viewers { get; internal set; }

    public IList<Wizard> Wizards { get; internal set; } 

    #endregion
  }
}
