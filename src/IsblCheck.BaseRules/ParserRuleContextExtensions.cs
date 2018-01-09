using Antlr4.Runtime;
using IsblCheck.Core.Reports;

namespace IsblCheck.BaseRules
{
  /// <summary>
  /// Класс-расширение для парсера.
  /// </summary>
  public static class ParserRuleContextExtensions
  {
    /// <summary>
    /// Получить позицию в тексте для текущего элемента.
    /// </summary>
    /// <param name="context">Входной элемент.</param>
    /// <returns>Возвращает позицию в тексте.</returns>
    public static TextPosition GetTextPosition(this ParserRuleContext context)
    {
      return new TextPosition
      {
        StartIndex = context.Start.StartIndex,
        EndIndex = context.Stop.StopIndex,
        Line = context.Start.Line,
        Column = context.Start.Column
      };
    }
  }
}
