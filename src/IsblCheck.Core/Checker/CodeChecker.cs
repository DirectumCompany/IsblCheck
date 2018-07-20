using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Logging;
using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;

namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Контролер проверки исходного кода.
  /// </summary>
  public class CodeChecker : ICodeChecker
  {
    #region Поля и свойства

    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<ContextManager>();

    private readonly ContextManager contextManager = new ContextManager();
    private readonly RuleManager ruleManager = new RuleManager();
    private readonly ReportManager reportManager = new ReportManager();

    #endregion

    #region ICodeChecker

    /// <summary>
    /// Менеджер контекста.
    /// </summary>
    public IContextManager ContextManager => this.contextManager;

    /// <summary>
    /// Менеджер правил.
    /// </summary>
    public IRuleManager RuleManager => this.ruleManager;

    /// <summary>
    /// Менеджер отчетов.
    /// </summary>
    public IReportManager ReportManager => this.reportManager;

    /// <summary>
    /// Выполнить проверку.
    /// </summary>
    /// <param name="documents">Проверяемые документы.</param>
    /// <returns>Отчет о проверке.</returns>
    public Task<IReport> Check(IEnumerable<IDocument> documents)
    {
      return this.Check(documents, null);
    }

    /// <summary>
    /// Выполнить проверку.
    /// </summary>
    /// <param name="documents">Проверяемые документы.</param>
    /// <param name="progress">Объект отображения прогресса выполнения.</param>
    /// <returns>Отчет о проверке.</returns>
    public Task<IReport> Check(IEnumerable<IDocument> documents, IProgress<int> progress)
    {
      var rules = this.RuleManager.GetAllRules();
      var report = this.ReportManager.Create();
      var context = this.ContextManager.Context;
      var counter = new ProgressCounter(progress);

      return Task.Run(() =>
      {
        Parallel.ForEach(documents, document =>
        {
          foreach (var rule in rules)
          {
            try
            {
              rule.Apply(report, document, context);
            }
            catch (Exception ex)
            {
              log.Error(ex.Message, ex);
            }
          }
          counter.Increment();
        });
        return report;
      });
    }

    #endregion

    #region IDisposable Support

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          this.ruleManager.Dispose();
        }
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }

    #endregion
  }
}
