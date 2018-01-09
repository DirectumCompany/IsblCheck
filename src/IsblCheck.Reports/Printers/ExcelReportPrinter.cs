using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using IsblCheck.Core.Reports;
using IsblCheck.Reports.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace IsblCheck.Reports.Printers
{
  /// <summary>
  /// Принтер отчета в Excel.
  /// </summary>
  public class ExcelReportPrinter : IReportPrinter
  {
    #region Поля и свойства

    /// <summary>
    /// Путь к сохраняемому файлу.
    /// </summary>
    public string FilePath { get; private set; }

    /// <summary>
    /// Шрифт.
    /// </summary>
    public string FontFamily { get; set; }

    /// <summary>
    /// Размер шрифта.
    /// </summary>
    public int FontSize { get; set; }

    #endregion

    #region IReportPrinter

    /// <summary>
    /// Напечатать отчет.
    /// </summary>
    /// <param name="report">Отчет.</param>
    public void Print(IReport report)
    {
      using (SpreadsheetDocument document = SpreadsheetDocument.Create(this.FilePath, SpreadsheetDocumentType.Workbook))
      {
        var workbookPart = document.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet();

        var sheets = workbookPart.Workbook.AppendChild(new Sheets());

        var sheet = new Sheet()
        {
          Id = workbookPart.GetIdOfPart(worksheetPart),
          SheetId = 1,
          Name = "Report"
        };
        sheets.Append(sheet);

        var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

        // Построить заголовок.
        var row = new Row();
        row.Append(
          ConstructCell(Resources.Code, CellValues.String),
          ConstructCell(Resources.Type, CellValues.String),
          ConstructCell(Resources.Message, CellValues.String),
          ConstructCell(Resources.Element, CellValues.String),
          ConstructCell(Resources.Line, CellValues.String),
          ConstructCell(Resources.Column, CellValues.String));
        sheetData.AppendChild(row);

        // Построить данные.
        foreach(var message in report.Messages)
        {
          row = new Row();
          row.Append(
            ConstructCell(message.Code, CellValues.String),
            ConstructCell(message.Type.ToString(), CellValues.String),
            ConstructCell(message.Message, CellValues.String),
            ConstructCell(message.Item.Name, CellValues.String),
            ConstructCell(message.Start.Line.ToString(), CellValues.Number),
            ConstructCell(message.Start.Column.ToString(), CellValues.Number));
          sheetData.AppendChild(row);
        }

        workbookPart.Workbook.Save();
      }
    }

    #endregion

    #region Методы

    private Cell ConstructCell(string value, CellValues dataType)
    {
      return new Cell()
      {
        CellValue = new CellValue(value),
        DataType = new EnumValue<CellValues>(dataType)
      };
    }

    private static double GetWidth(string font, int fontSize, string text)
    {
      var stringFont = new System.Drawing.Font(font, fontSize);
      return GetWidth(stringFont, text);
    }

    private static double GetWidth(System.Drawing.Font stringFont, string text)
    {
      // This formula is based on this article plus a nudge ( + 0.2M )
      // http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.column.width.aspx
      // Truncate(((256 * Solve_For_This + Truncate(128 / 7)) / 256) * 7) = DeterminePixelsOfString

      Size textSize = TextRenderer.MeasureText(text, stringFont);
      double width = (double)(((textSize.Width / (double)7) * 256) - (128 / 7)) / 256;
      width = (double)decimal.Round((decimal)width + 0.2M, 2);
      return width;
    }










    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="filePath">Путь к файлу.</param>
    public ExcelReportPrinter(string filePath)
    {
      this.FilePath = filePath;
    }

    #endregion
  }
}
