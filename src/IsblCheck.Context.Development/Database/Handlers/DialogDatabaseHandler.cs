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
  /// Обработчик диалогов.
  /// </summary>
  internal class DialogDatabaseHandler : IDatabaseHandler<Dialog>
  {
    #region IDatabaseHandler

    public IEnumerable<Dialog> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new Dictionary<string, Dialog>();
      var query = this.GetDialogQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components.Values.ToList();

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var dialog = new Dialog();
            dialog.Name = reader["Name"] as string;
            dialog.Title = reader["Title"] as string;
            dialog.CardForm = reader["CardForm"] as string;

            var eventsValue = reader["Events"] as string;
            if (!string.IsNullOrEmpty(eventsValue))
            {
              var events = EventTextParser.Parse(eventsValue);
              foreach (var @event in events)
                dialog.Events.Add(@event);
            }

            components.Add(dialog.Name, dialog);
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
              var dialogName = reader["DialogName"] as string;
              Dialog dialog;
              if (!components.TryGetValue(dialogName, out dialog))
                continue;

              var method = new Method();
              method.Name = reader["Name"] as string;
              method.CalculationText = reader["CalculationText"] as string ?? string.Empty;

              dialog.Methods.Add(method);
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
              var dialogName = reader["DialogName"] as string;
              Dialog dialog;
              if (!components.TryGetValue(dialogName, out dialog))
                continue;

              var methodName = reader["MethodName"] as string;
              var method = dialog.Methods
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
              var dialogName = reader["DialogName"] as string;
              Dialog dialog;
              if (!components.TryGetValue(dialogName, out dialog))
                continue;

              var action = new ActionWithHandler();
              action.Name = reader["Name"] as string;

              var calculationText = reader["CalculationText"] as string;
              var executionHandler = dialog.Methods
                .FirstOrDefault(m => m.Name == calculationText);
              if (executionHandler != null)
              {
                action.ExecutionHandler = executionHandler;
              }
              else
              {
                action.CalculationText = calculationText ?? string.Empty;
              }

              dialog.Actions.Add(action);
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
              var dialogName = reader["DialogName"] as string;
              Dialog dialog;
              if (!components.TryGetValue(dialogName, out dialog))
                continue;

              var cardRequisite = new CardRequisite();
              cardRequisite.Number = (int)reader["Number"];

              var sectionValue = reader["Section"] as string;
              if (sectionValue != null &&
                RequisiteSectionValues.ContainsKey(sectionValue))
                cardRequisite.Section = RequisiteSectionValues[sectionValue];

              cardRequisite.Name = reader["Name"] as string;
              cardRequisite.IsRequired = YesValue.Equals(reader["IsRequired"] as string);
              cardRequisite.IsLastValueSaved = YesValue.Equals(reader["IsLastValueSaved"] as string);
              cardRequisite.IsSelectionConstrained = YesValue.Equals(reader["IsSelectionConstrained"] as string);

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

              dialog.Requisites.Add(cardRequisite);
            }
          }
        }
      }

      return components.Values.ToList();
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос диалогов для 7.15 и выше.
    /// </summary>
    private const string DialogQuery_7_15 = @"
      SELECT
        [Code] as [Name],
        [Name] as [Title],
        [Dfm] as [CardForm],
        [EventHandlersText] as [Events]
      FROM
        [dbo].[SBDialog]
      ORDER BY
        [Name]";

    /// <summary>
    /// Запрос действий для 7.15 и выше.
    /// </summary>
    private const string ActionQuery_7_15 = @"
      SELECT
        [Dialogs].[Code] as [DialogName],
        [Actions].[Code] as [Name],
        [Actions].[ChangeEventHandlerText] as [CalculationText]
      FROM
        [dbo].[SBDialogRequisiteLink] [Actions]
      JOIN
        [dbo].[SBDialog] [Dialogs]
      ON
        [Dialogs].[XRecID] = [Actions].[DialogID]
      WHERE
        [Actions].[Section] = 'A'
      ORDER BY
        [DialogName],
        [Name]";

    /// <summary>
    /// Запрос прикладных методов для 7.55 и выше.
    /// </summary>
    private const string MethodsQuery_7_55 = @"
      select 
        [Dialogs].[Code] as [DialogName],
        [Methods].[Name] as [Name],
        [Methods].[Exprn] as [CalculationText]
      from
        [dbo].[SBDialogMethod] [Methods]
        join [dbo].[SBDialog] [Dialogs]
          on [Dialogs].[XRecID] = [Methods].[DialogID]";

    /// <summary>
    /// Запрос параметров прикладных методов для 7.55 и выше.
    /// </summary>
    private const string MethodParamsQuery_7_55 = @"
      select
        [Dialogs].[Code] as [DialogName],
        [MethodParameters].[MethodName] as [MethodName],
        [MethodParameters].[Name] as [Name],
        [MethodParameters].[ValueType] as [Type],
        [MethodParameters].[DefaultValue] as [DefaultValue],
        [MethodParameters].[NumStr] as [Number]
      from 
        [dbo].[SBDialogMethodParam] [MethodParameters]
        join [dbo].[SBDialog] [Dialogs]
          on [Dialogs].[XRecID] = [MethodParameters].[DialogID]";

    /// <summary>
    /// Запрос реквизитов для 7.7 и выше.
    /// </summary>
    private const string RequisiteQuery_7_15 = @"
      SELECT
        [Dialogs].[Code] as [DialogName],
        [Requisites].[RequisiteNumber] as [Number],
        [Requisites].[Section] as [Section],
        [Requisites].[Code] as [Name],
        [Requisites].[Required] as [IsRequired],
        [Requisites].[SaveLastValue] as [IsLastValueSaved],
        [Requisites].[SelectionConstrained] as [IsSelectionConstrained],
        [Requisites].[ChangeEventHandlerText] as [ChangeEvent],
        [Requisites].[LookupEventHandlerText] as [SelectEvents]
      FROM
        [dbo].[SBDialogRequisiteLink] [Requisites]
      JOIN
        [dbo].[SBDialog] [Dialogs]
      ON
        [Dialogs].[XRecID] = [Requisites].[DialogID]
      WHERE
        [Requisites].[Section] <> 'A'
      ORDER BY
        [DialogName],
        [Name]";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string YesValue = "Y";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Секция.
    /// </summary>
    private static readonly Dictionary<string, RequisiteSection> RequisiteSectionValues
      = new Dictionary<string, RequisiteSection>
    {
      { "C", RequisiteSection.Card },
      { "D", RequisiteSection.Table },
      { "E", RequisiteSection.Table2 },
      { "F", RequisiteSection.Table3 },
      { "G", RequisiteSection.Table4 },
      { "J", RequisiteSection.Table5 },
      { "K", RequisiteSection.Table6 },
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
    /// Запрос диалогов.
    /// </summary>
    public string GetDialogQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 15);
      if (platformVersion > minVersion)
        return DialogQuery_7_15;
      return null;
    }

    /// <summary>
    /// Запрос действий.
    /// </summary>
    public string GetActionQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 15);
      if (platformVersion > minVersion)
        return ActionQuery_7_15;
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
      var minVersion = new Version(7, 15);
      if (platformVersion > minVersion)
        return RequisiteQuery_7_15;
      return null;
    }

    #endregion
  }
}
