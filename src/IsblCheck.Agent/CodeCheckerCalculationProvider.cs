using System.Collections.Generic;
using System.Linq;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Agent
{
  internal class CodeCheckerCalculationProvider
  {
    #region Поля и свойства

    private readonly IDevelopmentContext context;

    #endregion

    #region Методы

    public static IEnumerable<IDocument> GetDocuments(IDevelopmentContext context)
    {
      var provider = new CodeCheckerCalculationProvider(context);
      return provider.GetDocuments();
    }

    private IEnumerable<IDocument> GetDocuments()
    {
      return this.ParseCommonReports()
        .Union(this.ParseDialogs())
        .Union(this.ParseDocumentCardTypes())
        .Union(this.ParseFunctions())
        .Union(this.ParseIntegratedReports())
        .Union(this.ParseManagedFolders())
        .Union(this.ParseReferenceTypes())
        .Union(this.ParseScripts())
        .Union(this.ParseRouteBlocks())
        .Union(this.ParseStandardRoutes())
        .Union(this.ParseWizards());
    }

    private IEnumerable<IDocument> ParseCommonReports()
    {
      foreach (var commonReport in this.context.CommonReports.Where(r => r.State == ComponentState.Active))
      {
        var documentName = $"Отчет {commonReport.Title}. Расчет";
        var document = new Document(documentName, commonReport.CalculationText)
        {
          ComponentType = ComponentType.CommonReport,
          ComponentName = commonReport.Name,
          Path = "Calculation"
        };
        yield return document;
      }
    }

    private IEnumerable<IDocument> ParseDialogs()
    {
      foreach (var dialog in this.context.Dialogs)
      {
        foreach (var action in dialog.Actions)
        {
          if (action.ExecutionHandler != null)
            continue;
          var documentName = $"Диалог {dialog.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.Dialog,
            ComponentName = dialog.Name,
            Path = $"Actions.{action.Name}"
          };
          yield return document;
        }
        foreach (var method in dialog.Methods)
        {
          var documentName = $"Диалог {dialog.Title}. Метод {method.Name}";
          var document = new Document(documentName, method.CalculationText)
          {
            ComponentType = ComponentType.Dialog,
            ComponentName = dialog.Name,
            Path = $"Methods.{method.Name}"
          };
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        foreach (var @event in dialog.Events)
        {
          var documentName = $"Диалог {dialog.Title}. Событие {@event.EventType}";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.Dialog,
            ComponentName = dialog.Name,
            Path = $"Events.{@event.EventType}"
          };
          yield return document;
        }
        foreach (var requisite in dialog.Requisites)
        {
          foreach (var @event in requisite.Events)
          {
            var documentName = $"Диалог {dialog.Title}. Реквизит {requisite.Name}. Событие {@event.EventType}";
            var document = new Document(documentName, @event.CalculationText)
            {
              ComponentType = ComponentType.Dialog,
              ComponentName = dialog.Name,
              Path = $"Requisites.{requisite.Name}.Events.{@event.EventType}"
            };
            yield return document;
          }
        }
      }
    }

    private IEnumerable<IDocument> ParseDocumentCardTypes()
    {
      foreach (var documentCardType in this.context.DocumentCardTypes.Where(d => d.State == ComponentState.Active))
      {
        foreach (var action in documentCardType.Actions)
        {
          if (action.ExecutionHandler != null)
            continue;
          var documentName = $"Тип карточки электронного документа {documentCardType.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.DocumentCardType,
            ComponentName = documentCardType.Name,
            Path = $"Actions.{action.Name}"
          };
          yield return document;
        }
        foreach (var method in documentCardType.Methods)
        {
          var documentName = $"Тип карточки электронного документа {documentCardType.Title}. Метод {method.Name}";
          var document = new Document(documentName, method.CalculationText)
          {
            ComponentType = ComponentType.DocumentCardType,
            ComponentName = documentCardType.Name,
            Path = $"Methods.{method.Name}"
          };
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        foreach (var @event in documentCardType.Events)
        {
          var documentName = $"Тип карточки электронного документа {documentCardType.Title}. Событие {@event.EventType}";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.DocumentCardType,
            ComponentName = documentCardType.Name,
            Path = $"Events.{@event.EventType}"
          };
          yield return document;
        }
        foreach (var requisite in documentCardType.Requisites)
        {
          foreach (var @event in requisite.Events)
          {
            var documentName = $"Тип карточки электронного документа {documentCardType.Title}. Реквизит {requisite.Name}. Событие {@event.EventType}";
            var document = new Document(documentName, @event.CalculationText)
            {
              ComponentType = ComponentType.DocumentCardType,
              ComponentName = documentCardType.Name,
              Path = $"Requisites.{requisite.Name}.Events.{@event.EventType}"
            };
            yield return document;
          }
        }
      }
    }

    private IEnumerable<IDocument> ParseFunctions()
    {
      foreach (var function in this.context.Functions)
      {
        var documentName = $"Функция {function.Title}. Вычисление";
        var document = new Document(documentName, function.CalculationText)
        {
          ComponentType = ComponentType.Function,
          ComponentName = function.Name,
          Path = "Calculation"
        };
        document.ContextVariables.AddRange(function.Arguments.Select(p => p.Name.ToUpper()));
        document.ContextVariables.AddRange(function.Arguments.Select(p => "!" + p.Name.ToUpper()));
        yield return document;
      }
    }

    private IEnumerable<IDocument> ParseIntegratedReports()
    {
      foreach (var integratedReport in this.context.IntegratedReports.Where(r => r.State == ComponentState.Active))
      {
        var documentName = $"Интегрированный отчет {integratedReport.Title}. Расчет";
        var document = new Document(documentName, integratedReport.CalculationText)
        {
          ComponentType = ComponentType.IntegratedReport,
          ComponentName = integratedReport.Name,
          Path = "Calculation"
        };
        yield return document;
      }
    }

    private IEnumerable<IDocument> ParseManagedFolders()
    {
      foreach (var managedFolder in this.context.ManagedFolders.Where(r => r.State == ComponentState.Active))
      {
        foreach (var action in managedFolder.Actions)
        {
          if (action.ExecutionHandler != null)
            continue;
          var documentName = $"Управляемая папка {managedFolder.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.ManagedFolder,
            ComponentName = managedFolder.Name,
            Path = $"Actions.{action.Name}"
          };
          yield return document;
        }
        foreach (var method in managedFolder.Methods)
        {
          var documentName = $"Управляемая папка {managedFolder.Title}. Метод {method.Name}";
          var document = new Document(documentName, method.CalculationText)
          {
            ComponentType = ComponentType.ManagedFolder,
            ComponentName = managedFolder.Name,
            Path = $"Methods.{method.Name}"
          };
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        if (managedFolder.SearchDescription != null)
        {
          var documentName = $"Управляемая папка {managedFolder.Title}. Событие \"До поиска\"";
          var document = new Document(documentName, managedFolder.SearchDescription.BeforeSearchEventText)
          {
            ComponentType = ComponentType.ManagedFolder,
            ComponentName = managedFolder.Name,
            Path = $"Events.BeforeSearch"
          };
          yield return document;
        }
      }
    }

    private IEnumerable<IDocument> ParseReferenceTypes()
    {
      foreach (var referenceType in this.context.ReferenceTypes.Where(r => r.State == ComponentState.Active))
      {
        foreach (var action in referenceType.Actions)
        {
          if (action.ExecutionHandler != null)
            continue;
          var documentName = $"Тип справочника {referenceType.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.ReferenceType,
            ComponentName = referenceType.Name,
            Path = $"Actions.{action.Name}"
          };
          yield return document;
        }
        foreach (var method in referenceType.Methods)
        {
          var documentName = $"Тип справочника {referenceType.Title}. Метод {method.Name}";
          var document = new Document(documentName, method.CalculationText)
          {
            ComponentType = ComponentType.ReferenceType,
            ComponentName = referenceType.Name,
            Path = $"Methods.{method.Name}"
          };
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        foreach (var @event in referenceType.Events)
        {
          var documentName = $"Тип справочника {referenceType.Title}. Событие {@event.EventType}";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.ReferenceType,
            ComponentName = referenceType.Name,
            Path = $"Events.{@event.EventType}"
          };
          yield return document;
        }
        foreach (var requisite in referenceType.Requisites)
        {
          foreach (var @event in requisite.Events)
          {
            var documentName = $"Тип справочника {referenceType.Title}. Реквизит {requisite.Name}. Событие {@event.EventType}";
            var document = new Document(documentName, @event.CalculationText)
            {
              ComponentType = ComponentType.ReferenceType,
              ComponentName = referenceType.Name,
              Path = $"Requisites.{requisite.Name}.Events.{@event.EventType}"
            };
            yield return document;
          }
        }
      }
    }

    private IEnumerable<IDocument> ParseScripts()
    {
      foreach (var script in this.context.Scripts.Where(s => s.State == ComponentState.Active))
      {
        var documentName = $"Сценарий {script.Title}. Вычисление";
        var document = new Document(documentName, script.CalculationText)
        {
          ComponentType = ComponentType.Script,
          ComponentName = script.Name,
          Path = "Calculation"
        };
        yield return document;
      }
    }

    private IEnumerable<IDocument> ParseRouteBlocks()
    {
      foreach (var block in this.context.RouteBlocks
        .Where(b => b.State == ComponentState.Active)
        .Where(b => b.WorkflowBlock != null))
      {
        foreach (var @event in block.WorkflowBlock.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.RouteBlock,
            ComponentName = block.Name,
            Path = $"Events.{@event.Name}"
          };
          yield return document;
        }

        foreach (var action in block.WorkflowBlock.Actions)
        {
          var documentName = $"Блок {block.Name}. Действие \"{action.Name}\"";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.RouteBlock,
            ComponentName = block.Name,
            Path = $"Actions.{action.Name}"
          };
          yield return document;
        }

        foreach (var isblProp in block.WorkflowBlock.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Свойство \"{isblProp.Title}\"";
          var document = new Document(documentName, isblProp.CalculationText)
          {
            ComponentType = ComponentType.RouteBlock,
            ComponentName = block.Name,
            Path = $"Properties.{isblProp.Name}"
          };
          yield return document;
        }
      }
    }

    private IEnumerable<IDocument> ParseStandardRoutes()
    {
      foreach (var route in this.context.StandardRoutes
        .Where(r => r.State == ComponentState.Active)
        .Where(r => r.WorkflowDescription != null))
      {
        #region События ТМ

        foreach (var @event in route.WorkflowDescription.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"ТМ \"{route.Title}\". Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.StandardRoute,
            ComponentName = route.Name,
            Path = $"Events.{@event.Name}"
          };
          yield return document;
        }

        #endregion

        #region Действия ТМ

        foreach (var action in route.WorkflowDescription.Actions)
        {
          var documentName = $"ТМ \"{route.Title}\". Действие \"{action.Name}\"";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.StandardRoute,
            ComponentName = route.Name,
            Path = $"Actions.{action.Name}"
          };
          yield return document;
        }

        #endregion

        #region Блоки маршрута

        foreach (var block in route.WorkflowDescription.Blocks)
        {
          var blockDescription = $"Блок \"{block.Name}\" (номер {block.Id})";
          var fullBlockDescription = $"ТМ \"{route.Title}\". {blockDescription}";

          foreach (var @event in block.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
          {
            var documentName = $"{fullBlockDescription}. Событие \"{@event.Title}\"";
            var document = new Document(documentName, @event.CalculationText)
            {
              ComponentType = ComponentType.StandardRoute,
              ComponentName = route.Name,
              Path = $"Blocks.{block.Id}.Events.{@event.Name}"
            };
            yield return document;
          }

          foreach (var action in block.Actions)
          {
            var documentName = $"{fullBlockDescription}. Действие \"{action.Name}\"";
            var document = new Document(documentName, action.CalculationText)
            {
              ComponentType = ComponentType.StandardRoute,
              ComponentName = route.Name,
              Path = $"Blocks.{block.Id}.Actions.{action.Name}"
            };
            yield return document;
          }

          foreach (var isblProp in block.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
          {
            var documentName = $"{fullBlockDescription}. Свойство \"{isblProp.Title}\"";
            var document = new Document(documentName, isblProp.CalculationText)
            {
              ComponentType = ComponentType.StandardRoute,
              ComponentName = route.Name,
              Path = $"Blocks.{block.Id}.Properties.{isblProp.Name}"
            };
            yield return document;
          }
        }

        #endregion
      }
    }

    private IEnumerable<IDocument> ParseWizards()
    {
      foreach (var wizard in this.context.Wizards.Where(r => r.State == ComponentState.Active))
      {
        #region События ТМ

        foreach (var @event in wizard.Events)
        {
          var documentName = $"Мастер \"{wizard.Title}\". Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.Wizard,
            ComponentName = wizard.Name,
            Path = $"Events.{@event.Name}"
          };
          yield return document;
        }

        #endregion

        #region Этапы мастера

        for (var stepIndex = 0; stepIndex < wizard.Steps.Count; stepIndex++)
        {
          var step = wizard.Steps[stepIndex];

          var stepDescription = $"Этап \"{step.Name}\"";
          var fullStepDescription = $"Мастер \"{wizard.Title}\". {stepDescription}";

          foreach (var @event in step.Events)
          {
            var documentName = $"{fullStepDescription}. Событие \"{@event.Title}\"";
            var document = new Document(documentName, @event.CalculationText)
            {
              ComponentType = ComponentType.Wizard,
              ComponentName = wizard.Name,
              Path = $"Steps.{stepIndex}.Events.{@event.Name}"
            };
            yield return document;
          }

          foreach (var action in step.Actions)
          {
            var documentName = $"{fullStepDescription}. Действие \"{action.Title}\"";
            var document = new Document(documentName, action.CalculationText)
            {
              ComponentType = ComponentType.Wizard,
              ComponentName = wizard.Name,
              Path = $"Steps.{stepIndex}.Actions.{action.Name}"
            };
            yield return document;
          }
        }

        #endregion
      }
    }

    #endregion

    #region Конструкторы

    private CodeCheckerCalculationProvider(IDevelopmentContext context)
    {
      this.context = context;
    }

    #endregion
  }
}