using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Action = IsblCheck.Core.Context.Development.Action;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик типов карточек электронных документов.
  /// </summary>
  internal class DocumentCardTypeDatabaseHandler : IDatabaseHandler<DocumentCardType>
  {
    #region IDatabaseHandler

    public IEnumerable<DocumentCardType> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new Dictionary<string, DocumentCardType>();
      var query = this.GetDocumentCardTypeQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components.Values.ToList();

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var documentCardType = new DocumentCardType();
            documentCardType.Name = reader["Name"] as string;
            documentCardType.Title = reader["Title"] as string;

            var stateValue = reader["State"] as string;
            if (stateValue != null &&
              StateValues.ContainsKey(stateValue))
              documentCardType.State = StateValues[stateValue];

            var numerationMethodValue = reader["NumerationMethod"] as string;
            if (numerationMethodValue != null &&
              NumerationMethodValues.ContainsKey(numerationMethodValue))
              documentCardType.NumerationMethod = NumerationMethodValues[numerationMethodValue];

            var eventsValue = reader["Events"] as string;
            if (!string.IsNullOrEmpty(eventsValue))
            {
              var events = EventTextParser.Parse(eventsValue);
              foreach (var @event in events)
                documentCardType.Events.Add(@event);
            }

            components.Add(documentCardType.Name, documentCardType);
          }
        }
      }

      query = this.GetMethodsQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var documentCardTypeName = reader["DocumentCardTypeName"] as string;
              DocumentCardType documentCardType;
              if (!components.TryGetValue(documentCardTypeName, out documentCardType))
                continue;

              var method = new Method();
              method.Name = reader["Name"] as string;
              method.CalculationText = reader["CalculationText"] as string ?? string.Empty;

              documentCardType.Methods.Add(method);
            }
          }
        }
      }

      query = this.GetMethodParamsQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var documentCardTypeName = reader["DocumentCardTypeName"] as string;
              DocumentCardType documentCardType;
              if (!components.TryGetValue(documentCardTypeName, out documentCardType))
                continue;

              var methodName = reader["MethodName"] as string;
              var method = documentCardType.Methods
                .FirstOrDefault(m => m.Name == methodName);
              if (method == null)
                continue;

              var parameter = new MethodParam();
              parameter.Number = (int)reader["Number"];
              parameter.Name = reader["Name"] as string;

              var typeValue = reader["Type"] as string;
              MethodParamType methodParamType;
              if (TypeValues.TryGetValue(typeValue, out methodParamType))
              {
                parameter.Type = methodParamType;
              }
              parameter.DefaultValue = reader["DefaultValue"] as string;
              parameter.HasDefaultValue = !string.IsNullOrEmpty(parameter.DefaultValue);

              method.Params.Add(parameter);
            }
          }
        }
      }

      query = this.GetActionQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var documentCardTypeName = reader["DocumentCardTypeName"] as string;
              DocumentCardType documentCardType;
              if (!components.TryGetValue(documentCardTypeName, out documentCardType))
                continue;

              var action = new ActionWithHandler();
              action.Name = reader["Name"] as string;

              var calculationText = reader["CalculationText"] as string;
              var executionHandler = documentCardType.Methods
                .FirstOrDefault(m => m.Name == calculationText);
              if (executionHandler != null)
              {
                action.ExecutionHandler = executionHandler;
              }
              else
              {
                action.CalculationText = calculationText ?? string.Empty;
              }

              documentCardType.Actions.Add(action);
            }
          }
        }
      }

      query = this.GetRequisiteQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var documentCardTypeName = reader["DocumentCardTypeName"] as string;
              DocumentCardType documentCardType;
              if (!components.TryGetValue(documentCardTypeName, out documentCardType))
                continue;

              var cardRequisite = new CardRequisite();
              cardRequisite.Number = (int)reader["Number"];

              var sectionValue = reader["Section"] as string;
              if (sectionValue != null &&
                RequisiteSectionValues.ContainsKey(sectionValue))
                cardRequisite.Section = RequisiteSectionValues[sectionValue];

              cardRequisite.Name = reader["Name"] as string;
              cardRequisite.IsRequired = RuYesValue.Equals(reader["IsRequired"] as string);
              cardRequisite.IsLeading = RuYesValue.Equals(reader["IsLeading"] as string);

              var changeEventValue = reader["ChangeEvent"] as string;
              if (!string.IsNullOrEmpty(changeEventValue))
              {
                var @event = new Event();
                @event.EventType = EventType.Change;
                @event.CalculationText = changeEventValue;
                cardRequisite.Events.Add(@event);
              }

              var selectEventsValue = reader["SelectEvents"] as string;
              if (!string.IsNullOrEmpty(selectEventsValue))
              {
                var events = EventTextParser.Parse(selectEventsValue);
                foreach (var @event in events)
                {
                  if (@event.EventType == EventType.Unknown)
                    @event.EventType = EventType.Select;
                  cardRequisite.Events.Add(@event);
                }
              }

              documentCardType.Requisites.Add(cardRequisite);
            }
          }
        }
      }

      query = this.GetViewQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var documentCardTypeName = reader["DocumentCardTypeName"] as string;
              DocumentCardType documentCardType;
              if (!components.TryGetValue(documentCardTypeName, out documentCardType))
                continue;

              var view = new View();
              view.Name = reader["Name"] as string;
              view.IsMain = RuYesValue.Equals(reader["IsMain"] as string);
              view.CardForm = reader["CardForm"] as string;
              documentCardType.Views.Add(view);
            }
          }
        }
      }

      return components.Values.ToList();
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос типов карточек электронных документов для 7.7 и выше.
    /// </summary>
    private const string DocumentCardTypeQuery_7_7 = @"
      SELECT
        [Kod] as [Name],
        [Name] as [Title],
        [Sost] as [State],
        [SposNum] as [NumerationMethod],
        [Exprn] as [Events]
      FROM
        [dbo].[MBEDocType]
      ORDER BY
        [Name]";

    /// <summary>
    /// Запрос действий для 7.7 и выше.
    /// </summary>
    private const string ActionQuery_7_7 = @"
      SELECT
        [DocumentCardTypes].[Kod] as [DocumentCardTypeName],
        [Actions].[Kod] as [Name],
        [Actions].[Exprn] as [CalculationText]
      FROM
        [dbo].[MBEDocTypeRecv] [Actions]
      JOIN
        [dbo].[MBEDocType] [DocumentCardTypes]
      ON
        [DocumentCardTypes].[TypeID] = [Actions].[TypeID]
      WHERE
        [Actions].[Razd] = 'К'
      ORDER BY
        [DocumentCardTypeName],
        [Name]";

    /// <summary>
    /// Запрос прикладных методов для 7.55 и выше.
    /// </summary>
    private const string MethodsQuery_7_55 = @"
      select 
        [DocumentCardTypes].[Kod] as [DocumentCardTypeName],
        [Methods].[Name] as [Name],
        [Methods].[Exprn] as [CalculationText]
      from
        [dbo].[MBEDocTypeMethod] [Methods]
        join [dbo].[MBEDocType] [DocumentCardTypes]
          on [DocumentCardTypes].[TypeID] = [Methods].[TypeID]";

    /// <summary>
    /// Запрос параметров прикладных методов для 7.55 и выше.
    /// </summary>
    private const string MethodParamsQuery_7_55 = @"
      select
        [DocumentCardTypes].[Kod] as [DocumentCardTypeName],
        [MethodParameters].[MethodName] as [MethodName],
        [MethodParameters].[Name] as [Name],
        [MethodParameters].[ValueType] as [Type],
        [MethodParameters].[DefaultValue] as [DefaultValue],
        [MethodParameters].[NumStr] as [Number]
      from 
        [dbo].[MBEDocTypeMethodParam] [MethodParameters]
        join [dbo].[MBEDocType] [DocumentCardTypes]
          on [DocumentCardTypes].[TypeID] = [MethodParameters].[TypeID]";

    /// <summary>
    /// Запрос реквизитов для 7.7 и выше.
    /// </summary>
    private const string RequisiteQuery_7_7 = @"
      SELECT
        [DocumentCardTypes].[Kod] as [DocumentCardTypeName],
        [Requisites].[NumRecv] as [Number],
        [Requisites].[Razd] as [Section],
        [Requisites].[Kod] as [Name],
        [Requisites].[IsNull] as [IsRequired],
        [Requisites].[IsHigh] as [IsLeading],
        [Requisites].[Exprn] as [ChangeEvent],
        [Requisites].[InpExprn] as [SelectEvents]
      FROM
        [dbo].[MBEDocTypeRecv] [Requisites]
      JOIN
        [dbo].[MBEDocType] [DocumentCardTypes]
      ON
        [DocumentCardTypes].[TypeID] = [Requisites].[TypeID]
      WHERE
        [Requisites].[Razd] <> 'К'
      ORDER BY
        [DocumentCardTypeName],
        [Name]";

    /// <summary>
    /// Запрос представлений для 7.7 и выше.
    /// </summary>
    private const string ViewQuery_7_7 = @"
      SELECT
        [DocumentCardTypes].[Kod] as [DocumentCardTypeName],
        [Views].[Kod] as [Name],
        [Views].[Main] as [IsMain],
        [Views].[Dfm] as [CardForm]
      FROM
        [dbo].[MBEDocTypeView] [Views]
      JOIN
        [dbo].[MBEDocType] [DocumentCardTypes]
      ON
        [DocumentCardTypes].[TypeID] = [Views].[TypeID]
      ORDER BY
        [DocumentCardTypeName],
        [Name]";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string YesValue = "Y";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string RuYesValue = "Д";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Состояние.
    /// </summary>
    private static readonly Dictionary<string, ComponentState> StateValues
      = new Dictionary<string, ComponentState>
    {
      { "Д", ComponentState.Active },
      { "З", ComponentState.Closed },
    };

    /// <summary>
    /// Способы нумерации.
    /// </summary>
    private static readonly Dictionary<string, NumerationMethod> NumerationMethodValues
      = new Dictionary<string, NumerationMethod>
    {
      { "А", NumerationMethod.StronglyAuto },
      { "Н", NumerationMethod.Auto },
      { "И", NumerationMethod.Manual }
    };

    /// <summary>
    /// Секция.
    /// </summary>
    private static readonly Dictionary<string, RequisiteSection> RequisiteSectionValues
      = new Dictionary<string, RequisiteSection>
    {
      { "Ш", RequisiteSection.Card },
      { "Т", RequisiteSection.Table },
      { "С", RequisiteSection.Table2 },
      { "Р", RequisiteSection.Table3 },
      { "О", RequisiteSection.Table4 },
      { "Н", RequisiteSection.Table5 },
      { "М", RequisiteSection.Table6 },
      { "Q", RequisiteSection.Table7 },
      { "W", RequisiteSection.Table8 },
      { "U", RequisiteSection.Table9 },
      { "R", RequisiteSection.Table10 },
      { "I", RequisiteSection.Table11 },
      { "Y", RequisiteSection.Table12 },
      { "B", RequisiteSection.Table13 },
      { "H", RequisiteSection.Table14 },
      { "L", RequisiteSection.Table15 },
      { "M", RequisiteSection.Table16 },
      { "N", RequisiteSection.Table17 },
      { "P", RequisiteSection.Table18 },
      { "O", RequisiteSection.Table19 },
      { "S", RequisiteSection.Table20 },
      { "T", RequisiteSection.Table21 },
      { "V", RequisiteSection.Table22 },
      { "X", RequisiteSection.Table23 },
      { "Z", RequisiteSection.Table24 }
    };

    /// <summary>
    /// Типы аргументов методов.
    /// </summary>
    private static readonly Dictionary<string, MethodParamType> TypeValues
      = new Dictionary<string, MethodParamType>
    {
      { "V", MethodParamType.Variant },
      { "Д", MethodParamType.Date },
      { "Ч", MethodParamType.Float },
      { "L", MethodParamType.Boolean },
      { "С", MethodParamType.String },
      { "Ц", MethodParamType.Integer }
    };

    #endregion

    #region Методы

    /// <summary>
    /// Запрос типов карточек электронных документов.
    /// </summary>
    public string GetDocumentCardTypeQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return DocumentCardTypeQuery_7_7;
      return null;
    }

    /// <summary>
    /// Запрос действий.
    /// </summary>
    public string GetActionQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ActionQuery_7_7;
      return null;
    }

    /// <summary>
    /// Запрос прикладных методов.
    /// </summary>
    public string GetMethodsQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 55);
      if (platformVersion > minVersion)
        return MethodsQuery_7_55;
      return null;
    }

    /// <summary>
    /// Запрос прикладных методов.
    /// </summary>
    public string GetMethodParamsQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 55);
      if (platformVersion > minVersion)
        return MethodParamsQuery_7_55;
      return null;
    }

    /// <summary>
    /// Запрос реквизитов.
    /// </summary>
    public string GetRequisiteQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return RequisiteQuery_7_7;
      return null;
    }

    /// <summary>
    /// Запрос представлений.
    /// </summary>
    public string GetViewQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ViewQuery_7_7;
      return null;
    }

    #endregion
  }
}
