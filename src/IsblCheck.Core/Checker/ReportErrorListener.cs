using Antlr4.Runtime;
using IsblCheck.Core.Reports;

namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Слушатель синтаксических ошибок для отчета.
  /// </summary>
  internal class ReportErrorListener : BaseErrorListener
  {
    #region Поля и свойства

    /// <summary>
    /// Отчет.
    /// </summary>
    private readonly IReport report;

    /// <summary>
    /// Проверяемый документ.
    /// </summary>
    private readonly IDocument document;

    #endregion

    #region BaseErrorListener

    /// <summary>
    /// Синтаксическая ошиюка.
    /// </summary>
    /// <param name="recognizer">Распознаватель.</param>
    /// <param name="offendingSymbol">Предлагаемый токен.</param>
    /// <param name="line">Строка.</param>
    /// <param name="charPositionInLine">Колонка.</param>
    /// <param name="msg">Сообщение.</param>
    /// <param name="e">Исключение.</param>
    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
      // TODO: Разобраться с начальной и конечной позицией.
      var start = offendingSymbol.ToTextPosition();

      // TODO: Добавить определение кода ошибки.
      report.AddError("", msg, document, start);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="report">Отчет.</param>
    /// <param name="document">Проверяемый документ.</param>
    internal ReportErrorListener(IReport report, IDocument document)
    {
      this.report = report;
      this.document = document;
    }

    #endregion
  }
}
