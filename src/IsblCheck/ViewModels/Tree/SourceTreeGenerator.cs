using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IsblCheck.ViewModels.Tree
{
  /// <summary>
  /// Генератор дерева исходных данных.
  /// </summary>
  public class SourceTreeGenerator
  {
    #region Поля и свойства

    /// <summary>
    /// Иконка диалогов.
    /// </summary>
    private static readonly ImageSource DialogsIcon;

    /// <summary>
    /// Иконка типов каточек электронных документов.
    /// </summary>
    private static readonly ImageSource DocumentCardTypesIcon;

    /// <summary>
    /// Иконка функций.
    /// </summary>
    private static readonly ImageSource FunctionsIcon;

    /// <summary>
    /// Иконка типов справочников.
    /// </summary>
    private static readonly ImageSource ReferenceTypesIcon;

    /// <summary>
    /// Иконка отчетов.
    /// </summary>
    private static readonly ImageSource ReportsIcon;

    /// <summary>
    /// Иконка блоков типовых маршрутов.
    /// </summary>
    private static readonly ImageSource RouteBlocksIcon;

    /// <summary>
    /// Иконка сценариев.
    /// </summary>
    private static readonly ImageSource ScriptsIcon;

    /// <summary>
    /// Иконка типовых маршрутов.
    /// </summary>
    private static readonly ImageSource StandardRoutesIcon;

    /// <summary>
    /// Иконка мастеров действий.
    /// </summary>
    private static readonly ImageSource WizardsIcon;

    #endregion

    #region Методы

    /// <summary>
    /// Сгенерировать дерево исходных данных.
    /// </summary>
    /// <param name="rootTitle">Наименование корневого узла.</param>
    /// <param name="provider">Провайдер исходных данных.</param>
    /// <returns>Список контейнеров типов компонент.</returns>
    public IEnumerable<ContainerTreeNode> Generate(string rootTitle, IDevelopmentContext context)
    {
      var componentTypes = new List<ContainerTreeNode>();

      var commonReports = context.CommonReports.Where(r => r.State == ComponentState.Active);
      var commonReportsTreeNode = GenerateCommonReportsNode(commonReports);
      componentTypes.Add(commonReportsTreeNode);

      var dialogs = context.Dialogs;
      var dialogsTreeNode = GenerateDialogsNode(dialogs);
      componentTypes.Add(dialogsTreeNode);

      var documentCardTypes = context.DocumentCardTypes.Where(d => d.State == ComponentState.Active);
      var documentCardTypesTreeNode = GenerateDocumentCardTypesNode(documentCardTypes);
      componentTypes.Add(documentCardTypesTreeNode);

      var functions = context.Functions;
      var functionsTreeNode = GenerateFunctionsNode(functions);
      componentTypes.Add(functionsTreeNode);

      var integratedReports = context.IntegratedReports.Where(r => r.State == ComponentState.Active);
      var integratedReportsTreeNode = GenerateIntegratedReportsNode(integratedReports);
      componentTypes.Add(integratedReportsTreeNode);

      var referenceTypes = context.ReferenceTypes.Where(r => r.State == ComponentState.Active);
      var referenceTypesTreeNode = GenerateReferenceTypesNode(referenceTypes);
      componentTypes.Add(referenceTypesTreeNode);

      var scripts = context.Scripts.Where(s => s.State == ComponentState.Active);
      var scriptsTreeNode = GenerateScriptsNode(scripts);
      componentTypes.Add(scriptsTreeNode);

      var routeBlocks = context.RouteBlocks.Where(b => b.State == ComponentState.Active);
      var routeBlocksTreeNode = GenerateRouteBlocksNode(routeBlocks);
      componentTypes.Add(routeBlocksTreeNode);

      var standardRoutes = context.StandardRoutes.Where(r => r.State == ComponentState.Active);
      var standardRoutesTreeNode = GenerateStandardRoutesNode(standardRoutes);
      componentTypes.Add(standardRoutesTreeNode);

      var wizards = context.Wizards.Where(r => r.State == ComponentState.Active);
      var wizardsTreeNode = GenerateWizardsTreeNode(wizards);
      componentTypes.Add(wizardsTreeNode);

      return componentTypes;
    }

    /// <summary>
    /// Сгенерировать узел отчетов.
    /// </summary>
    /// <param name="commonReports">Коллекция отчетов.</param>
    /// <returns>Узел отчетов.</returns>
    private ContainerTreeNode GenerateCommonReportsNode(IEnumerable<CommonReport> commonReports)
    {
      var commonReportNodes = new List<ContainerTreeNode>();
      foreach (var commonReport in commonReports)
      {
        var commonReportNode = new ContainerTreeNode();
        commonReportNode.Title = commonReport.Title;

        var documentName = string.Format("Отчет {0}. Расчет", commonReport.Title);
        var document = new Document(documentName, commonReport.CalculationText);
        document.ComponentType = ComponentType.CommonReport;
        document.ComponentName = commonReport.Name;
        document.Path = "Calculation";

        var documentNode = new DocumentTreeNode(document);
        documentNode.Title = "Расчет";
        commonReportNode.Items.Add(documentNode);

        // TODO: Пока не генерируем шаблоны, так как там могут быть
        // и шаблоны в виде документа ворд, ексель, кристал репортс.

        commonReportNodes.Add(commonReportNode);
      }

      var rootNode = new ContainerTreeNode(commonReportNodes);
      rootNode.TitleLocalizationKey = "COMMON_REPORTS";
      rootNode.IconSource = ReportsIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел диалогов.
    /// </summary>
    /// <param name="dialogs">Коллекция диалогов.</param>
    /// <returns>Узел диалогов.</returns>
    private ContainerTreeNode GenerateDialogsNode(IEnumerable<Dialog> dialogs)
    {
      var dialogNodes = new List<ContainerTreeNode>();
      foreach (var dialog in dialogs)
      {
        var documentNodes = new List<DocumentTreeNode>();

        foreach (var action in dialog.Actions)
        {
          if (action.ExecutionHandler != null)
          {
            // Не добавлять в дерево действия, которые привязаны к методам.
            continue;
          }

          var documentName = string.Format("Диалог {0}. Действие {1}", dialog.Title, action.Name);
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.Dialog;
          document.ComponentName = dialog.Name;
          document.Path = string.Format("Actions.{0}", action.Name);

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Действие {0}", action.Name);
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Метод {0}", method.Name);
          documentNodes.Add(documentNode);
        }

        foreach (var @event in dialog.Events)
        {
          var documentName = string.Format("Диалог {0}. Событие {1}", dialog.Title, @event.EventType);
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.Dialog;
          document.ComponentName = dialog.Name;
          document.Path = string.Format("Events.{0}", @event.EventType);

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Событие {0}", @event.EventType);
          documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = string.Format("Реквизит {0}. Событие {1}", requisite.Name, @event.EventType);
            documentNodes.Add(documentNode);
          }
        }

        var dialogNode = new ContainerTreeNode(documentNodes);
        dialogNode.Title = dialog.Title;
        dialogNodes.Add(dialogNode);
      }

      var rootNode = new ContainerTreeNode(dialogNodes);
      rootNode.TitleLocalizationKey = "DIALOGS";
      rootNode.IconSource = DialogsIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел типов карточек электронных документов.
    /// </summary>
    /// <param name="documentCardTypes">Коллекция типов карточек электронных документов.</param>
    /// <returns>Узел типов карточек электронных документов.</returns>
    private ContainerTreeNode GenerateDocumentCardTypesNode(IEnumerable<DocumentCardType> documentCardTypes)
    {
      var documentCardTypeNodes = new List<ContainerTreeNode>();
      foreach (var documentCardType in documentCardTypes)
      {
        var documentNodes = new List<DocumentTreeNode>();

        foreach (var action in documentCardType.Actions)
        {
          if (action.ExecutionHandler != null)
          {
            // Не добавлять в дерево действия, которые привязаны к методам.
            continue;
          }

          var documentName = string.Format("Тип карточки электронного документа {0}. Действие {1}", documentCardType.Title, action.Name);
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.DocumentCardType;
          document.ComponentName = documentCardType.Name;
          document.Path = string.Format("Actions.{0}", action.Name);

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Действие {0}", action.Name);
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Метод {0}", method.Name);
          documentNodes.Add(documentNode);
        }

        foreach (var @event in documentCardType.Events)
        {
          var documentName = string.Format("Тип карточки электронного документа {0}. Событие {1}", documentCardType.Title, @event.EventType);
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.DocumentCardType;
          document.ComponentName = documentCardType.Name;
          document.Path = string.Format("Events.{0}", @event.EventType);

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Событие {0}", @event.EventType);
          documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = string.Format("Реквизит {0}. Событие {1}", requisite.Name, @event.EventType);
            documentNodes.Add(documentNode);
          }
        }

        var documentCardTypeNode = new ContainerTreeNode(documentNodes);
        documentCardTypeNode.Title = documentCardType.Title;
        documentCardTypeNodes.Add(documentCardTypeNode);
      }

      var rootNode = new ContainerTreeNode(documentCardTypeNodes);
      rootNode.TitleLocalizationKey = "DOCUMENT_CARD_TYPES";
      rootNode.IconSource = DocumentCardTypesIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел функций.
    /// </summary>
    /// <param name="functions">Коллекция функций.</param>
    /// <returns>Узел функций.</returns>
    private ContainerTreeNode GenerateFunctionsNode(IEnumerable<Function> functions)
    {
      var functionNodes = new List<ContainerTreeNode>();
      foreach (var function in functions)
      {
        var functionNode = new ContainerTreeNode();
        functionNode.Title = function.Title;

        var documentName = string.Format("Функция {0}. Вычисление", function.Title);
        var document = new Document(documentName, function.CalculationText);
        document.ComponentType = ComponentType.Function;
        document.ComponentName = function.Name;
        document.Path = "Calculation";
        document.ContextVariables.AddRange(function.Arguments.Select(a => a.Name.ToUpper()));
        document.ContextVariables.AddRange(function.Arguments.Select(a => "!" + a.Name.ToUpper()));

        var documentNode = new DocumentTreeNode(document);
        documentNode.Title = "Вычисление";
        functionNode.Items.Add(documentNode);

        functionNodes.Add(functionNode);
      }

      var rootNode = new ContainerTreeNode(functionNodes);
      rootNode.TitleLocalizationKey = "FUNCTIONS";
      rootNode.IconSource = FunctionsIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел интегрированных отчетов.
    /// </summary>
    /// <param name="integratedReports">Коллекция интегрированных отчетов.</param>
    /// <returns>Узел интегрированных отчетов.</returns>
    private ContainerTreeNode GenerateIntegratedReportsNode(IEnumerable<IntegratedReport> integratedReports)
    {
      var integratedReportNodes = new List<ContainerTreeNode>();
      foreach (var integratedReport in integratedReports)
      {
        var integratedReportNode = new ContainerTreeNode();
        integratedReportNode.Title = integratedReport.Title;

        var documentName = string.Format("Интегрированный отчет {0}. Расчет", integratedReport.Title);
        var document = new Document(documentName, integratedReport.CalculationText);
        document.ComponentType = ComponentType.IntegratedReport;
        document.ComponentName = integratedReport.Name;
        document.Path = "Calculation";

        var documentNode = new DocumentTreeNode(document);
        documentNode.Title = "Расчет";
        integratedReportNode.Items.Add(documentNode);

        // TODO: Пока не генерируем шаблоны, так как там могут быть
        // и шаблоны в виде документа ворд, ексель, кристал репортс.

        integratedReportNodes.Add(integratedReportNode);
      }

      var rootNode = new ContainerTreeNode(integratedReportNodes);
      rootNode.TitleLocalizationKey = "INTEGRATED_REPORTS";
      rootNode.IconSource = ReportsIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел типов справочников.
    /// </summary>
    /// <param name="referenceTypes">Коллекция типов справочников.</param>
    /// <returns>Узел типов справочников.</returns>
    private ContainerTreeNode GenerateReferenceTypesNode(IEnumerable<ReferenceType> referenceTypes)
    {
      var referenceTypeNodes = new List<ContainerTreeNode>();
      foreach (var referenceType in referenceTypes)
      {
        var documentNodes = new List<DocumentTreeNode>();

        foreach (var action in referenceType.Actions)
        {
          if (action.ExecutionHandler != null)
          {
            // Не добавлять в дерево действия, которые привязаны к методам.
            continue;
          }

          var documentName = string.Format("Тип справочника {0}. Действие {1}", referenceType.Title, action.Name);
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.ReferenceType;
          document.ComponentName = referenceType.Name;
          document.Path = string.Format("Actions.{0}", action.Name);

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Действие {0}", action.Name);
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Метод {0}", method.Name);
          documentNodes.Add(documentNode);
        }

        foreach (var @event in referenceType.Events)
        {
          var documentName = string.Format("Тип справочника {0}. Событие {1}", referenceType.Title, @event.EventType);
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.ReferenceType;
          document.ComponentName = referenceType.Name;
          document.Path = string.Format("Events.{0}", @event.EventType);

          var documentNode = new DocumentTreeNode(document);
          documentNode.Title = string.Format("Событие {0}", @event.EventType);
          documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = string.Format("Реквизит {0}. Событие {1}", requisite.Name, @event.EventType);
            documentNodes.Add(documentNode);
          }
        }

        var referenceTypeNode = new ContainerTreeNode(documentNodes);
        referenceTypeNode.Title = referenceType.Title;
        referenceTypeNodes.Add(referenceTypeNode);
      }

      var rootNode = new ContainerTreeNode(referenceTypeNodes);
      rootNode.TitleLocalizationKey = "REFERENCE_TYPES";
      rootNode.IconSource = ReferenceTypesIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел сценариев.
    /// </summary>
    /// <param name="scripts">Коллекция сценариев.</param>
    /// <returns>Узел сценариев.</returns>
    private ContainerTreeNode GenerateScriptsNode(IEnumerable<Script> scripts)
    {
      var scriptNodes = new List<ContainerTreeNode>();
      foreach (var script in scripts)
      {
        var scriptNode = new ContainerTreeNode();
        scriptNode.Title = script.Title;

        var documentName = string.Format("Сценарий {0}. Вычисление", script.Title);
        var document = new Document(documentName, script.CalculationText);
        document.ComponentType = ComponentType.Script;
        document.ComponentName = script.Name;
        document.Path = "Calculation";

        var documentNode = new DocumentTreeNode(document);
        documentNode.Title = "Вычисление";
        scriptNode.Items.Add(documentNode);

        scriptNodes.Add(scriptNode);
      }

      var rootNode = new ContainerTreeNode(scriptNodes);
      rootNode.TitleLocalizationKey = "SCRIPTS";
      rootNode.IconSource = ScriptsIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел блоков ТМ.
    /// </summary>
    /// <param name="routeBlocks">Коллекция блоков.</param>
    /// <returns>Узел блоков типовых маршрутов.</returns>
    private ContainerTreeNode GenerateRouteBlocksNode(IEnumerable<RouteBlock> routeBlocks)
    {
      var routeBlockNodes = new List<ContainerTreeNode>();
      foreach (var block in routeBlocks.Where(b => b.WorkflowBlock != null))
      {
        var documentNodes = new List<DocumentTreeNode>();

        #region События блока
        foreach (var @event in block.WorkflowBlock.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.RouteBlock;
          document.ComponentName = block.Name;
          document.Path = $"Events.{@event.Name}";

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Событие \"{@event.Title}\"" });
        }
        #endregion

        #region Действия блока
        foreach (var action in block.WorkflowBlock.Actions)
        {
          var documentName = $"Блок {block.Name}. Действие \"{action.Name}\"";
          var document = new Document(documentName, action.CalculationText);
          document.ComponentType = ComponentType.RouteBlock;
          document.ComponentName = block.Name;
          document.Path = $"Actions.{action.Name}";

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Действие \"{action.Name}\"" });
        }
        #endregion

        #region Вычисления свойств блока
        foreach (var isblProp in block.WorkflowBlock.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Свойство \"{isblProp.Title}\"";
          var document = new Document(documentName, isblProp.CalculationText);
          document.ComponentType = ComponentType.RouteBlock;
          document.ComponentName = block.Name;
          document.Path = $"Properties.{isblProp.Name}";

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Свойство \"{isblProp.Title}\"" });
        } 
        #endregion

        routeBlockNodes.Add(new ContainerTreeNode(documentNodes) { Title = block.Title });
      }

      var rootNode = new ContainerTreeNode(routeBlockNodes);
      rootNode.TitleLocalizationKey = "ROUTE_BLOCKS";
      rootNode.IconSource = RouteBlocksIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел типовых маршрутов.
    /// </summary>
    /// <param name="standardRoutes">Коллекция типовых маршрутов.</param>
    /// <returns>Узел типовых маршрутов.</returns>
    private ContainerTreeNode GenerateStandardRoutesNode(IEnumerable<StandardRoute> standardRoutes)
    {
      var standardRouteNodes = new List<ContainerTreeNode>();
      foreach (var route in standardRoutes.Where(r => r.WorkflowDescription != null))
      {
        var documentNodes = new List<DocumentTreeNode>();

        #region События ТМ
        
        foreach (var @event in route.WorkflowDescription.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"ТМ \"{route.Title}\". Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.StandardRoute;
          document.ComponentName = route.Name;
          document.Path = $"Events.{@event.Name}";

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Событие \"{@event.Title}\"" });
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

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Действие \"{action.Name}\"" });
        }

        #endregion

        #region Блоки маршрута

        foreach (var block in route.WorkflowDescription.Blocks)
        {
          var blockDescription = $"Блок \"{block.Name}\" (номер {block.Id})";
          var fullBlockDescription = $"ТМ \"{route.Title}\". {blockDescription}";

          #region События блока
          foreach (var @event in block.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
          {
            var documentName = $"{fullBlockDescription}. Событие \"{@event.Title}\"";
            var document = new Document(documentName, @event.CalculationText);
            document.ComponentType = ComponentType.StandardRoute;
            document.ComponentName = route.Name;
            document.Path = $"Blocks.{block.Id}.Events.{@event.Name}";

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = $"{blockDescription}. Событие \"{@event.Title}\"";
            documentNodes.Add(documentNode);
          } 
          #endregion

          #region Действия блока
          foreach (var action in block.Actions)
          {
            var documentName = $"{fullBlockDescription}. Действие \"{action.Name}\"";
            var document = new Document(documentName, action.CalculationText);
            document.ComponentType = ComponentType.StandardRoute;
            document.ComponentName = route.Name;
            document.Path = $"Blocks.{block.Id}.Actions.{action.Name}";

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = $"{blockDescription}. Действие \"{action.Name}\"";
            documentNodes.Add(documentNode);
          } 
          #endregion

          #region Вычисления свойств блока
          foreach (var isblProp in block.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
          {
            var documentName = $"{fullBlockDescription}. Свойство \"{isblProp.Title}\"";
            var document = new Document(documentName, isblProp.CalculationText);
            document.ComponentType = ComponentType.StandardRoute;
            document.ComponentName = route.Name;
            document.Path = $"Blocks.{block.Id}.Properties.{isblProp.Name}";

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = $"{blockDescription}. Свойство \"{isblProp.Title}\"";
            documentNodes.Add(documentNode);
          } 
          #endregion
        }

        standardRouteNodes.Add(new ContainerTreeNode(documentNodes) { Title = route.Title });

        #endregion
      }

      var rootNode = new ContainerTreeNode(standardRouteNodes);
      rootNode.TitleLocalizationKey = "STANDARD_ROUTES";
      rootNode.IconSource = StandardRoutesIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел мастеров действий.
    /// </summary>
    /// <param name="wizards">Коллекция мастеров действий.</param>
    /// <returns>Узел мастеров действий.</returns>
    private ContainerTreeNode GenerateWizardsTreeNode(IEnumerable<Wizard> wizards)
    {
      var wizardNodes = new List<ContainerTreeNode>();
      foreach (var wizard in wizards)
      {
        var documentNodes = new List<DocumentTreeNode>();

        #region События ТМ

        foreach (var @event in wizard.Events)
        {
          var documentName = $"Мастер \"{wizard.Title}\". Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText);
          document.ComponentType = ComponentType.Wizard;
          document.ComponentName = wizard.Name;
          document.Path = $"Events.{@event.Name}";

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Событие \"{@event.Title}\"" });
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

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = $"{stepDescription}. Событие \"{@event.Title}\"";
            documentNodes.Add(documentNode);
          }

          foreach (var action in step.Actions)
          {
            var documentName = $"{fullStepDescription}. Действие \"{action.Title}\"";
            var document = new Document(documentName, action.CalculationText);
            document.ComponentType = ComponentType.Wizard;
            document.ComponentName = wizard.Name;
            document.Path = $"Steps.{stepIndex}.Actions.{action.Name}";

            var documentNode = new DocumentTreeNode(document);
            documentNode.Title = $"{stepDescription}. Действие \"{action.Title}\"";
            documentNodes.Add(documentNode);
          }
        }

        wizardNodes.Add(new ContainerTreeNode(documentNodes) { Title = wizard.Title });

        #endregion
      }

      var rootNode = new ContainerTreeNode(wizardNodes);
      rootNode.TitleLocalizationKey = "WIZARDS";
      rootNode.IconSource = WizardsIcon;
      rootNode.IsItemsCountVisible = true;
      return rootNode;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static SourceTreeGenerator()
    {
      DialogsIcon = new BitmapImage(new Uri("/Resources/Dialogs_16.png", UriKind.Relative));
      //DialogsIcon.Freeze();

      DocumentCardTypesIcon = new BitmapImage(new Uri("/Resources/DocumentCardTypes_16.png", UriKind.Relative));
      //DocumentCardTypesIcon.Freeze();

      FunctionsIcon = new BitmapImage(new Uri("/Resources/Functions_16.png", UriKind.Relative));
      //FunctionsIcon.Freeze();

      ReferenceTypesIcon = new BitmapImage(new Uri("/Resources/ReferenceTypes_16.png", UriKind.Relative));
      //ReferenceTypesIcon.Freeze();

      ReportsIcon = new BitmapImage(new Uri("/Resources/Reports_16.png", UriKind.Relative));
      //ReportsIcon.Freeze();

      RouteBlocksIcon = new BitmapImage(new Uri("/Resources/RouteBlocks_16.png", UriKind.Relative));
      //RouteBlocksIcon.Freeze();

      ScriptsIcon = new BitmapImage(new Uri("/Resources/Scripts_16.png", UriKind.Relative));
      //ScriptsIcon.Freeze();

      StandardRoutesIcon = new BitmapImage(new Uri("/Resources/StandardRoutes_16.png", UriKind.Relative));
      //ScriptsIcon.Freeze();

      WizardsIcon = new BitmapImage(new Uri("/Resources/Wizards_16.png", UriKind.Relative));
      //ScriptsIcon.Freeze();
    }

    #endregion
  }
}