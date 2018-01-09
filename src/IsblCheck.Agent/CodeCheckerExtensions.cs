using Common.Logging;
using IsblCheck.Agent.Configuration;
using IsblCheck.Context.Application;
using IsblCheck.Context.Development;
using IsblCheck.Context.Development.Database;
using IsblCheck.Context.Development.Folder;
using IsblCheck.Context.Development.Package;
using IsblCheck.Core.Checker;
using IsblCheck.Reports.Printers;
using System.Configuration;

namespace IsblCheck.Agent
{
  /// <summary>
  /// Методы расширения для CodeChecker.
  /// </summary>
  public static class CodeCheckerExtensions
  {
    private static ILog log = LogManager.GetLogger(typeof(CodeCheckerExtensions));

    /// <summary>
    /// Сконфигурировать чекер.
    /// </summary>
    /// <param name="checker">Чекер.</param>
    public static void Configure(this ICodeChecker checker)
    {
      var section = (AgentConfigurationSection)ConfigurationManager.GetSection("agent");

      log.Trace("Загрузка контекста приложения");
      var applicationContextFactory = new ApplicationContextFactory();
      checker.ContextManager.Load(applicationContextFactory);

      log.Trace("Загрузка контекста разработки");
      var developmentContextFactory = new DevelopmentContextFactory();
      foreach (ContextProviderElement element in section.ContextProviders)
      {
        switch (element.Provider)
        {
          case ContextProviderType.Package:
            log.Trace($"Найден загрузчик контекста из пакета {element.FilePath}");
            var packageProvider = new PackageProvider(element.FilePath);
            developmentContextFactory.Providers.Add(packageProvider);
            break;
          case ContextProviderType.Database:
            log.Trace($"Найден загрузчик контекста из базы данных {element.ConnectionString}");
            var databaseProvider = new DatabaseProvider(element.ConnectionString);
            developmentContextFactory.Providers.Add(databaseProvider);
            break;
          case ContextProviderType.Folder:
            log.Trace($"Найден загрузчик контекста из папки {element.FolderPath}");
            var folderProvider = new FolderProvider(element.FolderPath);
            developmentContextFactory.Providers.Add(folderProvider);
            break;
        }
      }
      checker.ContextManager.Load(developmentContextFactory);

      log.Trace("Загрузка сборок с правилами анализа");
      if (!string.IsNullOrEmpty(section.RuleLibraryPath))
        checker.RuleManager.LoadLibraries(section.RuleLibraryPath);

      log.Trace("Загрузка генераторов отчетов");
      if (section.ReportPrinters != null)
      {
        foreach (ReportPrinterElement element in section.ReportPrinters)
        {
          switch (element.Printer)
          {
            case ReportPrinterType.Console:
              var consoleReportPrinter = new ConsoleReportPrinter();
              checker.ReportManager.Printers.Add(consoleReportPrinter);
              break;
            case ReportPrinterType.CSV:
              var csvReportPrinter = new CsvReportPrinter(element.FilePath);
              checker.ReportManager.Printers.Add(csvReportPrinter);
              break;
          }
        }
      }
    }
  }
}
