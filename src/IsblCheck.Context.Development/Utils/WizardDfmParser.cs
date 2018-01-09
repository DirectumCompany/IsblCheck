using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Common.Logging;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IsblCheck.Context.Development.Utils
{
  /// <summary>
  /// Парсер описания мастера действий.
  /// </summary>
  public class WizardDfmParser
  {
    /// <summary>
    /// Заголовки событий мастера.
    /// </summary>
    private static readonly Dictionary<string, string> EventTitles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      ["wetWizardBeforeSelection"] = "До выбора",
      ["wetWizardStart"] = "Начало",
      ["wetWizardFinish"] = "Завершение",
      ["wetStepStart"] = "Начало",
      ["wetStepFinish"] = "Завершение",
      ["wetActionExecute"] = "Выполнение",
    };

    /// <summary>
    /// Слушатель для dfm.
    /// </summary>
    private abstract class WizardDfmListener : DfmGrammarBaseListener
    {
      private Wizard wizard;
      private WizardStep step;
      private WizardStepAction action;

      public Wizard Wizard { get { return wizard; } }

      public override void EnterObject([NotNull] DfmGrammarParser.ObjectContext context)
      {
        var objectType = context.type()?.GetText();
        if (objectType == null)
          return;

        if (objectType.Equals("TSBWizard", StringComparison.OrdinalIgnoreCase))
        {
          this.wizard = new Wizard();

          var nameProp = GetPropertyByName(context, "Code");
          if (nameProp != null)
            this.wizard.Name = GetTextPropValue(nameProp) ?? string.Empty;
          else
            this.wizard.Name = string.Empty;

          var titleProp = GetPropertyByName(context, "Title");
          if (titleProp != null)
            this.wizard.Title = GetTextPropValue(titleProp) ?? string.Empty;
          else
            this.wizard.Title = string.Empty;
        }
        else if (objectType.EndsWith("WizardStep", StringComparison.OrdinalIgnoreCase))
        {
          this.step = new WizardStep();

          var nameProp = GetPropertyByName(context, "StepName");
          if (nameProp != null)
            this.step.Name = GetTextPropValue(nameProp) ?? string.Empty;
          else
            this.step.Name = string.Empty;

          var titleProp = GetPropertyByName(context, "Title");
          if (titleProp != null)
            this.step.Title = GetTextPropValue(titleProp) ?? string.Empty;
          else
            this.step.Title = string.Empty;
        }
        else if (objectType.Equals("TSBWizardAction", StringComparison.OrdinalIgnoreCase))
        {
          this.action = new WizardStepAction();

          var nameProp = GetPropertyByName(context, "ActionName");
          if (nameProp != null)
            this.action.Name = GetTextPropValue(nameProp) ?? string.Empty;
          else
            this.action.Name = string.Empty;

          var titleProp = GetPropertyByName(context, "Title");
          if (titleProp != null)
            this.action.Title = GetTextPropValue(titleProp) ?? string.Empty;
          else
            this.action.Title = string.Empty;
        }
      }

      public override void ExitObject([NotNull] DfmGrammarParser.ObjectContext context)
      {
        var objectType = context.type()?.GetText();
        if (objectType == null)
          return;

        if (objectType.EndsWith("WizardStep"))
        {
          if(this.wizard != null && this.step != null)
            this.wizard.Steps.Add(this.step);
          this.step = null;
        }
        else if (objectType == "TSBWizardAction")
        {
          if(this.step != null && this.action != null && !string.IsNullOrEmpty(this.action.CalculationText))
            this.step.Actions.Add(this.action);
          this.action = null;
        }
      }

      public override void EnterProperty([NotNull] DfmGrammarParser.PropertyContext context)
      {
        if (context.qualifiedIdent().GetText() == "Events")
        {
          foreach (var item in context.propertyValue().itemList().item())
          {
            var textProp = GetPropertyByName(item, "ISBLText");
            if (textProp == null)
              continue;

            var text = GetTextPropValue(textProp);

            if (this.action != null)
            {
              this.action.CalculationText = text;
            }
            else
            {
              var typeProp = GetPropertyByName(item, "EventType");
              var name = typeProp?.propertyValue().GetText() ?? string.Empty;

              string title;
              if(!EventTitles.TryGetValue(name, out title))
                title = name;

              var wizardEvent = new WizardEvent
              {
                Name = name,
                Title = title,
                CalculationText = text
              };

              if (this.step != null)
                this.step.Events.Add(wizardEvent);
              else if(this.wizard != null)
                this.wizard.Events.Add(wizardEvent);
            }
          }
        }
      }

      private static DfmGrammarParser.PropertyContext GetPropertyByName(DfmGrammarParser.ObjectContext @object, string propertyName)
      {
        return @object.property().FirstOrDefault(p => string.Equals(p.qualifiedIdent().GetText(), propertyName, StringComparison.OrdinalIgnoreCase));
      }

      private static DfmGrammarParser.PropertyContext GetPropertyByName(DfmGrammarParser.ItemContext item, string propertyName)
      {
        return item.property().FirstOrDefault(p => string.Equals(p.qualifiedIdent().GetText(), propertyName, StringComparison.OrdinalIgnoreCase));
      }

      /// <summary>
      /// Получить значение строкового свойства.
      /// </summary>
      /// <param name="context">Контекст свойства объекта из dfm.</param>
      /// <returns>Значение стровокого свойства.</returns>
      protected abstract string GetTextPropValue(DfmGrammarParser.PropertyContext context);
    }

    private class WizardEncodedDfmListener : WizardDfmListener
    {
      private static readonly Regex StringLiteralRegex = new Regex(@"#(\d+)|'([^']*)'", RegexOptions.Compiled);

      protected override string GetTextPropValue(DfmGrammarParser.PropertyContext context)
      {
        var sb = new StringBuilder();
        foreach (var line in context.propertyValue().@string().STRING_LITERAL())
        {
          foreach (Match m in StringLiteralRegex.Matches(line.GetText()))
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

    private class WizardDecodedDfmListener : WizardDfmListener
    {
      private static readonly Regex DecodedTextPropertyValueRegex = new Regex(@"'((''|[^'])*)'", RegexOptions.Compiled);

      private static readonly ILog log = LogManager.GetLogger<WizardDecodedDfmListener>();

      protected override string GetTextPropValue(DfmGrammarParser.PropertyContext context)
      {
        var stringLiterals = context.propertyValue()?.@string()?.STRING_LITERAL();
        if (stringLiterals != null && stringLiterals.Length == 1)
        {
          var valueMatch = DecodedTextPropertyValueRegex.Match(stringLiterals[0].GetText());
          if (valueMatch.Success)
          {
            var value = valueMatch.Groups[1].Value;
            return value.Replace("''", "'");
          }
        }
        var propertyName = context.qualifiedIdent().GetText();
        log.Warn($"Ошибка чтения dfm мастера действий {this.Wizard?.Name}: Некорректное значение строкового свойства {propertyName}");
        return string.Empty;
      }
    }

    private static Wizard InternalParse<TListener>(string wizardDfm) where TListener : WizardDfmListener, new()
    {
      var inputStream = new AntlrInputStream(wizardDfm);
      var lexer = new DfmGrammarLexer(inputStream);
      // TODO: Нужно как-то обрабатывать ошибки парсера.
      lexer.RemoveErrorListeners();
      var commonTokenStream = new CommonTokenStream(lexer);
      var parser = new DfmGrammarParser(commonTokenStream);
      // TODO: Нужно как-то обрабатывать ошибки парсера.
      parser.RemoveErrorListeners();
      var tree = parser.@object();
      var listener = new TListener();
      var walker = new ParseTreeWalker();
      walker.Walk(listener, tree);
      return listener.Wizard;
    }

    /// <summary>
    /// Распарсить dfm мастера.
    /// </summary>
    /// <param name="wizardDfm">Dfm мастера действий.</param>
    /// <returns>Объект мастера. Может быть не полным (считываются только события и этапы). 
    /// Может быть null, если не удалось распарсить dfm.</returns>
    public static Wizard Parse(string wizardDfm)
    {
      return InternalParse<WizardEncodedDfmListener>(wizardDfm);
    }

    /// <summary>
    /// Распарсить преобразованную dfm мастера (в которой строковые свойства раскодированы).
    /// </summary>
    /// <param name="wizardDfm">Dfm мастера действий.</param>
    /// <returns>Объект мастера. Может быть не полным (считываются только события и этапы). 
    /// Может быть null, если не удалось распарсить dfm.</returns>
    public static Wizard ParseDecoded(string wizardDfm)
    {
      return InternalParse<WizardDecodedDfmListener>(wizardDfm);
    }
  }
}
