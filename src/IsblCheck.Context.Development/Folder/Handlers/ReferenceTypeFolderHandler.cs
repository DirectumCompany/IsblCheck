using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal class ReferenceTypeFolderHandler : FolderHandlerBase<ReferenceType, ComponentModel>
  {
    #region Константы

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    /// <summary>
    /// Метод нумерации.
    /// </summary>
    private const string NumerationMethodReqName = "ISBRefTypeNumerationMethod";

    /// <summary>
    /// Имя отображаемого реквизита.
    /// </summary>
    private const string DisplayRequisiteReqName = "ISBRefTypeDisplayReqName";

    /// <summary>
    /// Лидирующий справочник.
    /// </summary>
    private const string LeadingReferenceReqName = "ISBRefTypeMainLeadingRef";

    /// <summary>
    /// Признак длинных имен записей.
    /// </summary>
    private const string HasLongRecordNamesReqName = "ISBIsNameLong";

    /// <summary>
    /// Признак уникальности имен записей.
    /// </summary>
    private const string HasUniqueRecordNames = "ISBIsNameUnique";

    /// <summary>
    /// Имя действия.
    /// </summary>
    private const string ActionNameReqName = "ISBRefTypeActCode";

    /// <summary>
    /// Имя метода.
    /// </summary>
    private const string ActionMethodNameReqName = "ISBRefTypeActOnExecute";

    /// <summary>
    /// Номер реквизита.
    /// </summary>
    private const string ReqNumberReqName = "ISBRefTypeReqNumber";

    /// <summary>
    /// Секция реквизита.
    /// </summary>
    private const string ReqSectionReqName = "ISBRefTypeReqSection";

    /// <summary>
    /// Имя реквизита.
    /// </summary>
    private const string ReqNameReqName = "ISBRefTypeReqCode";

    /// <summary>
    /// Обязательность заполнения реквизита.
    /// </summary>
    private const string ReqIsRequiredReqName = "ISBRefTypeReqIsRequired";

    /// <summary>
    /// Фильтр.
    /// </summary>
    private const string ReqIsFilterReqName = "ISBRefTypeReqIsFilter";

    /// <summary>
    /// Лидирующий.
    /// </summary>
    private const string ReqIsLeadingReqName = "ISBRefTypeReqIsLeading";

    /// <summary>
    /// Контроль.
    /// </summary>
    private const string ReqIsControlReqName = "ISBRefTypeReqIsControl";

    /// <summary>
    /// Имя представления.
    /// </summary>
    private const string ViewNameReqName = "ISBRefTypeViewCode";

    /// <summary>
    /// Главное представление.
    /// </summary>
    private const string ViewIsMainReqName = "ISBRefTypeViewIsMain";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string YesValue = "SYSRES_COMMON.YES_CONST";

    /// <summary>
    /// Активная.
    /// </summary>
    private const string Active = "SYSRES_SYSCOMP.OPERATING_RECORD_FLAG_VALUE_MASCULINE";

    /// <summary>
    /// Автоматическая строгая.
    /// </summary>
    private const string StronglyAuto = "SYSRES_SYSCOMP.NUMERATION_AUTO_STRONG";

    /// <summary>
    /// Автоматическая не строгая.
    /// </summary>
    private const string Auto = "SYSRES_SYSCOMP.NUMERATION_AUTO_NOT_STRONG";

    /// <summary>
    /// Ручная.
    /// </summary>
    private const string Manual = "SYSRES_SYSCOMP.NUMERATION_MANUAL";

    /// <summary>
    /// Имя записи.
    /// </summary>
    private const string RecordName = "SYSRES_SYSCOMP.RECORD_NAME_REQUISITE_NAME";

    /// <summary>
    /// Имя метода.
    /// </summary>
    private const string MethodNameReqName = "Name";

    /// <summary>
    /// Имя реквизита номера аргумента.
    /// </summary>
    private const string MethodParamNumberReqName = "НомСтр";

    /// <summary>
    /// Имя метода параметра метода.
    /// </summary>
    private const string MethodParamMethodNameReqName = "MethodName";

    /// <summary>
    /// Имя реквизита имени аргумента.
    /// </summary>
    private const string MethodParamNameReqName = "Name";

    /// <summary>
    /// Имя реквизита типа аргумента.
    /// </summary>
    private const string MethodParamTypeReqName = "ValueType";

    /// <summary>
    /// Имя реквизита типа аргумента.
    /// </summary>
    private const string MethodParamDefaultValueReqName = "DefaultValue";

    /// <summary>
    /// Вариантный.
    /// </summary>
    private const string VariantArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_VARIANT";

    /// <summary>
    /// Дата.
    /// </summary>
    private const string DateArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_DATE";

    /// <summary>
    /// Дробное число.
    /// </summary>
    private const string FloatArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_FLOAT";

    /// <summary>
    /// Логический.
    /// </summary>
    private const string BooleanArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_BOOLEAN";

    /// <summary>
    /// Целое число.
    /// </summary>
    private const string StringArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_STRING";

    /// <summary>
    /// Строка.
    /// </summary>
    private const string IntegerArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_INTEGER";

    #endregion

    #region Поля и свойства

    private static readonly ILog log = LogManager.GetLogger<ReferenceTypeFolderHandler>(); 

    #endregion

    #region FolderHandlerBase

    protected override string FolderName { get { return "ReferenceTypes"; } }

    protected override string CardModelRootNode { get { return "ReferenceType"; } }

    protected override IEnumerable<ReferenceType> ReadComponents(ComponentModel model, string componentFolderPath)
    {
      var entity = PackageHandlerUtils.CreateEntity<ReferenceType>(model);

      var stateReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == StateReqName);
      if (stateReq != null)
        entity.State = stateReq.ValueLocalizeID == Active ? ComponentState.Active : ComponentState.Closed;

      var numerationMethodReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == NumerationMethodReqName);
      if (numerationMethodReq != null)
        switch (numerationMethodReq.ValueLocalizeID)
        {
          case StronglyAuto:
            entity.NumerationMethod = NumerationMethod.StronglyAuto;
            break;
          case Auto:
            entity.NumerationMethod = NumerationMethod.Auto;
            break;
          case Manual:
            entity.NumerationMethod = NumerationMethod.Manual;
            break;
        }

      var displayRequisiteReqName = model.Card.Requisites
        .FirstOrDefault(r => r.Code == DisplayRequisiteReqName);
      if (displayRequisiteReqName != null)
        switch (displayRequisiteReqName.ValueLocalizeID)
        {
          case RecordName:
            entity.DisplayRequisite = DisplayRequisite.Name;
            break;
          default:
            entity.DisplayRequisite = DisplayRequisite.Code;
            break;
        }

      var leadingReferenceReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == LeadingReferenceReqName);
      if (leadingReferenceReq != null)
        entity.LeadingReference = leadingReferenceReq.Value;

      var hasLongRecordNamesReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == HasLongRecordNamesReqName);
      if (hasLongRecordNamesReq != null)
        entity.HasLongRecordNames = hasLongRecordNamesReq.ValueLocalizeID == YesValue;

      var hasUniqueRecordNamesReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == HasUniqueRecordNames);
      if (hasUniqueRecordNamesReq != null)
        entity.HasUniqueRecordNames = hasUniqueRecordNamesReq.ValueLocalizeID == YesValue;

      this.ReadMethods(model, entity, componentFolderPath);
      this.ReadMethodsParameters(model, entity, componentFolderPath);
      this.ReadActions(model, entity, componentFolderPath);
      this.ReadEvents(model, entity, componentFolderPath);
      this.ReadCardRequisites(model, entity, componentFolderPath);
      this.ReadViews(model, entity, componentFolderPath);

      yield return entity;
    }

    #endregion

    #region Методы

    private void ReadMethods(ComponentModel model, ReferenceType entity, string componentFolderPath)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet7 == null)
        return;

      foreach (var methodModel in model.DetailDataSets.DetailDataSet7.Rows)
      {
        var method = new Method();

        var nameReq = methodModel.Requisites
          .FirstOrDefault(r => r.Code == MethodNameReqName);
        if (nameReq != null)
          method.Name = nameReq.Value;

        var methodFileName = Path.Combine(componentFolderPath, "Methods", $"{method.Name}.isbl");
        if (File.Exists(methodFileName))
        {
          method.CalculationText = File.ReadAllText(methodFileName, Encoding.GetEncoding(1251));
        }
        else
        {
          // Не выводим ошибку, если файл не найден: для платформенных действий вычисления не выгружаются.
          method.CalculationText = "";
        }

        entity.Methods.Add(method);
      }
    }

    private void ReadMethodsParameters(ComponentModel model, ReferenceType entity, string componentFolderPath)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet8 == null)
        return;

      foreach (var paramModel in model.DetailDataSets.DetailDataSet8.Rows)
      {
        var parameter = new MethodParam();

        var methodNameReq = paramModel.Requisites
          .FirstOrDefault(r => r.Code == MethodParamMethodNameReqName);
        if (methodNameReq == null)
          continue;

        var methodName = methodNameReq.Value;
        var method = entity.Methods
          .FirstOrDefault(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));
        if (method == null)
          continue;

        var paramNumberReq = paramModel.Requisites
          .FirstOrDefault(r => r.Code == MethodParamNumberReqName);
        if (paramNumberReq != null)
          parameter.Number = int.Parse(paramNumberReq.Value);

        var paramNameReq = paramModel.Requisites
          .FirstOrDefault(r => r.Code == MethodParamNameReqName);
        if (paramNameReq != null)
          parameter.Name = paramNameReq.Value;

        var paramTypeReq = paramModel.Requisites
          .FirstOrDefault(r => r.Code == MethodParamTypeReqName);
        if (paramTypeReq != null)
        {
          switch (paramTypeReq.ValueLocalizeID)
          {
            case VariantArgumentType:
              parameter.Type = MethodParamType.Variant;
              break;
            case DateArgumentType:
              parameter.Type = MethodParamType.Date;
              break;
            case FloatArgumentType:
              parameter.Type = MethodParamType.Float;
              break;
            case BooleanArgumentType:
              parameter.Type = MethodParamType.Boolean;
              break;
            case StringArgumentType:
              parameter.Type = MethodParamType.String;
              break;
            case IntegerArgumentType:
              parameter.Type = MethodParamType.Integer;
              break;
          }
        }

        var paramDefaultValueReq = paramModel.Requisites
          .FirstOrDefault(r => r.Code == MethodParamDefaultValueReqName);
        if (paramDefaultValueReq != null)
          parameter.DefaultValue = paramDefaultValueReq.Value;
        parameter.HasDefaultValue = !string.IsNullOrEmpty(parameter.DefaultValue);

        method.Params.Add(parameter);
      }
    }


    private void ReadActions(ComponentModel model, ReferenceType entity, string componentFolderPath)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet2 == null)
        return;

      foreach (var actionModel in model.DetailDataSets.DetailDataSet2.Rows)
      {
        var action = new ActionWithHandler();

        var nameReq = actionModel.Requisites
          .FirstOrDefault(r => r.Code == ActionNameReqName);
        if (nameReq != null)
          action.Name = nameReq.Value;

        var methodNameReq = actionModel.Requisites
          .FirstOrDefault(r => r.Code == ActionMethodNameReqName);
        if (methodNameReq != null)
        {
          // Заполнить имя действия, только если его не нашли ранее (имя метода менее читабельное).
          var methodName = methodNameReq.Value;
          if (action.Name == null)
            action.Name = methodName;

          var executionHandler = entity.Methods
            .FirstOrDefault(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));
          if (executionHandler != null)
          {
            action.ExecutionHandler = executionHandler;
          }
          else
          {
            log.Warn($"Method {methodName} for action {action.Name} not found");
            action.CalculationText = "";
          }
        }

        if (action.ExecutionHandler == null)
        {
          var actionFileName = Path.Combine(componentFolderPath, "Actions", $"{action.Name}.isbl");
          if (File.Exists(actionFileName))
          {
            action.CalculationText = File.ReadAllText(actionFileName, Encoding.GetEncoding(1251));
          }
          else
          {
            log.Warn($"File not found {actionFileName}");
            action.CalculationText = "";
          }
        }

        entity.Actions.Add(action);
      }
    }

    /// <summary>
    /// Прочитать события.
    /// </summary>
    /// <param name="model">Модель.</param>
    /// <param name="entity">Сущность.</param>
    private void ReadEvents(ComponentModel model, ReferenceType entity, string componentFolderPath)
    {
      var eventsTextsFolder = Path.Combine(componentFolderPath, "Events");
      if (!Directory.Exists(eventsTextsFolder))
        return;

      foreach (var eventFile in Directory.EnumerateFiles(eventsTextsFolder, "*.isbl"))
      {
        var eventName = Path.GetFileNameWithoutExtension(eventFile);
        var eventType = EventTypeResolver.GetExportedEventType(eventName);
        var calculationText = File.ReadAllText(eventFile, Encoding.GetEncoding(1251));
        entity.Events.Add(new Event
        {
          EventType = eventType,
          CalculationText = calculationText
        });
      }
    }

    /// <summary>
    /// Прочитать реквизиты карточки.
    /// </summary>
    /// <param name="model">Модель.</param>
    /// <param name="entity">Сущность.</param>
    private void ReadCardRequisites(ComponentModel model, ReferenceType entity, string componentFolderPath)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet1 == null)
        return;

      foreach (var row in model.DetailDataSets.DetailDataSet1.Rows)
      {
        var cardRequisite = new CardRequisite();

        var numberReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqNumberReqName);
        if (numberReq != null && !string.IsNullOrEmpty(numberReq.Value))
          cardRequisite.Number = int.Parse(numberReq.Value);

        var sectionReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqSectionReqName);
        if (sectionReq != null)
          cardRequisite.Section = RequisiteHandlerUtils.GetRequisiteSection(sectionReq.ValueLocalizeID);

        var nameReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqNameReqName);
        if (nameReq != null)
          cardRequisite.Name = nameReq.Value;

        var isRequiredReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqIsRequiredReqName);
        if (isRequiredReq != null)
          cardRequisite.IsRequired = isRequiredReq.ValueLocalizeID == YesValue;

        var isFilterReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqIsFilterReqName);
        if (isFilterReq != null)
          cardRequisite.IsFilter = isFilterReq.ValueLocalizeID == YesValue;

        var isLeadingReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqIsLeadingReqName);
        if (isLeadingReq != null)
          cardRequisite.IsLeading = isLeadingReq.ValueLocalizeID == YesValue;

        var isControlReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqIsControlReqName);
        if (isControlReq != null)
          cardRequisite.IsSelectionConstrained = isControlReq.ValueLocalizeID == YesValue;

        var requisiteEventsFolder = Path.Combine(componentFolderPath, "Requisites", cardRequisite.Name);
        if (Directory.Exists(requisiteEventsFolder))
        {
          foreach (var eventFile in Directory.EnumerateFiles(requisiteEventsFolder, "*.isbl"))
          {
            var eventName = Path.GetFileNameWithoutExtension(eventFile);
            var eventType = EventTypeResolver.GetExportedEventType(eventName);
            var calculationText = File.ReadAllText(eventFile, Encoding.GetEncoding(1251));
            cardRequisite.Events.Add(new Event
            {
              EventType = eventType,
              CalculationText = calculationText
            });
          }
        }

        entity.Requisites.Add(cardRequisite);
      }
    }

    /// <summary>
    /// Прочитать действия.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="entity"></param>
    private void ReadViews(ComponentModel model, ReferenceType entity, string componentFolderPath)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet3 == null)
        return;

      foreach (var row in model.DetailDataSets.DetailDataSet3.Rows)
      {
        var view = new View();

        var nameReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewNameReqName);
        if (nameReq != null)
          view.Name = nameReq.Value;

        var isMainReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewIsMainReqName);
        if (isMainReq != null)
          view.IsMain = isMainReq.ValueLocalizeID == YesValue;

        var cardFormFile = Path.Combine(componentFolderPath, "Views", view.Name, "CardForm.dfm");
        if (File.Exists(cardFormFile))
          view.CardForm = File.ReadAllText(cardFormFile, Encoding.GetEncoding(1251));

        var listFormFile = Path.Combine(componentFolderPath, "Views", view.Name, "ListForm.dfm");
        if (File.Exists(listFormFile))
          view.ListForm = File.ReadAllText(listFormFile, Encoding.GetEncoding(1251));

        entity.Views.Add(view);
      }
    }

    #endregion
  }
}
