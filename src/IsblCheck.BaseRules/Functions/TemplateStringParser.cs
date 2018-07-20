using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IsblCheck.Core.Reports;

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Описатель форматной строки.
  /// </summary>
  public struct FormatItem
  {
    /// <summary>
    /// Индекс аргумента функции форматирования для описателя.
    /// </summary>
    public int Index;
    /// <summary>
    /// Текст описателя.
    /// </summary>
    public string Text;
    /// <summary>
    /// Позиция описателя в строке форматирования.
    /// </summary>
    public TextPosition Pos;
  }

  /// <summary>
  /// Парсер форматной строки.
  /// </summary>
  public class TemplateStringParser
  {
    /// <summary>
    /// Регулярное выражение для поиска описателей форматной строки.
    /// </summary>
    private static readonly Regex FormatArgRegex =
      new Regex(@"(%%)|(?<item>%((?<index>\d+)\:)?-?(\d+)?(\.\d+)?[duefgnmpsx])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly string templateString;

    /// <summary>
    /// Описателя форматной строки.
    /// </summary>
    public List<FormatItem> FormatItems { get; }

    /// <summary>
    /// Распарсить строку.
    /// </summary>
    public void Parse()
    {
      var matches = FormatArgRegex.Matches(templateString);
      var currentArgIndex = -1;
      var currentLine = 0;
      var lastNewLineCharPos = -1;
      var lastMatchPos = 0;
      for (var i = 0; i < matches.Count; i++)
      {
        var match = matches[i];
        if (match.Groups["item"].Value != string.Empty)
        {
          if (match.Groups["index"].Value != string.Empty)
          {
            currentArgIndex = int.Parse(match.Groups["index"].Value);
          }
          else
          {
            currentArgIndex++;
          }
          for (var j = lastMatchPos; j < match.Index; j++)
          {
            if (templateString[j] == '\n')
            {
              currentLine++;
              lastNewLineCharPos = j;
            }
          }
          lastMatchPos = match.Index + match.Length;
          this.FormatItems.Add(new FormatItem
          {
            Index = currentArgIndex,
            Text = match.Value,
            Pos = new TextPosition
            {
              StartIndex = match.Index,
              EndIndex = match.Index + match.Length - 1,
              Column = match.Index - lastNewLineCharPos - 1,
              Line = currentLine
            }
          });
        }
      }
    }

    public TemplateStringParser(string templateString)
    {
      if (templateString == null)
        throw new ArgumentNullException(nameof(templateString));
      this.templateString = templateString;
      this.FormatItems = new List<FormatItem>();
    }
  }
}
