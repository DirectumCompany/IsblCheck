using Antlr4.Runtime;

namespace IsblCheck.Core.Reports
{
  /// <summary>
  /// Методы расширения токенов.
  /// </summary>
  public static class TokenExtensions
  {
    /// <summary>
    /// Преобразовать токен в позицию текста.
    /// </summary>
    /// <param name="token">Токен.</param>
    /// <returns>Позиция текста.</returns>
    public static TextPosition ToTextPosition(this IToken token)
    {
      var textPosition = new TextPosition
      {
        StartIndex = token.StartIndex,
        EndIndex = token.StopIndex,
        Line = token.Line,
        Column = token.Column
      };
      return textPosition;      
    }
  }
}
