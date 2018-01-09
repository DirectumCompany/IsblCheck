using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Agent
{
  internal class CodeCheckerCalculationProvider
  {
    #region Поля и свойства

    private IDevelopmentContext context; 

    #endregion

    #region Методы

    public static IEnumerable<IDocument> GetDocuments(IDevelopmentContext context)
    {
      var provider = new CodeCheckerCalculationProvider(context);
      return provider.GetDocuments();
    }

    private IEnumerable<IDocument> GetDocuments()
    {
      return ParseCommonReports()
        .Union(ParseDialogs())
        .Union(ParseDocumentCardTypes())
        .Union(ParseFunctions())
        .Union(ParseIntegratedReports())
        .Union(ParseReferenceTypes())
        .Union(ParseScripts())
        .Union(ParseRouteBlocks())
        .Union(ParseStandardRoutes())
        .Union(ParseWizards());
    }

    private IEnumerable<IDocument> ParseCommonReports()
    {
      foreach (var commonReport in this.context.CommonReports.Where(r => r.State == ComponentState.Active))
      {
        var documentName = string.Format("Отчет {0}. Расчет", commonReport.Title);
        var document = new Document(documentName, commonReport.CalculationText);
        document.ComponentType = ComponentType.CommonReport;
        document.ComponentName = commonReport.Name;
        document.Path = "Calculation";
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
          var documentName = string.Format("Диалог {0}. Действие {1}", dialog.Title, action.Name);
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.Dialog;
          document.ComponentName = dialog.Name;
          document.Path = string.Format("Actions.{0}", action.Name);
          yield return document;
        }
        foreach (var method in dialog.Methods)
        {
          var documentName = string.Format("Диалог {0}. Метод {1}", dialog.Title, method.Name);
          var document = new Document(documentName, method.CalculationText);
          document.ComponentType = ComponentType.Dialog;
          document.ComponentName = dialog.Name;
          document.Path = string.Format("Methods.{0}", method.Name);
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        foreach (var @event in dialog.Events)
        {
          var documentName = string.Format("Диалог {0}. Событие {1}", dialog.Title, @event.EventType);
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.Dialog;
          document.ComponentName = dialog.Name;
          document.Path = string.Format("Events.{0}", @event.EventType);
          yield return document;
        }
        foreach (var requisite in dialog.Requisites)
        {
          foreach (var @event in requisite.Events)
          {
            var documentName = string.Format("Диалог {0}. Реквизит {1}. Событие {2}", dialog.Title, requisite.Name, @event.EventType);
            var document = new Document(documentName, @event.CalculationText);
            document.ComponentType = ComponentType.Dialog;
            document.ComponentName = dialog.Name;
            document.Path = string.Format("Requisites.{0}.Events.{1}", requisite.Name, @event.EventType);
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
          var documentName = string.Format("Тип карточки электронного документа {0}. Действие {1}", documentCardType.Title, action.Name);
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.DocumentCardType;
          document.ComponentName = documentCardType.Name;
          document.Path = string.Format("Actions.{0}", action.Name);
          yield return document;
        }
        foreach (var method in documentCardType.Methods)
        {
          var documentName = string.Format("Тип карточки электронного документа {0}. Метод {1}", documentCardType.Title, method.Name);
          var document = new Document(documentName, method.CalculationText);
          document.ComponentType = ComponentType.DocumentCardType;
          document.ComponentName = documentCardType.Name;
          document.Path = string.Format("Methods.{0}", method.Name);
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        foreach (var @event in documentCardType.Events)
        {
          var documentName = string.Format("Тип карточки электронного документа {0}. Событие {1}", documentCardType.Title, @event.EventType);
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.DocumentCardType;
          document.ComponentName = documentCardType.Name;
          document.Path = string.Format("Events.{0}", @event.EventType);
          yield return document;
        }
        foreach (var requisite in documentCardType.Requisites)
        {
          foreach (var @event in requisite.Events)
          {
            var documentName = string.Format("Тип карточки электронного документа {0}. Реквизит {1}. Событие {2}", documentCardType.Title, requisite.Name, @event.EventType);
            var document = new Document(documentName, @event.CalculationText);
            document.ComponentType = ComponentType.DocumentCardType;
            document.ComponentName = documentCardType.Name;
            document.Path = string.Format("Requisites.{0}.Events.{1}", requisite.Name, @event.EventType);
            yield return document;
          }
        }
      }
    }

    private IEnumerable<IDocument> ParseFunctions()
    {
      foreach (var function in this.context.Functions)
      {
        var documentName = string.Format("Функция {0}. Вычисление", function.Title);
        var document = new Document(documentName, function.CalculationText);
        document.ComponentType = ComponentType.Function;
        document.ComponentName = function.Name;
        document.Path = "Calculation";
        document.ContextVariables.AddRange(function.Arguments.Select(p => p.Name.ToUpper()));
        document.ContextVariables.AddRange(function.Arguments.Select(p => "!" + p.Name.ToUpper()));
        yield return document;
      }
    }

    private IEnumerable<IDocument> ParseIntegratedReports()
    {
      foreach (var integratedReport in this.context.IntegratedReports.Where(r => r.State == ComponentState.Active))
      {
        var documentName = string.Format("Интегрированный отчет {0}. Расчет", integratedReport.Title);
        var document = new Document(documentName, integratedReport.CalculationText);
        document.ComponentType = ComponentType.IntegratedReport;
        document.ComponentName = integratedReport.Name;
        document.Path = "Calculation";
        yield return document;
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
          var documentName = string.Format("Тип справочника {0}. Действие {1}", referenceType.Title, action.Name);
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.ReferenceType;
          document.ComponentName = referenceType.Name;
          document.Path = string.Format("Actions.{0}", action.Name);
          yield return document;
        }
        foreach (var method in referenceType.Methods)
        {
          var documentName = string.Format("Тип справочника {0}. Метод {1}", referenceType.Title, method.Name);
          var document = new Document(documentName, method.CalculationText);
          document.ComponentType = ComponentType.ReferenceType;
          document.ComponentName = referenceType.Name;
          document.Path = string.Format("Methods.{0}", method.Name);
          document.ContextVariables.AddRange(method.Params.Select(p => p.Name.ToUpper()));
          document.ContextVariables.AddRange(method.Params.Select(p => "!" + p.Name.ToUpper()));
          yield return document;
        }
        foreach (var @event in referenceType.Events)
        {
          var documentName = string.Format("Тип справочника {0}. Событие {1}", referenceType.Title, @event.EventType);
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.ReferenceType;
          document.ComponentName = referenceType.Name;
          document.Path = string.Format("Events.{0}", @event.EventType);
          yield return document;
        }
        foreach (var requisite in referenceType.Requisites)
        {
          foreach (var @event in requisite.Events)
          {
            var documentName = string.Format("Тип справочника {0}. Реквизит {1}. Событие {2}", referenceType.Title, requisite.Name, @event.EventType);
            var document = new Document(documentName, @event.CalculationText);
            document.ComponentType = ComponentType.ReferenceType;
            document.ComponentName = referenceType.Name;
            document.Path = string.Format("Requisites.{0}.Events.{1}", requisite.Name, @event.EventType);
            yield return document;
          }
        }
      }
    }

    private IEnumerable<IDocument> ParseScripts()
    {
      foreach (var script in this.context.Scripts.Where(s => s.State == ComponentState.Active))
      {
        var documentName = string.Format("Сценарий {0}. Вычисление", script.Title);
        var document = new Document(documentName, script.CalculationText);
        document.ComponentType = ComponentType.Script;
        document.ComponentName = script.Name;
        document.Path = "Calculation";
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
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.RouteBlock;
          document.ComponentName = block.Name;
          document.Path = $"Events.{@event.Name}";
          yield return document;
        }

        foreach (var action in block.WorkflowBlock.Actions)
        {
          var documentName = $"Блок {block.Name}. Действие \"{action.Name}\"";
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.RouteBlock;
          document.ComponentName = block.Name;
          document.Path = $"Actions.{action.Name}";
          yield return document;
        }

        foreach (var isblProp in block.WorkflowBlock.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Свойство \"{isblProp.Title}\"";
          var document = new Document(documentName, isblProp.CalculationText);
          document.ComponentType = ComponentType.RouteBlock;
          document.ComponentName = block.Name;
          document.Path = $"Properties.{isblProp.Name}";
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
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.StandardRoute;
          document.ComponentName = route.Name;
          document.Path = $"Events.{@event.Name}";
          yield return document;
        }

        #endregion

        #region Действия ТМ

        foreach (var action in route.WorkflowDescription.Actions)
        {
          var documentName = $"ТМ \"{route.Title}\". Действие \"{action.Name}\"";
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.StandardRoute;
          document.ComponentName = route.Name;
          document.Path = $"Actions.{action.Name}";
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
            var document = new Document(documentName, @event.CalculationText);
            document.ComponentType = ComponentType.StandardRoute;
            document.ComponentName = route.Name;
            document.Path = $"Blocks.{block.Id}.Events.{@event.Name}";
            yield return document;
          }

          foreach (var action in block.Actions)
          {
            var documentName = $"{fullBlockDescription}. Действие \"{action.Name}\"";
            var document = new Document(documentName, action.CalculationText);
            document.ComponentType = ComponentType.StandardRoute;
            document.ComponentName = route.Name;
            document.Path = $"Blocks.{block.Id}.Actions.{action.Name}";
            yield return document;
          }

          foreach (var isblProp in block.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
          {
            var documentName = $"{fullBlockDescription}. Свойство \"{isblProp.Title}\"";
            var document = new Document(documentName, isblProp.CalculationText);
            document.ComponentType = ComponentType.StandardRoute;
            document.ComponentName = route.Name;
            document.Path = $"Blocks.{block.Id}.Properties.{isblProp.Name}";
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
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.Wizard;
          document.ComponentName = wizard.Name;
          document.Path = $"Events.{@event.Name}";
          yield return document;
        }

        #endregion

        #region Этапы мастера

        for (int stepIndex = 0; stepIndex < wizard.Steps.Count; stepIndex++)
        {
          var step = wizard.Steps[stepIndex];

          var stepDescription = $"Этап \"{step.Name}\"";
          var fullStepDescription = $"Мастер \"{wizard.Title}\". {stepDescription}";

          foreach (var @event in step.Events)
          {
            var documentName = $"{fullStepDescription}. Событие \"{@event.Title}\"";
            var document = new Document(documentName, @event.CalculationText);
            document.ComponentType = ComponentType.Wizard;
            document.ComponentName = wizard.Name;
            document.Path = $"Steps.{stepIndex}.Events.{@event.Name}";
            yield return document;
          }

          foreach (var action in step.Actions)
          {
            var documentName = $"{fullStepDescription}. Действие \"{action.Title}\"";
            var document = new Document(documentName, action.CalculationText);
            document.ComponentType = ComponentType.Wizard;
            document.ComponentName = wizard.Name;
            document.Path = $"Steps.{stepIndex}.Actions.{action.Name}";
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