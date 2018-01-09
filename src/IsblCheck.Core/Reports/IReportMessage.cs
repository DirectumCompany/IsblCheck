using IsblCheck.Core.Checker;

namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Интерфейс сообщения отчета.
  /// </summary>
  public interface IReportMessage
  {
    /// <summary>
    /// Уровень.
    /// </summary>
    Severity Severity { get; }

    /// <summary>
    /// Код.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Описание.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Проверяемый документ.
    /// </summary>
    IDocument Document { get; }

    /// <summary>
    /// Позиция.
    /// </summary>
    TextPosition Position { get; }
  }
}
