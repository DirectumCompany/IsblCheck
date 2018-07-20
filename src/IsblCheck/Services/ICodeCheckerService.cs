using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Development;
using IsblCheck.Core.Reports;

namespace IsblCheck.Services
{
  /// <summary>
  /// Интерфейс сервиса проверки кода.
  /// </summary>
  public interface ICodeCheckerService : IDisposable
  {
    /// <summary>
    /// Очистить провайдеры разработки.
    /// </summary>
    void ClearProviders();

    /// <summary>
    /// Добавить провайдер разработки.
    /// </summary>
    /// <param name="provider">Провайдер разработки.</param>
    void AddProvider(IDevelopmentContextProvider provider);

    /// <summary>
    /// Проверить документ.
    /// </summary>
    /// <param name="document">Документ.</param>
    /// <param name="progress">Прогесс выполнения.</param>
    /// <returns>Отчет проверки.</returns>
    Task<IReport> Check(IDocument document, IProgress<int> progress);

    /// <summary>
    /// Проверить документы.
    /// </summary>
    /// <param name="documents">Документы.</param>
    /// <param name="progress">Прогессы выполнения.</param>
    /// <returns>Отчет проверки.</returns>
    Task<IReport> Check(IEnumerable<IDocument> documents, IProgress<int> progress);

    ICodeChecker CodeChecker { get; }
  }
}
