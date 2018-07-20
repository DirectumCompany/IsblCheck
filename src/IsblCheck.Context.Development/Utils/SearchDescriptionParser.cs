using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Utils
{
  // TODO: Поддержка описания поисков реализована пока только для управляемых папок.

  /// <summary>
  /// Парсер описания поиска.
  /// </summary>
  public class SearchDescriptionParser
  {
    private static readonly string[] SearchDescriptionClasses =
    {
      "TSBJobSearchDescription",
      "TSBTaskSearchDescription",
      "TSBEDocumentSearchDescription"
    };

    /// <summary>
    /// Слушатель для dfm.
    /// </summary>
    private class SearchDescriptionDfmListener : DfmGrammarBaseListener, IListenerWithResult<SearchDescription>
    {
      private SearchDescription searchDescription;

      #region DfmGrammarBaseListener

      public override void EnterObject([NotNull] DfmGrammarParser.ObjectContext context)
      {
        var objectType = context.type()?.GetText();
        if (objectType == null)
          return;

        if (SearchDescriptionClasses.Contains(objectType, StringComparer.OrdinalIgnoreCase))
        {
          this.searchDescription = new SearchDescription();
        }
      }

      public override void EnterProperty([NotNull] DfmGrammarParser.PropertyContext context)
      {
        if (context.qualifiedIdent().GetText() == "BeforeSearch.Strings")
        {
          if (this.searchDescription != null)
          {
            this.searchDescription.BeforeSearchEventText = DfmParseUtils.GetTextPropValue(context);
          }
        }
      }

      public SearchDescription GetResult() => this.searchDescription;

      #endregion
    }

    #region Методы

    /// <summary>
    /// Распарсить dfm описания поиска.
    /// </summary>
    /// <param name="searchDescriptionDfm">Dfm описания поиска.</param>
    /// <returns>Объект описания поиска. Может быть null, если не удалось распарсить dfm.</returns>
    public static SearchDescription Parse(string searchDescriptionDfm)
    {
      return AntlrUtils.Parse<SearchDescription, DfmGrammarParser, SearchDescriptionDfmListener>(
        searchDescriptionDfm, p => p.@object());
    }

    #endregion
  }
}
