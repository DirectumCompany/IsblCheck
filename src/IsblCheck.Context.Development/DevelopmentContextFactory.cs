using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development
{
  /// <summary>
  /// Фабрика контекста разработки. 
  /// </summary>
  public class DevelopmentContextFactory : IDevelopmentContextFactory
  {
    #region IDevelopmentContextFactory

    /// <summary>
    /// Список провайдеров.
    /// </summary>
    public IList<IDevelopmentContextProvider> Providers { get; private set; }

    /// <summary>
    /// Создать контекст разработки.
    /// </summary>
    /// <returns>Контекст разработки.</returns>
    public IDevelopmentContext Create()
    {
      var context = new DevelopmentContext
      {
        CommonReports = this.ReadComponents<CommonReport>(),
        Constants = this.ReadComponents<Constant>(),
        DialogRequisites = this.ReadComponents<DialogRequisite>(),
        Dialogs = this.ReadComponents<Dialog>(),
        DocumentRequisites = this.ReadComponents<DocumentRequisite>(),
        DocumentCardTypes = this.ReadComponents<DocumentCardType>(),
        Functions = this.ReadComponents<Function>(),
        IntegratedReports = this.ReadComponents<IntegratedReport>(),
        LocalizationStrings = this.ReadComponents<LocalizationString>(),
        ReferenceRequisites = this.ReadComponents<ReferenceRequisite>(),
        ReferenceTypes = this.ReadComponents<ReferenceType>(),
        RouteBlocks = this.ReadComponents<RouteBlock>(),
        Scripts = this.ReadComponents<Script>(),
        StandardRoutes = this.ReadComponents<StandardRoute>(),
        Viewers = this.ReadComponents<Viewer>(),
        Wizards = this.ReadComponents<Wizard>()
      };

      return context;
    }

    #endregion

    #region Методы

    /// <summary>
    /// Прочитать компоненты.
    /// </summary>
    /// <typeparam name="T">Тип компоненты.</typeparam>
    /// <returns>Список компонент.</returns>
    private IList<T> ReadComponents<T>() where T : Component
    {
      var mergedComponents = new List<T>();
      foreach (var provider in this.Providers)
      {
        var components = provider.ReadComponents<T>().Except(mergedComponents);
        mergedComponents.AddRange(components);
      }

      return mergedComponents;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DevelopmentContextFactory()
    {
      this.Providers = new List<IDevelopmentContextProvider>();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="providers">Список провайдеров.</param>
    public DevelopmentContextFactory(IEnumerable<IDevelopmentContextProvider> providers)
    {
      this.Providers = new List<IDevelopmentContextProvider>(providers);
    }

    #endregion
  }
}
