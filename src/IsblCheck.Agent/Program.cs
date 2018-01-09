using Common.Logging;
using IsblCheck.Core.Checker;
using System;

namespace IsblCheck.Agent
{
  internal class Program
  {
    private static ILog log = LogManager.GetLogger(typeof(Program));

    internal static void Main()
    {
      log.Info("Запуск агента");
      ICodeChecker checker = new CodeChecker();
      checker.Configure();

      log.Info("Проверка разработки");
      var context = checker.ContextManager.Context.Development;
      var documents = CodeCheckerCalculationProvider.GetDocuments(context);
      var report = checker.Check(documents).Result;

      log.Info("Генерация отчета");
      report.Print();

      log.Info("Агент успешно завершил работу");
    }
  }
}
