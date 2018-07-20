using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IsblCheck.Context.Development.Utils
{
  internal class DfmParseUtils
  {
    private static readonly Regex StringLiteralRegex = new Regex(@"#(\d+)|'([^']*)'", RegexOptions.Compiled);

    public static string GetTextPropValue(DfmGrammarParser.PropertyContext context)
    {
      if (context.propertyValue().@string() != null)
      {
        return GetString(context.propertyValue().@string());
      }
      if (context.propertyValue().stringList() != null)
      {
        return string.Join(Environment.NewLine, context.propertyValue().stringList().@string().Select(GetString));
      }
      return null;
    }

    public static string GetString(DfmGrammarParser.StringContext context)
    {
      var sb = new StringBuilder();
      foreach (var stringLiteral in context.STRING_LITERAL())
      {
        foreach (Match m in StringLiteralRegex.Matches(stringLiteral.GetText()))
        {
          if (m.Groups[1].Success)
          {
            sb.Append((char)int.Parse(m.Groups[1].Value));
          }
          else if (m.Groups[2].Success)
          {
            sb.Append(m.Groups[2].Value);
          }
        }
      }
      return sb.ToString();
    }
  }
}
