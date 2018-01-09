using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик типов карточек электронных документов.
  /// </summary>
  internal class DocumentCardTypePackageHandler : IPackageHandler<DocumentCardType>
  {
    #region Константы

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    /// <summary>
    /// Метод нумерации.
    /// </summary>
    private const string NumerationMethodReqName = "ISBEDocTypeNumerationMethod";

    /// <summary>
    /// Имя.
    /// </summary>
    private const string ActionNameReqName = "ISBEDocTypeActCode";

    /// <summary>
    /// Текст вычисления действия.
    /// </summary>
    private const string ActionCalculationTextReqName = "ISBEDocTypeActOnExecute";

    /// <summary>
    /// События.
    /// </summary>
    private const string EventsReqName = "ISBRefTypeEventText";

    /// <summary>
    /// Номер реквизита.
    /// </summary>
    private const string ReqNumberReqName = "ISBEDocTypeReqNumber";

    /// <summary>
    /// Секция реквизита.
    /// </summary>
    private const string ReqSectionReqName = "ISBEDocTypeReqSection";

    /// <summary>
    /// Имя реквизита.
    /// </summary>
    private const string ReqNameReqName = "ISBEDocTypeReqCode";

    /// <summary>
    /// Обязательность заполнения реквизита.
    /// </summary>
    private const string ReqIsRequiredReqName = "ISBEDocTypeReqIsRequired";

    /// <summary>
    /// Лидирующий.
    /// </summary>
    private const string ReqIsLeadingReqName = "ISBEDocTypeReqIsLeading";

    /// <summary>
    /// Событие изменения.
    /// </summary>
    private const string ReqChangeEventReqName = "ISBEDocTypeReqOnChange";

    /// <summary>
    /// События выбора.
    /// </summary>
    private const string ReqSelectEventsReqName = "ISBEDocTypeReqOnSelect";

    /// <summary>
    /// Имя представления.
    /// </summary>
    private const string ViewNameReqName = "ISBEDocTypeViewCode";

    /// <summary>
    /// Главное представление.
    /// </summary>
    private const string ViewIsMainReqName = "ISBEDocTypeViewIsMain";

    /// <summary>
    /// Форма карточки.
    /// </summary>
    private const string ViewCardFormReqName = "ISBEDocTypeViewCardForm";

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
    /// Код реквизита, хранящий ИД справочника.
    /// </summary>
    private const string RefIDRequisiteName = "ИДЗапГлавРазд";

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

    private void ReadMethods(ComponentModel model, DocumentCardType entity)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet7 == null)
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

    private void ReadMethodsParameters(ComponentModel model, DocumentCardType entity)
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

    private void ReadActions(ComponentModel model, DocumentCardType entity)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet2 == null)
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
    private void ReadEvents(ComponentModel model, DocumentCardType entity)
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
    private void ReadCardRequisites(ComponentModel model, DocumentCardType entity)
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

        var isLeadingReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqIsLeadingReqName);
        if (isLeadingReq != null)
          cardRequisite.IsLeading = isLeadingReq.ValueLocalizeID == YesValue;

        var changeEventReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqChangeEventReqName);
        if (changeEventReq != null && !string.IsNullOrEmpty(changeEventReq.DecodedText))
        {
          var @event = new Event();
          @event.EventType = EventType.Change;
          @event.CalculationText = changeEventReq.DecodedText;
          cardRequisite.Events.Add(@event);
        }

        var selectEventReq = row.Requisites
          .FirstOrDefault(r => r.Code == ReqSelectEventsReqName);
        if (selectEventReq != null && !string.IsNullOrEmpty(selectEventReq.DecodedText))
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
    private void ReadViews(ComponentModel model, DocumentCardType entity)
    {
      if (model.DetailDataSets == null || model.DetailDataSets.DetailDataSet3 == null)
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

        var cardFormReq = row.Requisites
          .FirstOrDefault(r => r.Code == ViewCardFormReqName);
        if (cardFormReq != null)
          view.CardForm = cardFormReq.DecodedText;

        entity.Views.Add(view);
      }
    }

    #endregion

    #region IPackageHandler

    public IEnumerable<DocumentCardType> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.DocumentCardTypes)
      {
        var entity = PackageHandlerUtils.CreateEntity<DocumentCardType>(model);

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

        this.ReadMethods(model, entity);
        this.ReadMethodsParameters(model, entity);
        this.ReadActions(model, entity);
        this.ReadEvents(model, entity);
        this.ReadCardRequisites(model, entity);
        this.ReadViews(model, entity);

        yield return entity;
      }
    }

    #endregion
  }
}
