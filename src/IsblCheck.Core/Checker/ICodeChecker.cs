using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Интерфейс Контролера проверки исходного кода.
  /// </summary>
  public interface ICodeChecker
  {
    /// <summary>
    /// Менеджер контекста.
    /// </summary>
    IContextManager ContextManager { get; }

    /// <summary>
    /// Менеджер правил.
    /// </summary>
    IRuleManager RuleManager { get; }

    /// <summary>
    /// Менеджер отчетов.
    /// </summary>
    IReportManager ReportManager { get; }

    /// <summary>
    /// Выполнить проверку.
    /// </summary>
    /// <param name="documents">Проверяемые документы.</param>
    /// <returns>Отчет о проверке.</returns>
    Task<IReport> Check(IEnumerable<IDocument> documents);

    /// <summary>
    /// Выполнить проверку.
    /// </summary>
    /// <param name="documents">Проверяемые документы.</param>
    /// <param name="progress">Объект отображения прогресса выполнения.</param>
    /// <returns>Отчет о проверке.</returns>
    Task<IReport> Check(IEnumerable<IDocument> documents, IProgress<int> progress);
  }
}
