using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context.Development;

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
    /// Иконка управляемых папок.
    /// </summary>
    private static readonly ImageSource ManagedFoldersIcon;

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
    /// <param name="context">Контекст с разработкой.</param>
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

      var managedFolders = context.ManagedFolders.Where(r => r.State == ComponentState.Active);
      var managedFoldersTreeNode = GenerateManagedFoldersNode(managedFolders);
      componentTypes.Add(managedFoldersTreeNode);

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
    private static ContainerTreeNode GenerateCommonReportsNode(IEnumerable<CommonReport> commonReports)
    {
      var commonReportNodes = new List<ContainerTreeNode>();
      foreach (var commonReport in commonReports)
      {
        var commonReportNode = new ContainerTreeNode { Title = commonReport.Title };

        var documentName = $"Отчет {commonReport.Title}. Расчет";
        var document = new Document(documentName, commonReport.CalculationText)
        {
          ComponentType = ComponentType.CommonReport,
          ComponentName = commonReport.Name,
          Path = "Calculation"
        };

        var documentNode = new DocumentTreeNode(document) { Title = "Расчет" };
        commonReportNode.Items.Add(documentNode);

        // TODO: Пока не генерируем шаблоны, так как там могут быть
        // и шаблоны в виде документа ворд, ексель, кристал репортс.

        commonReportNodes.Add(commonReportNode);
      }

      var rootNode = new ContainerTreeNode(commonReportNodes)
      {
        TitleLocalizationKey = "COMMON_REPORTS",
        IconSource = ReportsIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел диалогов.
    /// </summary>
    /// <param name="dialogs">Коллекция диалогов.</param>
    /// <returns>Узел диалогов.</returns>
    private static ContainerTreeNode GenerateDialogsNode(IEnumerable<Dialog> dialogs)
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

          var documentName = $"Диалог {dialog.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.Dialog,
            ComponentName = dialog.Name,
            Path = $"Actions.{action.Name}"
          };

          var documentNode = new DocumentTreeNode(document) { Title = $"Действие {action.Name}" };
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Метод {method.Name}" };
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Событие {@event.EventType}"};
          documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"Реквизит {requisite.Name}. Событие {@event.EventType}"
            };
            documentNodes.Add(documentNode);
          }
        }

        var dialogNode = new ContainerTreeNode(documentNodes) { Title = dialog.Title };
        dialogNodes.Add(dialogNode);
      }

      var rootNode = new ContainerTreeNode(dialogNodes)
      {
        TitleLocalizationKey = "DIALOGS",
        IconSource = DialogsIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел типов карточек электронных документов.
    /// </summary>
    /// <param name="documentCardTypes">Коллекция типов карточек электронных документов.</param>
    /// <returns>Узел типов карточек электронных документов.</returns>
    private static ContainerTreeNode GenerateDocumentCardTypesNode(IEnumerable<DocumentCardType> documentCardTypes)
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

          var documentName = $"Тип карточки электронного документа {documentCardType.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.DocumentCardType,
            ComponentName = documentCardType.Name,
            Path = $"Actions.{action.Name}"
          };

          var documentNode = new DocumentTreeNode(document) { Title = $"Действие {action.Name}"};
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Метод {method.Name}"};
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Событие {@event.EventType}"};
          documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"Реквизит {requisite.Name}. Событие {@event.EventType}"
            };
            documentNodes.Add(documentNode);
          }
        }

        var documentCardTypeNode = new ContainerTreeNode(documentNodes) { Title = documentCardType.Title };
        documentCardTypeNodes.Add(documentCardTypeNode);
      }

      var rootNode = new ContainerTreeNode(documentCardTypeNodes)
      {
        TitleLocalizationKey = "DOCUMENT_CARD_TYPES",
        IconSource = DocumentCardTypesIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел функций.
    /// </summary>
    /// <param name="functions">Коллекция функций.</param>
    /// <returns>Узел функций.</returns>
    private static ContainerTreeNode GenerateFunctionsNode(IEnumerable<Function> functions)
    {
      var functionNodes = new List<ContainerTreeNode>();
      foreach (var function in functions)
      {
        var functionNode = new ContainerTreeNode { Title = function.Title };

        var documentName = $"Функция {function.Title}. Вычисление";
        var document = new Document(documentName, function.CalculationText)
        {
          ComponentType = ComponentType.Function,
          ComponentName = function.Name,
          Path = "Calculation"
        };
        document.ContextVariables.AddRange(function.Arguments.Select(a => a.Name.ToUpper()));
        document.ContextVariables.AddRange(function.Arguments.Select(a => "!" + a.Name.ToUpper()));

        var documentNode = new DocumentTreeNode(document) { Title = "Вычисление" };
        functionNode.Items.Add(documentNode);

        functionNodes.Add(functionNode);
      }

      var rootNode = new ContainerTreeNode(functionNodes)
      {
        TitleLocalizationKey = "FUNCTIONS",
        IconSource = FunctionsIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел интегрированных отчетов.
    /// </summary>
    /// <param name="integratedReports">Коллекция интегрированных отчетов.</param>
    /// <returns>Узел интегрированных отчетов.</returns>
    private static ContainerTreeNode GenerateIntegratedReportsNode(IEnumerable<IntegratedReport> integratedReports)
    {
      var integratedReportNodes = new List<ContainerTreeNode>();
      foreach (var integratedReport in integratedReports)
      {
        var integratedReportNode = new ContainerTreeNode { Title = integratedReport.Title };

        var documentName = $"Интегрированный отчет {integratedReport.Title}. Расчет";
        var document = new Document(documentName, integratedReport.CalculationText)
        {
          ComponentType = ComponentType.IntegratedReport,
          ComponentName = integratedReport.Name,
          Path = "Calculation"
        };

        var documentNode = new DocumentTreeNode(document) { Title = "Расчет" };
        integratedReportNode.Items.Add(documentNode);

        // TODO: Пока не генерируем шаблоны, так как там могут быть
        // и шаблоны в виде документа ворд, ексель, кристал репортс.

        integratedReportNodes.Add(integratedReportNode);
      }

      var rootNode = new ContainerTreeNode(integratedReportNodes)
      {
        TitleLocalizationKey = "INTEGRATED_REPORTS",
        IconSource = ReportsIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел управляемых папок.
    /// </summary>
    /// <param name="managedFolders">Коллекция управляемых папок.</param>
    /// <returns>Узел управляемых папок.</returns>
    private static ContainerTreeNode GenerateManagedFoldersNode(IEnumerable<ManagedFolder> managedFolders)
    {
      var managedFolderNodes = new List<ContainerTreeNode>();
      foreach (var managedFolder in managedFolders)
      {
        var documentNodes = new List<DocumentTreeNode>();

        foreach (var action in managedFolder.Actions)
        {
          if (action.ExecutionHandler != null)
          {
            // Не добавлять в дерево действия, которые привязаны к методам.
            continue;
          }

          var documentName = $"Управляемая папка {managedFolder.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.ManagedFolder,
            ComponentName = managedFolder.Name,
            Path = $"Actions.{action.Name}"
          };

          var documentNode = new DocumentTreeNode(document) { Title = $"Действие {action.Name}" };
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Метод {method.Name}" };
          documentNodes.Add(documentNode);
        }

        if (managedFolder.SearchDescription != null)
        {
          var documentName = $"Управляемая папка {managedFolder.Title}. Событие \"До поиска\"";
          var document = new Document(documentName, managedFolder.SearchDescription.BeforeSearchEventText)
          {
            ComponentType = ComponentType.ManagedFolder,
            ComponentName = managedFolder.Name,
            Path = "Events.BeforeSearch"
          };

          var documentNode = new DocumentTreeNode(document) { Title = "Событие \"До поиска\"" };
          documentNodes.Add(documentNode);
        }

        var managedFolderNode = new ContainerTreeNode(documentNodes) { Title = managedFolder.Title };
        managedFolderNodes.Add(managedFolderNode);
      }

      var rootNode = new ContainerTreeNode(managedFolderNodes)
      {
        TitleLocalizationKey = "MANAGED_FOLDERS",
        IconSource = ManagedFoldersIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел типов справочников.
    /// </summary>
    /// <param name="referenceTypes">Коллекция типов справочников.</param>
    /// <returns>Узел типов справочников.</returns>
    private static ContainerTreeNode GenerateReferenceTypesNode(IEnumerable<ReferenceType> referenceTypes)
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

          var documentName = $"Тип справочника {referenceType.Title}. Действие {action.Name}";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.ReferenceType,
            ComponentName = referenceType.Name,
            Path = $"Actions.{action.Name}"
          };

          var documentNode = new DocumentTreeNode(document) { Title = $"Действие {action.Name}"};
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Метод {method.Name}"};
          documentNodes.Add(documentNode);
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

          var documentNode = new DocumentTreeNode(document) { Title = $"Событие {@event.EventType}"};
          documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"Реквизит {requisite.Name}. Событие {@event.EventType}"
            };
            documentNodes.Add(documentNode);
          }
        }

        var referenceTypeNode = new ContainerTreeNode(documentNodes) { Title = referenceType.Title };
        referenceTypeNodes.Add(referenceTypeNode);
      }

      var rootNode = new ContainerTreeNode(referenceTypeNodes)
      {
        TitleLocalizationKey = "REFERENCE_TYPES",
        IconSource = ReferenceTypesIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел сценариев.
    /// </summary>
    /// <param name="scripts">Коллекция сценариев.</param>
    /// <returns>Узел сценариев.</returns>
    private static ContainerTreeNode GenerateScriptsNode(IEnumerable<Script> scripts)
    {
      var scriptNodes = new List<ContainerTreeNode>();
      foreach (var script in scripts)
      {
        var scriptNode = new ContainerTreeNode { Title = script.Title };

        var documentName = $"Сценарий {script.Title}. Вычисление";
        var document = new Document(documentName, script.CalculationText)
        {
          ComponentType = ComponentType.Script,
          ComponentName = script.Name,
          Path = "Calculation"
        };

        var documentNode = new DocumentTreeNode(document) { Title = "Вычисление" };
        scriptNode.Items.Add(documentNode);

        scriptNodes.Add(scriptNode);
      }

      var rootNode = new ContainerTreeNode(scriptNodes)
      {
        TitleLocalizationKey = "SCRIPTS",
        IconSource = ScriptsIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел блоков ТМ.
    /// </summary>
    /// <param name="routeBlocks">Коллекция блоков.</param>
    /// <returns>Узел блоков типовых маршрутов.</returns>
    private static ContainerTreeNode GenerateRouteBlocksNode(IEnumerable<RouteBlock> routeBlocks)
    {
      var routeBlockNodes = new List<ContainerTreeNode>();
      foreach (var block in routeBlocks.Where(b => b.WorkflowBlock != null))
      {
        var documentNodes = new List<DocumentTreeNode>();

        #region События блока
        foreach (var @event in block.WorkflowBlock.Events.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Событие \"{@event.Title}\"";
          var document = new Document(documentName, @event.CalculationText)
          {
            ComponentType = ComponentType.RouteBlock,
            ComponentName = block.Name,
            Path = $"Events.{@event.Name}"
          };

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Событие \"{@event.Title}\"" });
        }
        #endregion

        #region Действия блока
        foreach (var action in block.WorkflowBlock.Actions)
        {
          var documentName = $"Блок {block.Name}. Действие \"{action.Name}\"";
          var document = new Document(documentName, action.CalculationText)
          {
            ComponentType = ComponentType.RouteBlock,
            ComponentName = block.Name,
            Path = $"Actions.{action.Name}"
          };

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Действие \"{action.Name}\"" });
        }
        #endregion

        #region Вычисления свойств блока
        foreach (var isblProp in block.WorkflowBlock.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
        {
          var documentName = $"Блок {block.Name}. Свойство \"{isblProp.Title}\"";
          var document = new Document(documentName, isblProp.CalculationText)
          {
            ComponentType = ComponentType.RouteBlock,
            ComponentName = block.Name,
            Path = $"Properties.{isblProp.Name}"
          };

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Свойство \"{isblProp.Title}\"" });
        }
        #endregion

        routeBlockNodes.Add(new ContainerTreeNode(documentNodes) { Title = block.Title });
      }

      var rootNode = new ContainerTreeNode(routeBlockNodes)
      {
        TitleLocalizationKey = "ROUTE_BLOCKS",
        IconSource = RouteBlocksIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел типовых маршрутов.
    /// </summary>
    /// <param name="standardRoutes">Коллекция типовых маршрутов.</param>
    /// <returns>Узел типовых маршрутов.</returns>
    private static ContainerTreeNode GenerateStandardRoutesNode(IEnumerable<StandardRoute> standardRoutes)
    {
      var standardRouteNodes = new List<ContainerTreeNode>();
      foreach (var route in standardRoutes.Where(r => r.WorkflowDescription != null))
      {
        var documentNodes = new List<DocumentTreeNode>();

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

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Событие \"{@event.Title}\"" });
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
            var document = new Document(documentName, @event.CalculationText)
            {
              ComponentType = ComponentType.StandardRoute,
              ComponentName = route.Name,
              Path = $"Blocks.{block.Id}.Events.{@event.Name}"
            };

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"{blockDescription}. Событие \"{@event.Title}\""
            };
            documentNodes.Add(documentNode);
          }
          #endregion

          #region Действия блока
          foreach (var action in block.Actions)
          {
            var documentName = $"{fullBlockDescription}. Действие \"{action.Name}\"";
            var document = new Document(documentName, action.CalculationText)
            {
              ComponentType = ComponentType.StandardRoute,
              ComponentName = route.Name,
              Path = $"Blocks.{block.Id}.Actions.{action.Name}"
            };

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"{blockDescription}. Действие \"{action.Name}\""
            };
            documentNodes.Add(documentNode);
          }
          #endregion

          #region Вычисления свойств блока
          foreach (var isblProp in block.IsblProperties.Where(e => !string.IsNullOrEmpty(e.CalculationText)))
          {
            var documentName = $"{fullBlockDescription}. Свойство \"{isblProp.Title}\"";
            var document = new Document(documentName, isblProp.CalculationText)
            {
              ComponentType = ComponentType.StandardRoute,
              ComponentName = route.Name,
              Path = $"Blocks.{block.Id}.Properties.{isblProp.Name}"
            };

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"{blockDescription}. Свойство \"{isblProp.Title}\""
            };
            documentNodes.Add(documentNode);
          }
          #endregion
        }

        standardRouteNodes.Add(new ContainerTreeNode(documentNodes) { Title = route.Title });

        #endregion
      }

      var rootNode = new ContainerTreeNode(standardRouteNodes)
      {
        TitleLocalizationKey = "STANDARD_ROUTES",
        IconSource = StandardRoutesIcon,
        IsItemsCountVisible = true
      };
      return rootNode;
    }

    /// <summary>
    /// Сгенерировать узел мастеров действий.
    /// </summary>
    /// <param name="wizards">Коллекция мастеров действий.</param>
    /// <returns>Узел мастеров действий.</returns>
    private static ContainerTreeNode GenerateWizardsTreeNode(IEnumerable<Wizard> wizards)
    {
      var wizardNodes = new List<ContainerTreeNode>();
      foreach (var wizard in wizards)
      {
        var documentNodes = new List<DocumentTreeNode>();

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

          documentNodes.Add(new DocumentTreeNode(document) { Title = $"Событие \"{@event.Title}\"" });
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

            var documentNode = new DocumentTreeNode(document) { Title = $"{stepDescription}. Событие \"{@event.Title}\"" };
            documentNodes.Add(documentNode);
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

            var documentNode = new DocumentTreeNode(document)
            {
              Title = $"{stepDescription}. Действие \"{action.Title}\""
            };
            documentNodes.Add(documentNode);
          }
        }

        wizardNodes.Add(new ContainerTreeNode(documentNodes) { Title = wizard.Title });

        #endregion
      }

      var rootNode = new ContainerTreeNode(wizardNodes)
      {
        TitleLocalizationKey = "WIZARDS",
        IconSource = WizardsIcon,
        IsItemsCountVisible = true
      };
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

      ManagedFoldersIcon = new BitmapImage(new Uri("/Resources/ReferenceIcon_16.png", UriKind.Relative));
      //ReferenceTypesIcon.Freeze();

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