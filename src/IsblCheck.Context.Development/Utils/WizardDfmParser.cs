using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Common.Logging;
using IsblCheck.Core.Context.Development;

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
    private class WizardDfmListener : DfmGrammarBaseListener, IListenerWithResult<Wizard>
    {
      private Wizard wizard;
      private WizardStep step;
      private WizardStepAction action;

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
            this.wizard.Name = DfmParseUtils.GetTextPropValue(nameProp) ?? string.Empty;
          else
            this.wizard.Name = string.Empty;

          var titleProp = GetPropertyByName(context, "Title");
          if (titleProp != null)
            this.wizard.Title = DfmParseUtils.GetTextPropValue(titleProp) ?? string.Empty;
          else
            this.wizard.Title = string.Empty;
        }
        else if (objectType.EndsWith("WizardStep", StringComparison.OrdinalIgnoreCase))
        {
          this.step = new WizardStep();

          var nameProp = GetPropertyByName(context, "StepName");
          if (nameProp != null)
            this.step.Name = DfmParseUtils.GetTextPropValue(nameProp) ?? string.Empty;
          else
            this.step.Name = string.Empty;

          var titleProp = GetPropertyByName(context, "Title");
          if (titleProp != null)
            this.step.Title = DfmParseUtils.GetTextPropValue(titleProp) ?? string.Empty;
          else
            this.step.Title = string.Empty;
        }
        else if (objectType.Equals("TSBWizardAction", StringComparison.OrdinalIgnoreCase))
        {
          this.action = new WizardStepAction();

          var nameProp = GetPropertyByName(context, "ActionName");
          if (nameProp != null)
            this.action.Name = DfmParseUtils.GetTextPropValue(nameProp) ?? string.Empty;
          else
            this.action.Name = string.Empty;

          var titleProp = GetPropertyByName(context, "Title");
          if (titleProp != null)
            this.action.Title = DfmParseUtils.GetTextPropValue(titleProp) ?? string.Empty;
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
          if(this.step != null && !string.IsNullOrEmpty(this.action?.CalculationText))
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

            var text = DfmParseUtils.GetTextPropValue(textProp);

            if (this.action != null)
            {
              this.action.CalculationText = text;
            }
            else
            {
              var typeProp = GetPropertyByName(item, "EventType");
              var name = typeProp?.propertyValue().GetText() ?? string.Empty;

              if (!EventTitles.TryGetValue(name, out string title))
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

      public Wizard GetResult() => this.wizard;
    }

    /// <summary>
    /// Распарсить dfm мастера.
    /// </summary>
    /// <param name="wizardDfm">Dfm мастера действий.</param>
    /// <returns>Объект мастера. Может быть не полным (считываются только события и этапы). 
    /// Может быть null, если не удалось распарсить dfm.</returns>
    public static Wizard Parse(string wizardDfm)
    {
      return AntlrUtils.Parse<Wizard, DfmGrammarParser, WizardDfmListener>(wizardDfm, p => p.@object());
    }
  }
}
