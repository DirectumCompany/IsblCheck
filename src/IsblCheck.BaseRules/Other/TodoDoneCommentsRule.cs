using System;
using System.Linq;
using System.Text.RegularExpressions;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;

namespace IsblCheck.BaseRules.Other
{
  /// <summary>
  /// Правило поиска комментариев TODO, DONE.
  /// </summary>
  internal class TodoDoneCommentsRule : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "M001";

    /// <summary>
    /// Максимальная длина отображаемой части комментария.
    /// </summary>
    private const int DisplayedCommentMaxLength = 50;

    /// <summary>
    /// Регулярное выражение для однострочных комментариев.
    /// </summary>
    private static readonly Regex oneLineCommentRegex = new Regex(@"(?://|--)(.*)",
      RegexOptions.Multiline | RegexOptions.Compiled);
    
    /// <summary>
    /// Регулярное выражение для многострочных комментариев.
    /// </summary>
    private static readonly Regex multiLineCommentRegex = new Regex(@"/\*(.*?)\*/",
      RegexOptions.Singleline | RegexOptions.Compiled);

    /// <summary>
    /// Регулярное выражение для определения TODO/DONE-комментариев.
    /// </summary>
    private static readonly Regex todoDoneCommentBodyRegex = new Regex(@"^[\s\W\r\n]*(?:todo|done)", 
      RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
     
    #endregion

    #region Поля

    private static readonly Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(TodoDoneCommentsRule).Name, Resources.TodoDoneCommentsRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info => info.Value;

    #endregion

    #region Методы

    /// <summary>
    /// Применить правило.
    /// </summary>
    /// <param name="report">Отчет.</param>
    /// <param name="document">Документ.</param>
    /// <param name="context">Контекст.</param>
    public override void Apply(IReport report, IDocument document, IContext context)
    {
      var allCommentsMatches = oneLineCommentRegex.Matches(document.Text).OfType<Match>()
        .Concat(multiLineCommentRegex.Matches(document.Text).OfType<Match>());
      foreach (var match in allCommentsMatches)
      {
        var commentBody = match.Groups[1].Value;
        if (todoDoneCommentBodyRegex.IsMatch(commentBody))
        {
          var comment = match.Value.Trim();
          var description = string.Format(Resources.TodoDoneComments,
            comment.Substring(0, comment.Length > DisplayedCommentMaxLength ? DisplayedCommentMaxLength : comment.Length));
          report.AddWarning(Code, description, document, GetTextPosition(document.Text, match));
        }
      }
    }

    /// <summary>
    /// Получить позицию комментария в тексте документа.
    /// </summary>
    /// <param name="text">Текст документа.</param>
    /// <param name="comment">Комментарий.</param>
    /// <returns>Позиция в тексте.</returns>
    private static TextPosition GetTextPosition(string text, Match comment)
    {
      var index = comment.Index;
      var substring = text.Substring(0, index);
      var line = (substring.Length - substring.Replace(Environment.NewLine, "").Length) / Environment.NewLine.Length + 1;
      var column = substring.Length - substring.LastIndexOf(Environment.NewLine) - 1;
      if (line != 1)
        column--;
      return new TextPosition
      {
        Line = line,
        Column = column,
        StartIndex = index,
        EndIndex = index + comment.Length - 1
      };
    }
    
    #endregion
  }
}
