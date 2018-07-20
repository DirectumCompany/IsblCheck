using System;
using System.Collections.Generic;
using System.Linq;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик типов справочников.
  /// </summary>
  internal class ReferenceTypePackageHandler : IPackageHandler<ReferenceType>
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
    /// Имя.
    /// </summary>
    private const string ActionNameReqName = "ISBRefTypeActCode";

    /// <summary>
    /// Текст вычисления действия.
    /// </summary>
    private const string ActionCalculationTextReqName = "ISBRefTypeActOnExecute";

    /// <summary>
    /// События.
    /// </summary>
    private const string EventsReqName = "ISBRefTypeEventText";

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
    /// Событие изменения.
    /// </summary>
    private const string ReqChangeEventReqName = "ISBRefTypeReqOnChange";

    /// <summary>
    /// События выбора.
    /// </summary>
    private const string ReqSelectEventsReqName = "ISBRefTypeReqOnSelect";

    /// <summary>
    /// Имя представления.
    /// </summary>
    private const string ViewNameReqName = "ISBRefTypeViewCode";

    /// <summary>
    /// Главное представление.
    /// </summary>
    private const string ViewIsMainReqName = "ISBRefTypeViewIsMain";

    /// <summary>
    /// Форма списка.
    /// </summary>
    private const string ViewListFormReqName = "ISBRefTypeViewListForm";

    /// <summary>
    /// Форма карточки.
    /// </summary>
    private const string ViewCardFormReqName = "ISBRefTypeViewCardForm";

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
    /// Текст вычисления метода.
    /// </summary>
    private const string MethodCalculationTextReqName = "Calculation";

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

    #region Методы

    private static void ReadMethods(ComponentModel model, ReferenceType entity)
    {
      if (model.DetailDataSets?.DetailDataSet7 == null)
        return;

      foreach (var row in model.DetailDataSets.DetailDataSet7.Rows)
      {
        var method = new Method();

        var nameReq = row.Requisites
          .FirstOrDefault(r => r.Code == MethodNameReqName);
        if (nameReq != null)
          method.Name = nameReq.DecodedText;

        var calculationTextReq = row.Requisites
          .FirstOrDefault(r => r.Code == MethodCalculationTextReqName);
        if (calculationTextReq != null)
          method.CalculationText = calculationTextReq.DecodedText;

        entity.Methods.Add(method);
      }
    }

    private static void ReadMethodsParameters(ComponentModel model, ReferenceType entity)
    {
      if (model.DetailDataSets?.DetailDataSet8 == null)
        return;

      foreach (var paramModel in model.DetailDataSets.DetailDataSet8.Rows)
      {
        var parameter = new MethodParam();

        var methodNameReq = paramModel.Requisites
          .FirstOrDefault(r => r.Code == MethodParamMethodNameReqName);
        if (methodNameReq == null)
          continue;

        var methodName = methodNameReq.DecodedText;
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
          parameter.Name = paramNameReq.DecodedText;

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
          parameter.DefaultValue = paramDefaultValueReq.DecodedText;
        parameter.HasDefaultValue = !string.IsNullOrEmpty(parameter.DefaultValue);

        method.Params.Add(parameter);
      }
    }

    private static void ReadActions(ComponentModel model, ReferenceType entity)
    {
      if (model.DetailDataSets?.DetailDataSet2 == null)
        return;

      foreach (var row in model.DetailDataSets.DetailDataSet2.Rows)
      {
        var action = new ActionWithHandler();

        var nameReq = row.Requisites
          .FirstOrDefault(r => r.Code == ActionNameReqName);
        if (nameReq != null)
          action.Name = nameReq.DecodedText;

        var calculationTextReq = row.Requisites
          .FirstOrDefault(r => r.Code == ActionCalculationTextReqName);
        if (calculationTextReq != null)
        {
          var calculationText = calculationTextReq.DecodedText;
          var executionHandler = entity.Methods
            .FirstOrDefault(m => m.Name.Equals(calculationText, StringComparison.OrdinalIgnoreCase));
          if (executionHandler != null)
          {
            action.ExecutionHandler = executionHandler;
          }
          else
          {
            action.CalculationText = calculationText;
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
    private static void ReadEvents(ComponentModel model, ReferenceType entity)
    {
      var eventsReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == EventsReqName);
      if (eventsReq == null)
        return;

      var events = EventTextParser.Parse(eventsReq.DecodedText);
      foreach (var @event in events)
        entity.Events.Add(@event);
    }

    /// <summary>
    /// Прочитать реквизиты карточки.
    /// </summary>
    /// <param name="model">Модель.</param>
    /// <param name="entity">Сущность.</param>
    private static void ReadCardRequisites(ComponentModel model, ReferenceType entity)
    {
      if (model.DetailDataSets?.DetailDataSet1 == null)
        return;

      foreach (var row in model.DetailDataSets.DetailDataSet1.Rows)
      {
        var cardRequisite = new CardRequisite();

        var numberReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqNumberReqName);
        if (!string.IsNullOrEmpty(numberReq?.Value))
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

        var changeEventReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqChangeEventReqName);
        if (!string.IsNullOrEmpty(changeEventReq?.DecodedText))
        {
          var @event = new Event
          {
            EventType = EventType.Change,
            CalculationText = changeEventReq.DecodedText
          };
          cardRequisite.Events.Add(@event);
        }

        var selectEventReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqSelectEventsReqName);
        if (!string.IsNullOrEmpty(selectEventReq?.DecodedText))
        {
          var events = EventTextParser.Parse(selectEventReq.DecodedText);
          foreach (var @event in events)
          {
            if (@event.EventType == EventType.Unknown)
              @event.EventType = EventType.Select;
            cardRequisite.Events.Add(@event);
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
    private static void ReadViews(ComponentModel model, ReferenceType entity)
    {
      if (model.DetailDataSets?.DetailDataSet3 == null)
        return;

      foreach (var row in model.DetailDataSets.DetailDataSet3.Rows)
      {
        var view = new View();

        var nameReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewNameReqName);
        if (nameReq != null)
          view.Name = nameReq.DecodedText;

        var isMainReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewIsMainReqName);
        if (isMainReq != null)
          view.IsMain = isMainReq.ValueLocalizeID == YesValue;

        var listFormReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewListFormReqName);
        if (listFormReq != null)
          view.ListForm = listFormReq.DecodedText;

        var cardFormReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewCardFormReqName);
        if (cardFormReq != null)
          view.CardForm = cardFormReq.DecodedText;

        entity.Views.Add(view);
      }
    }

    #endregion

    #region IPackageHandler

    public IEnumerable<ReferenceType> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.ReferenceTypes)
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
          entity.LeadingReference = leadingReferenceReq.DecodedText;

        var hasLongRecordNamesReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == HasLongRecordNamesReqName);
        if (hasLongRecordNamesReq != null)
          entity.HasLongRecordNames = hasLongRecordNamesReq.ValueLocalizeID == YesValue;

        var hasUniqueRecordNamesReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == HasUniqueRecordNames);
        if (hasUniqueRecordNamesReq != null)
          entity.HasUniqueRecordNames = hasUniqueRecordNamesReq.ValueLocalizeID == YesValue;

        ReadMethods(model, entity);
        ReadMethodsParameters(model, entity);
        ReadActions(model, entity);
        ReadEvents(model, entity);
        ReadCardRequisites(model, entity);
        ReadViews(model, entity);

        yield return entity;
      }
    }

    #endregion
  }
}
