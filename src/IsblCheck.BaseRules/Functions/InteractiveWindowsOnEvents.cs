using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using IsblCheck.BaseRules.Properties;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;
using IsblCheck.Core.Context.Development;
using IsblCheck.Core.Parser;
using IsblCheck.Core.Reports;
using IsblCheck.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.BaseRules.Functions
{
  class InteractiveWindowsOnEvents : AbstractRule
  {
    #region Константы

    /// <summary>
    /// Код правила.
    /// </summary>
    private const string Code = "J006";

    /// <summary>
    /// Префикс, с которого должен начинаться document.Path.
    /// </summary>
    private const string DocumentPathPrefix = "Events.";

    #endregion

    #region Вложенные классы

    /// <summary>
    /// Поиск использования интерактивных окон в событиях Сохранить до/после, форма-карточка скрытие.
    /// </summary>
    private class InteractiveWindowsOnEventsListener : IsblBaseListener
    {
      /// <summary>
      /// Список контекстов, попадающих под данное правило. 
      /// </summary>
      public List<IsblParser.FunctionContext> ContextWithFunctionErrors { get; } = new List<IsblParser.FunctionContext>();

      /// <summary>
      /// Список интерактивных функций.
      /// </summary>
      private static readonly HashSet<string> FunctionsWithInteractiveWindow = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
      {
        "CreateCommentToAmendNoticeDialog",
        "CreateDialog",
        "CreateDualListDialog",
        "CreateEditor",
        "CreateFolderDialog",
        "CreateInputDialog",
        "CreateOpenDialog",
        "CreateRefusalReasonsRequestDialog",
        "CreateSaveDialog",
        "CreateTreeListSelectDialog",
        "CSelectSQL",
        "EditText",
        "InputDialog",
        "InputDialogEx",
        "MessageBox",
        "MessageBoxEx",
        "RMOpenDocumentActionsDialog",
        "RMShowAssignmentTreeInDialog",
        "SelectContactFromDialog",
        "SelectFolderDialog",
        "SelectFromDualList",
        "SelectionFromContactPersons",
        "SelectScriptFromList",
        "SelectSQL",
        "SetDialogFromParams",
        "ShowConstantsInputDialog",
        "ShowHintForFillReqs",
        "ShowDialog",
        "ShowMessage",
        "Ввод",
        "ВводМеню",
        "ВыборSQL",
        "ДиалогДаНет",
        "Меню",
        "МенюРасш",
        "Окно",
        "ПВыборSQL",
        "РедТекст",
        "РедактироватьРТФ"
      };

      /// <summary>
      /// Вход в выражение вызова функции.
      /// </summary>
      /// <param name="context">Контекст.</param>
      public override void EnterFunction(IsblParser.FunctionContext context)
      {
        var functionName = context.identifier().GetText();
        if (FunctionsWithInteractiveWindow.Contains(functionName))
        {
          ContextWithFunctionErrors.Add(context);
        }
      }
    }

    #endregion

    #region Поля

    private static Lazy<IRuleInfo> info = new Lazy<IRuleInfo>(() =>
      new RuleInfo(typeof(InteractiveWindowsOnEvents).Name, Resources.InteractiveWindowsOnEventsRuleDescription), true);

    /// <summary>
    /// Инфо правила.
    /// </summary>
    public static IRuleInfo Info { get { return info.Value; } }

    /// <summary>
    /// События, проверяемые на наличие интерактивных функций.
    /// </summary>
    private static readonly Dictionary<EventType, string> CheckingEvents = new Dictionary<EventType, string>
    {
      { EventType.AfterUpdate, "Сохранение после" },
      { EventType.BeforeUpdate, "Сохранение до" },
      { EventType.AfterDelete, "Удаление после" },
      { EventType.BeforeDelete, "Удаление до" },
      { EventType.FormHide, "Форма-карточка. Скрытие" },
      { EventType.FormShow, "Форма-карточка. Показ" }
    };

    #endregion

    #region Методы

    public override void Apply(IReport report, IDocument document, IContext context)
    {
      // TODO: Пока проверяются только справочники.
      EventType eventType;
      if (document.ComponentType != ComponentType.ReferenceType ||
        !document.Path.StartsWith(DocumentPathPrefix, StringComparison.OrdinalIgnoreCase) ||
        !Enum.TryParse(document.Path.Substring(DocumentPathPrefix.Length), out eventType) ||
        !CheckingEvents.ContainsKey(eventType))
      {
        return;
      }

      var tree = document.GetSyntaxTree();
      var walker = new ParseTreeWalker();
      var listener = new InteractiveWindowsOnEventsListener();
      walker.Walk(listener, tree);

      var referenceType = context.Development.ReferenceTypes
        .FirstOrDefault(r => r.Name == document.ComponentName);
      Severity severity;
      string description;
      if (referenceType?.NumerationMethod == NumerationMethod.StronglyAuto)
      {
        severity = Severity.Error;
        description = string.Format(Resources.InteractiveWindowsOnStronglyNumerationReferenceEvents, CheckingEvents[eventType]);
      }
      else
      {
        severity = Severity.Warning;
        description = string.Format(Resources.InteractiveWindowsOnEvents, CheckingEvents[eventType]);
      }
      foreach (var contextWithError in listener.ContextWithFunctionErrors)
      {
        report.Add(severity, Code, description, document, contextWithError.GetTextPosition());
      }
    }

    #endregion
  }
}
