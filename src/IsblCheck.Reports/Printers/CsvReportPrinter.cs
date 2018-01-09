using IsblCheck.Core.Reports;
using IsblCheck.Reports.Properties;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace IsblCheck.Reports.Printers
{
  /// <summary>
  /// Csv принтер отчетов.
  /// </summary>
  public class CsvReportPrinter : IReportPrinter
  {
    #region Поля и свойства

    /// <summary>
    /// Заголовки.
    /// </summary>
    private static readonly IList<string> columnHeaders = new List<string>
    {
      Resources.Code,
      Resources.Type,
      Resources.Message,
      Resources.Element,
      Resources.Line,
      Resources.Column
    };

    /// <summary>
    /// Путь к сохраняемому файлу.
    /// </summary>
    public string FilePath { get; private set; }

    /// <summary>
    /// Разделитель колонок.
    /// </summary>
    public string Delimiter { get; set; }

    /// <summary>
    /// Экранирующий символ.
    /// </summary>
    public string QuoteChar { get; set; }

    /// <summary>
    /// Режим экранирования.
    /// </summary>
    public CsvQuotingMode Quoting { get; set; }

    /// <summary>
    /// Печатать заголовок.
    /// </summary>
    public bool WithHeader { get; set; }

    #endregion

    #region IReportPrinter

    /// <summary>
    /// Напечатать отчет.
    /// </summary>
    /// <param name="report">Отчет.</param>
    public void Print(IReport report)
    {
      var quotableCharacters = (this.QuoteChar + "\r\n" + this.Delimiter).ToCharArray();
      var doubleQuoteChar = this.QuoteChar + this.QuoteChar;

      Stream stream = null;
      try
      {
        stream = new FileStream(this.FilePath, FileMode.Create, FileAccess.Write);
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
          stream = null;

          if (this.WithHeader)
          {
            var header = this.GetHeader(quotableCharacters, doubleQuoteChar);
            writer.WriteLine(header);
          }

          foreach(var message in report.Messages)
          {
            var messageText = this.GetFormattedMessage(message, quotableCharacters, doubleQuoteChar);
            writer.WriteLine(messageText);
          }
        }
      }
      finally
      {
        if (stream != null)
          stream.Dispose();
      }
    }

    #endregion

    #region Методы

    /// <summary>
    /// Получить форматированное сообщение.
    /// </summary>
    /// <returns>Форматированное сообщение.</returns>
    private string GetFormattedMessage(IReportMessage message, char[] quotableCharacters, string doubleQuoteChar)
    {
      var columns = new[]
      {
        message.Code,
        message.Severity.ToString(),
        message.Description,
        message.Document.Name,
        message.Position.Line.ToString(),
        message.Position.Column.ToString()
      };

      var stringBuilder = new StringBuilder();
      var isFirstColumn = true;

      foreach (string currentColumn in columns)
      {
        if (!isFirstColumn)
          stringBuilder.Append(this.Delimiter);
        isFirstColumn = false;

        var currentColumnQuoting = false;
        switch (this.Quoting)
        {
          case CsvQuotingMode.All:
            currentColumnQuoting = true;
            break;
          case CsvQuotingMode.Nothing:
            currentColumnQuoting = false;
            break;
          default:
            currentColumnQuoting = currentColumn.IndexOfAny(quotableCharacters) >= 0;
            break;
        }

        if (currentColumnQuoting)
        {
          stringBuilder.Append(this.QuoteChar);
          stringBuilder.Append(currentColumn.Replace(this.QuoteChar, doubleQuoteChar));
          stringBuilder.Append(this.QuoteChar);
        }
        else
          stringBuilder.Append(currentColumn);
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Получить заголовок.
    /// </summary>
    /// <param name="quotableCharacters">Символы, которые нужно экранировать.</param>
    /// <param name="doubleQuoteChar">Экранирование экранирующего символа.</param>
    /// <returns>Заголовок.</returns>
    private string GetHeader(char[] quotableCharacters, string doubleQuoteChar)
    {
      var stringBuilder = new StringBuilder();
      var isFirstColumn = true;

      foreach (string currentColumn in columnHeaders)
      {
        if (!isFirstColumn)
          stringBuilder.Append(this.Delimiter);
        isFirstColumn = false;

        var currentColumnQuoting = false;
        switch (this.Quoting)
        {
          case CsvQuotingMode.All:
            currentColumnQuoting = true;
            break;
          case CsvQuotingMode.Nothing:
            currentColumnQuoting = false;
            break;
          default:
            currentColumnQuoting = currentColumn.IndexOfAny(quotableCharacters) >= 0;
            break;
        }

        if (currentColumnQuoting)
        {
          stringBuilder.Append(this.QuoteChar);
          stringBuilder.Append(currentColumn.Replace(this.QuoteChar, doubleQuoteChar));
          stringBuilder.Append(this.QuoteChar);
        }
        else
          stringBuilder.Append(currentColumn);
      }
      return stringBuilder.ToString();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="filePath">Путь к файлу.</param>
    public CsvReportPrinter(string filePath)
    {
      this.FilePath = filePath;
      this.WithHeader = true;
      this.Delimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
      this.Quoting = CsvQuotingMode.Auto;
      this.QuoteChar = "\"";
    }

    #endregion
  }
}
