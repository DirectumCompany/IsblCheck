using IsblCheck.Context.Application;
using IsblCheck.Context.Development;
using IsblCheck.Context.Development.Package;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Context.Development;
using IsblCheck.Core.Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IsblCheck.Services
{
  /// <summary>
  /// Сервис проверки кода.
  /// </summary>
  public class CodeCheckerService : ICodeCheckerService
  {
    /// <summary>
    /// Чекер.
    /// </summary>
    private ICodeChecker codeChecker = new CodeChecker();

    /// <summary>
    /// Фабрика контекста приложения.
    /// </summary>
    private IApplicationContextFactory applicationContextFactory = new ApplicationContextFactory();

    /// <summary>
    /// Фабрика контекста разработки.
    /// </summary>
    private IDevelopmentContextFactory developmentContextFactory = new DevelopmentContextFactory();

    public ICodeChecker CodeChecker
    {
      get
      {
        return codeChecker;
      }
    }

    /// <summary>
    /// Очистить провайдеры разработки.
    /// </summary>
    public void ClearProviders()
    {
      this.developmentContextFactory.Providers.Clear();
    }

    /// <summary>
    /// Добавить провайдер разработки.
    /// </summary>
    /// <param name="provider">Провайдер разработки.</param>
    public void AddProvider(IDevelopmentContextProvider provider)
    {
      this.developmentContextFactory.Providers.Add(provider);
      this.codeChecker.ContextManager.Refresh();
    }

    /// <summary>
    /// Проверить документ.
    /// </summary>
    /// <param name="document">Документ.</param>
    /// <param name="progress">Прогресс выполнения.</param>
    /// <returns>Отчет проверки.</returns>
    public Task<IReport> Check(IDocument document, IProgress<int> progress)
    {
      return this.Check(new List<IDocument> { document }, progress);
    }

    /// <summary>
    /// Проверить документы.
    /// </summary>
    /// <param name="documents">Документы.</param>
    /// <param name="progress">Прогессы выполнения.</param>
    /// <returns>Отчет проверки.</returns>
    public Task<IReport> Check(IEnumerable<IDocument> documents, IProgress<int> progress)
    {
      return this.codeChecker.Check(documents, progress);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CodeCheckerService()
    {
      this.codeChecker.ContextManager.Load(applicationContextFactory);
      this.codeChecker.ContextManager.Load(developmentContextFactory);

      // TODO: Загружать из настроек.
      this.codeChecker.RuleManager.LoadLibraries("Rules");
    }
  }
}
