using IsblCheck.Core.Checker;
using System.Collections.Generic;

namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Интерфейс отчета проверки кода.
  /// </summary>
  public interface IReport
  {
    /// <summary>
    /// Сообщения.
    /// </summary>
    IEnumerable<IReportMessage> Messages { get; }

    /// <summary>
    /// Добавить ошибку.
    /// </summary>
    /// <param name="severity">Уровень.</param>
    /// <param name="code">Код.</param>
    /// <param name="description">Описание.</param>
    /// <param name="document">Документ.</param>
    /// <param name="position">Позиция.</param>
    void Add(Severity severity, string code, string description, IDocument document, TextPosition position);

    /// <summary>
    /// Добавить ошибку.
    /// </summary>
    /// <param name="code">Код.</param>
    /// <param name="description">Описание.</param>
    /// <param name="document">Документ.</param>
    /// <param name="position">Позиция.</param>
    void AddError(string code, string description, IDocument document, TextPosition position);

    /// <summary>
    /// Добавить ошибку.
    /// </summary>
    /// <param name="code">Код.</param>
    /// <param name="description">Описание.</param>
    /// <param name="document">Документ.</param>
    /// <param name="position">Позиция.</param>
    void AddWarning(string code, string description, IDocument document, TextPosition position);

    /// <summary>
    /// Добавить ошибку.
    /// </summary>
    /// <param name="code">Код.</param>
    /// <param name="description">Описание.</param>
    /// <param name="document">Документ.</param>
    /// <param name="position">Позиция.</param>
    void AddInformation(string code, string description, IDocument document, TextPosition position);

    /// <summary>
    /// Напечатать отчет.
    /// </summary>
    void Print();
  }
}
