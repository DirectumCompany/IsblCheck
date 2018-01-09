using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IsblCheck.Context.Development.Utils
{
  /// <summary>
  /// Парсер текста событий.
  /// </summary>
  internal static class EventTextParser
  {
    #region Внутренние классы

    /// <summary>
    /// Префиксы событий c гуидами.
    /// </summary>
    private static class EventGuidPrefix
    {
      public const string DataSet = "DATASET{9AFC8FC7-30C4-4076-9076-6E09A49B791C}";
      public const string Card = "CARD{2147B5A6-496E-4EFF-88D9-78970D889F1F}";
      public const string Operation = "OPERATION{C6CE6EDC-3645-4BBC-B00F-587BD2A54B4C}";
      public const string Form = "FORM{B28D55C1-651A-46C9-AD4E-50E73EF213A8}";
      public const string ListForm = "LISTFORM{EF850CF0-3135-4D1F-8726-74F95C4D08C7}";
      public const string Table = "TABLE{D402E843-74B2-4DC1-BFFD-DE677B48452C}";
      public const string Dialog = "DIALOG{3AA220D8-D906-4914-8586-F534A4C3767E}";
      public const string Requisite = "REQUISITE{CA900538-FC82-47D2-B574-001875901D2D}";
    }

    /// <summary>
    /// Префиксы событий без гуидов.
    /// </summary>
    private static class EventPrefix
    {
      public const string DataSet = "DATASET";
      public const string Card = "CARD";
      public const string Operation = "OPERATION";
      public const string Form = "FORM";
      public const string ListForm = "LISTFORM";
      public const string Table = "TABLE";
      public const string Dialog = "DIALOG";
      public const string Requisite = "REQUISITE";
    }

    /// <summary>
    /// Старые префиксы событий.
    /// </summary>
    private static class EventOldPrefix
    {
      public const string DataSet = "НАБОР ДАННЫХ";
      public const string Card = "КАРТОЧКА";
      public const string Operation = "ОПЕРАЦИЯ";
      public const string Form = "ФОРМА";
      public const string Table = "ТАБЛИЦА";
    }

    /// <summary>
    /// Постфиксы событий.
    /// </summary>
    private static class EventPostfix
    {
      public const string Open = "OPEN";
      public const string Close = "CLOSE";
      public const string Execution = "EXECUTION";
      public const string BeforeInsert = "BEFORE_INSERT";
      public const string AfterInsert = "AFTER_INSERT";
      public const string ValidUpdate = "VALID_UPDATE";
      public const string BeforeUpdate = "BEFORE_UPDATE";
      public const string AfterUpdate = "AFTER_UPDATE";
      public const string ValidDelete = "VALID_DELETE";
      public const string BeforeDelete = "BEFORE_DELETE";
      public const string AfterDelete = "AFTER_DELETE";
      public const string BeforeCancel = "BEFORE_CANCEL";
      public const string AfterCancel = "AFTER_CANCEL";
      public const string Show = "SHOW";
      public const string Hide = "HIDE";
      public const string Create = "CREATE";
      public const string ValidCloseWithResult = "VALID_CLOSE_WITH_RESULT";
      public const string CloseWithResult = "CLOSE_WITH_RESULT";
      public const string DialogShow = "DIALOG_SHOW";
      public const string DialogHide = "DIALOG_HIDE";
      public const string Select = "SELECT";
      public const string BeforeSelect = "BEFORE_SELECT";
      public const string AfterSelect = "AFTER_SELECT";
    }

    /// <summary>
    /// Постфиксы событий.
    /// </summary>
    private static class EventOldPostfix
    {
      public const string Open = "ОТКРЫТИЕ";
      public const string Close = "ЗАКРЫТИЕ";
      public const string Execution = "ВЫПОЛНЕНИЕ";
      public const string BeforeInsert = "ДОБАВЛЕНИЕ ДО";
      public const string AfterInsert = "ДОБАВЛЕНИЕ ПОСЛЕ";
      public const string ValidUpdate = "СОХРАНЕНИЕ ВОЗМОЖНОСТЬ";
      public const string BeforeUpdate = "СОХРАНЕНИЕ ДО";
      public const string AfterUpdate = "СОХРАНЕНИЕ ПОСЛЕ";
      public const string ValidDelete = "УДАЛЕНИЕ ВОЗМОЖНОСТЬ";
      public const string BeforeDelete = "УДАЛЕНИЕ ДО";
      public const string AfterDelete = "УДАЛЕНИЕ ПОСЛЕ";
      public const string BeforeCancel = "ОТМЕНА ДО";
      public const string AfterCancel = "ОТМЕНА ПОСЛЕ";
      public const string Show = "ПОКАЗ";
      public const string Hide = "СКРЫТИЕ";
    }

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Имена событий.
    /// </summary>
    private static Dictionary<string, EventType> eventNames = new Dictionary<string, EventType>();

    #endregion

    #region Методы

    /// <summary>
    /// Выполнить парсинг значения реквизита события.
    /// </summary>
    /// <param name="source">Значения реквизита события.</param>
    /// <returns>События.</returns>
    public static IList<Event> Parse(string source)
    {
      var result = new List<Event>();
      if (string.IsNullOrEmpty(source))
        return result;

      using (var reader = new StringReader(source))
      {
        var newLine = reader.ReadLine();
        var @event = new Event();
        @event.EventType = EventType.Unknown;
        var eventText = new StringBuilder();
        if (eventNames.ContainsKey(newLine))
          @event.EventType = eventNames[newLine];
        else
          eventText.AppendFormat("{0}\r\n", newLine);
        
        while (reader.Peek() != -1)
        {
          newLine = reader.ReadLine();
          if (eventNames.ContainsKey(newLine))
          {
            @event.CalculationText = eventText.ToString();
            result.Add(@event);

            @event = new Event();
            @event.EventType = eventNames[newLine];
            eventText = eventText.Clear();
          }
          else
            eventText = eventText.AppendFormat("{0}\r\n", newLine);
        }

        @event.CalculationText = eventText.ToString();
        result.Add(@event);
      }
      return result;
    }

    /// <summary>
    /// Сгенерировать имена событий c Guid.
    /// </summary>
    private static void GenerateGuidEventNames()
    {
      // Датасет
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.DataSet, EventPostfix.Open), EventType.OnDataSetOpen);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.DataSet, EventPostfix.Close), EventType.OnDataSetClose);

      // Запись
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.Open), EventType.OnOpenRecord);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.Close), EventType.OnCloseRecord);

      // Выполнение
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Operation, EventPostfix.Execution), EventType.OnUpdateRatifiedRecord);

      // Карточка
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.BeforeInsert), EventType.BeforeInsert);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.AfterInsert), EventType.AfterInsert);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.ValidUpdate), EventType.OnValidUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.BeforeUpdate), EventType.BeforeUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.AfterUpdate), EventType.AfterUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.ValidDelete), EventType.OnValidDelete);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.BeforeDelete), EventType.BeforeDelete);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.AfterDelete), EventType.AfterDelete);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.BeforeCancel), EventType.BeforeCancel);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Card, EventPostfix.AfterCancel), EventType.AfterCancel);

      // Форма карточки
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Form, EventPostfix.Show), EventType.FormShow);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Form, EventPostfix.Hide), EventType.FormHide);

      // Форма списка
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.ListForm, EventPostfix.Show), EventType.ListFormShow);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.ListForm, EventPostfix.Hide), EventType.ListFormHide);

      // Диалог
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Dialog, EventPostfix.Create), EventType.Create);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Dialog, EventPostfix.ValidCloseWithResult), EventType.OnValidCloseWithResult);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Dialog, EventPostfix.CloseWithResult), EventType.CloseWithResult);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Form, EventPostfix.DialogShow), EventType.DialogShow);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Form, EventPostfix.DialogHide), EventType.DialogHide);

      // Таблица
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Table, EventPostfix.BeforeInsert), EventType.TableBeforeInsert);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Table, EventPostfix.AfterInsert), EventType.TableAfterInsert);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Table, EventPostfix.BeforeDelete), EventType.TableBeforeDelete);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Table, EventPostfix.AfterDelete), EventType.TableAfterDelete);

      // Таблицы 2-24
      for (var i = 2; i <= 24; i++)
      {
        var beforeInsertEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}BeforeInsert", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventGuidPrefix.Table, i, EventPostfix.BeforeInsert), beforeInsertEnumValue);

        var afterInsertEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}AfterInsert", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventGuidPrefix.Table, i, EventPostfix.AfterInsert), afterInsertEnumValue);

        var beforeDeleteEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}BeforeDelete", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventGuidPrefix.Table, i, EventPostfix.BeforeDelete), beforeDeleteEnumValue);

        var afterDeleteEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}AfterDelete", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventGuidPrefix.Table, i, EventPostfix.AfterDelete), afterDeleteEnumValue);
      }

      // Реквизит
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Requisite, EventPostfix.Select), EventType.Select);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Requisite, EventPostfix.BeforeSelect), EventType.BeforeSelect);
      eventNames.Add(string.Format("{0}.{1}", EventGuidPrefix.Requisite, EventPostfix.AfterSelect), EventType.AfterSelect);
    }

    /// <summary>
    /// Сгенерировать имена событий без Guid.
    /// </summary>
    private static void GenerateEventNames()
    {
      // Датасет
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.DataSet, EventPostfix.Open), EventType.OnDataSetOpen);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.DataSet, EventPostfix.Close), EventType.OnDataSetClose);

      // Запись
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.Open), EventType.OnOpenRecord);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.Close), EventType.OnCloseRecord);

      // Выполнение.
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Operation, EventPostfix.Execution), EventType.OnUpdateRatifiedRecord);

      // Карточка
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.BeforeInsert), EventType.BeforeInsert);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.AfterInsert), EventType.AfterInsert);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.ValidUpdate), EventType.OnValidUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.BeforeUpdate), EventType.BeforeUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.AfterUpdate), EventType.AfterUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.ValidDelete), EventType.OnValidDelete);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.BeforeDelete), EventType.BeforeDelete);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.AfterDelete), EventType.AfterDelete);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.BeforeCancel), EventType.BeforeCancel);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Card, EventPostfix.AfterCancel), EventType.AfterCancel);

      // Форма карточки
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Form, EventPostfix.Show), EventType.FormShow);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Form, EventPostfix.Hide), EventType.FormHide);

      // Форма списка
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.ListForm, EventPostfix.Show), EventType.ListFormShow);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.ListForm, EventPostfix.Hide), EventType.ListFormHide);

      // Диалог
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Dialog, EventPostfix.Create), EventType.Create);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Dialog, EventPostfix.ValidCloseWithResult), EventType.OnValidCloseWithResult);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Dialog, EventPostfix.CloseWithResult), EventType.CloseWithResult);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Form, EventPostfix.DialogShow), EventType.DialogShow);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Form, EventPostfix.DialogHide), EventType.DialogHide);

      // Таблица
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Table, EventPostfix.BeforeInsert), EventType.TableBeforeInsert);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Table, EventPostfix.AfterInsert), EventType.TableAfterInsert);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Table, EventPostfix.BeforeDelete), EventType.TableBeforeDelete);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Table, EventPostfix.AfterDelete), EventType.TableAfterDelete);

      // Таблицы 2-24
      for (var i = 2; i <= 24; i++)
      {
        var beforeInsertEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}BeforeInsert", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventPrefix.Table, i, EventPostfix.BeforeInsert), beforeInsertEnumValue);

        var afterInsertEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}AfterInsert", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventPrefix.Table, i, EventPostfix.AfterInsert), afterInsertEnumValue);

        var beforeDeleteEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}BeforeDelete", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventPrefix.Table, i, EventPostfix.BeforeDelete), beforeDeleteEnumValue);

        var afterDeleteEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}AfterDelete", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventPrefix.Table, i, EventPostfix.AfterDelete), afterDeleteEnumValue);
      }

      // Реквизит
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Requisite, EventPostfix.Select), EventType.Select);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Requisite, EventPostfix.BeforeSelect), EventType.BeforeSelect);
      eventNames.Add(string.Format("{0}.{1}", EventPrefix.Requisite, EventPostfix.AfterSelect), EventType.AfterSelect);
    }

    /// <summary>
    /// Сгенерировать старые имена событий.
    /// </summary>
    private static void GenerateOldEventNames()
    {
      // Датасет
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.DataSet, EventOldPostfix.Open), EventType.OnDataSetOpen);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.DataSet, EventOldPostfix.Close), EventType.OnDataSetClose);

      // Запись
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.Open), EventType.OnOpenRecord);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.Close), EventType.OnCloseRecord);

      // Выполнение.
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Operation, EventOldPostfix.Execution), EventType.OnUpdateRatifiedRecord);

      // Карточка
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.BeforeInsert), EventType.BeforeInsert);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.AfterInsert), EventType.AfterInsert);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.ValidUpdate), EventType.OnValidUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.BeforeUpdate), EventType.BeforeUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.AfterUpdate), EventType.AfterUpdate);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.ValidDelete), EventType.OnValidDelete);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.BeforeDelete), EventType.BeforeDelete);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.AfterDelete), EventType.AfterDelete);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.BeforeCancel), EventType.BeforeCancel);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Card, EventOldPostfix.AfterCancel), EventType.AfterCancel);

      // Форма карточки
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Form, EventOldPostfix.Show), EventType.FormShow);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Form, EventOldPostfix.Hide), EventType.FormHide);

      // Таблица
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Table, EventOldPostfix.BeforeInsert), EventType.TableBeforeInsert);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Table, EventOldPostfix.AfterInsert), EventType.TableAfterInsert);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Table, EventOldPostfix.BeforeDelete), EventType.TableBeforeDelete);
      eventNames.Add(string.Format("{0}.{1}", EventOldPrefix.Table, EventOldPostfix.AfterDelete), EventType.TableAfterDelete);

      // Таблицы 2-24
      for (var i = 2; i <= 24; i++)
      {
        var beforeInsertEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}BeforeInsert", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventOldPrefix.Table, i, EventOldPostfix.BeforeInsert), beforeInsertEnumValue);

        var afterInsertEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}AfterInsert", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventOldPrefix.Table, i, EventOldPostfix.AfterInsert), afterInsertEnumValue);

        var beforeDeleteEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}BeforeDelete", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventOldPrefix.Table, i, EventOldPostfix.BeforeDelete), beforeDeleteEnumValue);

        var afterDeleteEnumValue = (EventType)Enum.Parse(typeof(EventType), string.Format("Table{0}AfterDelete", i));
        eventNames.Add(string.Format("{0}{1}.{2}", EventOldPrefix.Table, i, EventOldPostfix.AfterDelete), afterDeleteEnumValue);
      }
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static EventTextParser()
    {
      GenerateGuidEventNames();
      GenerateEventNames();
      GenerateOldEventNames();
    }

    #endregion
  }
}
