using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик типов справочников.
  /// </summary>
  internal class ReferenceTypeDatabaseHandler : IDatabaseHandler<ReferenceType>
  {
    #region IDatabaseHandler

    public IEnumerable<ReferenceType> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new Dictionary<string, ReferenceType>();
      var query = this.GetReferenceTypeQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components.Values.ToList();

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var referenceType = new ReferenceType
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string
            };

            if (reader["State"] is string stateValue &&
              StateValues.ContainsKey(stateValue))
              referenceType.State = StateValues[stateValue];

            if (reader["NumerationMethod"] is string numerationMethodValue &&
              NumerationMethodValues.ContainsKey(numerationMethodValue))
              referenceType.NumerationMethod = NumerationMethodValues[numerationMethodValue];

            if (reader["DisplayRequisite"] is string displayRequisiteValue &&
              DisplayRequisiteValues.ContainsKey(displayRequisiteValue))
              referenceType.DisplayRequisite = DisplayRequisiteValues[displayRequisiteValue];

            referenceType.LeadingReference = reader["LeadingReference"] as string;
            referenceType.HasLongRecordNames = YesValue.Equals(reader["HasLongRecordNames"] as string);
            referenceType.HasUniqueRecordNames = YesValue.Equals(reader["HasUniqueRecordNames"] as string);

            var eventsValue = reader["Events"] as string;
            if (!string.IsNullOrEmpty(eventsValue))
            {
              var events = EventTextParser.Parse(eventsValue);
              foreach (var @event in events)
                referenceType.Events.Add(@event);
            }

            components.Add(referenceType.Name, referenceType);
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
              var referenceTypeName = reader["ReferenceTypeName"] as string;
              if (!components.TryGetValue(referenceTypeName, out ReferenceType referenceType))
                continue;

              var method = new Method
              {
                Name = reader["Name"] as string,
                CalculationText = reader["CalculationText"] as string ?? string.Empty
              };

              referenceType.Methods.Add(method);
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
              var referenceTypeName = reader["ReferenceTypeName"] as string;
              if (!components.TryGetValue(referenceTypeName, out ReferenceType referenceType))
                continue;

              var methodName = reader["MethodName"] as string;
              var method = referenceType.Methods
                .FirstOrDefault(m => m.Name == methodName);
              if (method == null)
                continue;

              var parameter = new MethodParam
              {
                Number = (int) reader["Number"],
                Name = reader["Name"] as string
              };

              var typeValue = reader["Type"] as string;
              if (TypeValues.TryGetValue(typeValue, out MethodParamType methodParamType))
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
              var referenceTypeName = reader["ReferenceTypeName"] as string;
              if (!components.TryGetValue(referenceTypeName, out ReferenceType referenceType))
                continue;

              var action = new ActionWithHandler {Name = reader["Name"] as string};

              var calculationText = reader["CalculationText"] as string;
              var executionHandler = referenceType.Methods
                .FirstOrDefault(m => m.Name == calculationText);
              if (executionHandler != null)
              {
                action.ExecutionHandler = executionHandler;
              }
              else
              {
                action.CalculationText = calculationText ?? string.Empty;
              }

              referenceType.Actions.Add(action);
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
              var referenceTypeName = reader["ReferenceTypeName"] as string;
              if (!components.TryGetValue(referenceTypeName, out ReferenceType referenceType))
                continue;

              var cardRequisite = new CardRequisite {Number = (int) reader["Number"]};

              if (reader["Section"] is string sectionValue &&
                RequisiteSectionValues.ContainsKey(sectionValue))
                cardRequisite.Section = RequisiteSectionValues[sectionValue];

              cardRequisite.Name = reader["Name"] as string;
              cardRequisite.IsRequired = RuYesValue.Equals(reader["IsRequired"] as string);
              cardRequisite.IsFilter = RuYesValue.Equals(reader["IsFilter"] as string);
              cardRequisite.IsLeading = RuYesValue.Equals(reader["IsLeading"] as string);
              cardRequisite.IsControl = RuYesValue.Equals(reader["IsControl"] as string);

              var changeEventValue = reader["ChangeEvent"] as string;
              if (!string.IsNullOrEmpty(changeEventValue))
              {
                var @event = new Event
                {
                  EventType = EventType.Change,
                  CalculationText = changeEventValue
                };
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

              referenceType.Requisites.Add(cardRequisite);
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
              var referenceTypeName = reader["ReferenceTypeName"] as string;
              if (!components.TryGetValue(referenceTypeName, out ReferenceType referenceType))
                continue;

              var view = new View
              {
                Name = reader["Name"] as string,
                IsMain = RuYesValue.Equals(reader["IsMain"] as string),
                ListForm = reader["ListForm"] as string,
                CardForm = reader["CardForm"] as string
              };
              referenceType.Views.Add(view);

              referenceType.Views.Add(view);
            }
          }
        }
      }

      return components.Values.ToList();
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос справочников для 7.7 и до 7.9.
    /// </summary>
    private const string ReferenceTypeQuery_7_7 = @"
      SELECT
        [ReferenceTypes].[Kod] as [Name],
        [ReferenceTypes].[Name] as [Title],
        [ReferenceTypes].[Sost] as [State],
        [ReferenceTypes].[SposNum] as [NumerationMethod],
        [ReferenceTypes].[TypeConcept] as [DisplayRequisite],
        [LeadingReferenceTypes].[Kod] as [LeadingReference],
        'N' as [HasLongRecordNames],
        'N' as [HasUniqueRecordNames],
        [ReferenceTypes].[Exprn] as [Events]
      FROM
        [dbo].[MBVidAn] [ReferenceTypes]
      LEFT JOIN
        [dbo].[MBVidAn] [LeadingReferenceTypes]
      ON
        [LeadingReferenceTypes].[Vid] = [ReferenceTypes].[HighLvl]
      ORDER BY
        [Name]";

    /// <summary>
    /// Запрос справочников для 7.9 и выше.
    /// </summary>
    private const string ReferenceTypeQuery_7_9 = @"
      SELECT
        [ReferenceTypes].[Kod] as [Name],
        [ReferenceTypes].[Name] as [Title],
        [ReferenceTypes].[Sost] as [State],
        [ReferenceTypes].[SposNum] as [NumerationMethod],
        [ReferenceTypes].[TypeConcept] as [DisplayRequisite],
        [LeadingReferenceTypes].[Kod] as [LeadingReference],
        [ReferenceTypes].[IsNameLong] as [HasLongRecordNames],
        [ReferenceTypes].[IsNameUnique] as [HasUniqueRecordNames],
        [ReferenceTypes].[Exprn] as [Events]
      FROM
        [dbo].[MBVidAn] [ReferenceTypes]
      LEFT JOIN
        [dbo].[MBVidAn] [LeadingReferenceTypes]
      ON
        [LeadingReferenceTypes].[Vid] = [ReferenceTypes].[HighLvl]
      ORDER BY
        [Name]";

    /// <summary>
    /// Запрос действий для 7.7 и выше.
    /// </summary>
    private const string ActionQuery_7_7 = @"
      SELECT
        [ReferenceTypes].[Kod] as [ReferenceTypeName],
        [Actions].[Kod] as [Name],
        [Actions].[Exprn] as [CalculationText]
      FROM
        [dbo].[MBVidAnRecv] [Actions]
      JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [Actions].[Vid]
      WHERE
        [Actions].[Razd] = 'К'
      ORDER BY
        [ReferenceTypeName],
        [Name]";

    /// <summary>
    /// Запрос прикладных методов для 7.55 и выше.
    /// </summary>
    private const string MethodsQuery_7_55 = @"
      select 
        [ReferenceTypes].[Kod] as [ReferenceTypeName],
        [Methods].[Name] as [Name],
        [Methods].[Exprn] as [CalculationText]
      from
        [dbo].[MBVidAnMethod] [Methods]
        join [dbo].[MBVidAn] [ReferenceTypes]
          on [ReferenceTypes].[Vid] = [Methods].[Vid]";

    /// <summary>
    /// Запрос параметров прикладных методов для 7.55 и выше.
    /// </summary>
    private const string MethodParamsQuery_7_55 = @"
      select
        [ReferenceTypes].[Kod] as [ReferenceTypeName],
        [MethodParameters].[MethodName] as [MethodName],
        [MethodParameters].[Name] as [Name],
        [MethodParameters].[ValueType] as [Type],
        [MethodParameters].[DefaultValue] as [DefaultValue],
        [MethodParameters].[NumStr] as [Number]
      from 
        [dbo].[MBVidAnMethodParam] [MethodParameters]
        join [dbo].[MBVidAn] [ReferenceTypes]
          on [ReferenceTypes].[Vid] = [MethodParameters].[Vid]";

    /// <summary>
    /// Запрос реквизитов для 7.7 и выше.
    /// </summary>
    private const string RequisiteQuery_7_7 = @"
      SELECT
        [ReferenceTypes].[Kod] as [ReferenceTypeName],
        [Requisites].[NumRecv] as [Number],
        [Requisites].[Razd] as [Section],
        [Requisites].[Kod] as [Name],
        [Requisites].[IsNull] as [IsRequired],
        [Requisites].[IsKlass] as [IsFilter],
        [Requisites].[IsHigh] as [IsLeading],
        [Requisites].[IsSources] as [IsControl],
        [Requisites].[Exprn] as [ChangeEvent],
        [Requisites].[InpExprn] as [SelectEvents]
      FROM
        [dbo].[MBVidAnRecv] [Requisites]
      JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [Requisites].[Vid]
      WHERE
        [Requisites].[Razd] <> 'К'
      ORDER BY
        [ReferenceTypeName],
        [Name]";

    /// <summary>
    /// Запрос представлений для 7.7 и до 7.15.
    /// </summary>
    private const string ViewQuery_7_7 = @"
      SELECT
        [ReferenceTypes].[Kod] as [ReferenceTypeName],
        [Views].[Kod] as [Name],
        [Views].[Main] as [IsMain],
        NULL as [ListForm],
        [Views].[Dfm] as [CardForm]
      FROM
        [dbo].[MBVidAnView] [Views]
      JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [Views].[Vid]
      ORDER BY
        [ReferenceTypeName],
        [Name]";

    /// <summary>
    /// Запрос представлений для 7.15 и выше.
    /// </summary>
    private const string ViewQuery_7_15 = @"
      SELECT
        [ReferenceTypes].[Kod] as [ReferenceTypeName],
        [Views].[Kod] as [Name],
        [Views].[Main] as [IsMain],
        [Views].[ViewDfm] as [ListForm],
        [Views].[Dfm] as [CardForm]
      FROM
        [dbo].[MBVidAnView] [Views]
      JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [Views].[Vid]
      ORDER BY
        [ReferenceTypeName],
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
    /// Способы нумерации.
    /// </summary>
    private static readonly Dictionary<string, DisplayRequisite> DisplayRequisiteValues
      = new Dictionary<string, DisplayRequisite>
    {
      { "К", DisplayRequisite.Code },
      { "Н", DisplayRequisite.Name }
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
    /// Запрос типов справочников.
    /// </summary>
    public string GetReferenceTypeQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 9);
      if (platformVersion > minVersion)
        return ReferenceTypeQuery_7_9;

      minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ReferenceTypeQuery_7_7;

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
      var minVersion = new Version(7, 15);
      if (platformVersion > minVersion)
        return ViewQuery_7_15;

      minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ViewQuery_7_7;

      return null;
    }

    #endregion
  }
}
