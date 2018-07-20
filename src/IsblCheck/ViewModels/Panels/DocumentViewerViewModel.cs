using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using IsblCheck.Context.Application;
using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Reports;
using IsblCheck.Properties;
using IsblCheck.UI.Editor;
using IDocument = IsblCheck.Core.Checker.IDocument;

namespace IsblCheck.ViewModels.Panels
{
  /// <summary>
  /// Презентер документа.
  /// </summary>
  public class DocumentViewerViewModel : PanelViewModelBase
  {
    #region Поля и свойства

    /// <summary>
    /// Контекст приложения.
    /// </summary>
    private static readonly IApplicationContext applicationContext;

    /// <summary>
    /// Цвет ошибки.
    /// </summary>
    private static readonly Color ErrorColor = (Color)ColorConverter.ConvertFromString("#FFFF0000");

    /// <summary>
    /// Цвет предупреждения.
    /// </summary>
    private static readonly Color WarningColor = (Color)ColorConverter.ConvertFromString("#FFFFA500");

    /// <summary>
    /// Цвет предупреждения.
    /// </summary>
    private static readonly Color InformationColor = (Color)ColorConverter.ConvertFromString("#FF01A400");

    /// <summary>
    /// Документ.
    /// </summary>
    public IDocument Document { get; }

    /// <summary>
    /// Документ.
    /// </summary>
    public TextDocument TextDocument
    {
      get { return this.textDocument; }
      set
      {
        if (this.textDocument == value)
          return;
        this.textDocument = value;
        this.RaisePropertyChanged();
      }
    }
    private TextDocument textDocument;

    /// <summary>
    /// Подсветка синтаксиса.
    /// </summary>
    public IHighlightingDefinition SyntaxHighlighting
    {
      get { return this.syntaxHighlighting; }
      set
      {
        if (this.syntaxHighlighting == value)
          return;
        this.syntaxHighlighting = value;
        this.RaisePropertyChanged();
      }
    }
    private IHighlightingDefinition syntaxHighlighting;

    /// <summary>
    /// Маркеры.
    /// </summary>
    public TextSegmentCollection<TextMarker> Markers
    {
      get { return this.markers; }
      set
      {
        if (this.markers == value)
          return;
        this.markers = value;
        this.RaisePropertyChanged();
      }
    }
    private TextSegmentCollection<TextMarker> markers;

    /// <summary>
    /// Позиция каретки.
    /// </summary>
    public int CaretOffset
    {
      get { return this.caretOffset; }
      set
      {
        if (this.caretOffset == value)
          return;
        this.caretOffset = value;
        this.RaisePropertyChanged();
      }
    }
    private int caretOffset;

    #endregion

    #region Методы

    /// <summary>
    /// Добавить сообщения отчета проверки.
    /// </summary>
    /// <param name="messages">Сообщения.</param>
    public void AddReportMessages(IEnumerable<IReportMessage> messages)
    {
      foreach (var message in messages)
      {
        var marker = new TextMarker(message.Position.StartIndex, message.Position.Length)
        {
          MarkerType = TextMarkerType.SquigglyUnderline | TextMarkerType.LineInScrollBar
        };
        switch (message.Severity)
        {
          case Severity.Error:
            marker.MarkerColor = ErrorColor;
            break;
          case Severity.Warning:
            marker.MarkerColor = WarningColor;
            break;
          case Severity.Information:
            marker.MarkerColor = InformationColor;
            break;
        }
        marker.ToolTip = message.Description;
        this.Markers.Add(marker);
      }
    }

    /// <summary>
    /// Очистить сообщения отчета проверки.
    /// </summary>
    public void ClearReportMessages()
    {
      this.Markers.Clear();
    }

    /// <summary>
    /// Получить описание подстветки ISBL.
    /// </summary>
    /// <returns>Описание подсветки ISBL.</returns>
    private static IHighlightingDefinition LoadIsblHighlightingDefinition()
    {
      using (var stream = new MemoryStream(Resources.Isbl))
      {
        using (XmlReader reader = new XmlTextReader(stream))
        {
          var xshd = HighlightingLoader.LoadXshd(reader);

          var dynamicRuleSet = xshd.Elements
            .OfType<XshdRuleSet>()
            .First(o => o.Name == "Dynamic");

          var constants = new XshdKeywords
          {
            ColorReference = new XshdReference<XshdColor>(null, "Constant")
          };
          foreach (var constant in applicationContext.Constants.Keys)
            constants.Words.Add(constant);
          foreach (var enumValue in applicationContext.Enums.Keys)
            constants.Words.Add(enumValue);
          dynamicRuleSet.Elements.Add(constants);

          var predefinedVariables = new XshdKeywords
          {
            ColorReference = new XshdReference<XshdColor>(null, "PredefinedVariable")
          };
          foreach (var predefinedVariable in applicationContext.PredefinedVariables)
            predefinedVariables.Words.Add(predefinedVariable);
          dynamicRuleSet.Elements.Add(predefinedVariables);

          return HighlightingLoader.Load(xshd, HighlightingManager.Instance);
        }
      }
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static DocumentViewerViewModel()
    {
      IApplicationContextFactory factory = new ApplicationContextFactory();
      applicationContext = factory.Create();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="document">Документ.</param>
    public DocumentViewerViewModel(IDocument document)
    {
      this.Document = document;
      this.Title = this.Document.Name;
      this.TextDocument = new TextDocument(this.Document.Text);
      this.SyntaxHighlighting = LoadIsblHighlightingDefinition();
      this.Markers = new TextSegmentCollection<TextMarker>(this.TextDocument);
    }

    #endregion
  }
}
