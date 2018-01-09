using IsblCheck.Core.Checker;

namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Сообщение отчета.
  /// </summary>
  internal struct ReportMessage : IReportMessage
  {
    /// <summary>
    /// Уровень.
    /// </summary>
    public Severity Severity { get; internal set; }

    /// <summary>
    /// Код.
    /// </summary>
    public string Code { get; internal set; }

    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; internal set; }

    /// <summary>
    /// Проверяемый документ.
    /// </summary>
    public IDocument Document { get; internal set; }

    /// <summary>
    /// Позиция.
    /// </summary>
    public TextPosition Position { get; internal set; }
  }
}
