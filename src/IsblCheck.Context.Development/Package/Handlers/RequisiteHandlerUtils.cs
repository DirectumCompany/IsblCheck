using System;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Утилиты для обработчика реквизитов.
  /// </summary>
  internal static class RequisiteHandlerUtils
  {
    /// <summary>
    /// Секция карточки.
    /// </summary>
    private const string CardSection = "SYSRES_SYSCOMP.REQUISITE_SECTION_CARD";

    /// <summary>
    /// Шаблон секции таблицы.
    /// </summary>
    private const string TableSection = "SYSRES_SYSCOMP.REQUISITE_SECTION_TABLE";

    /// <summary>
    /// Дата.
    /// </summary>
    private const string DateRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_DATE";

    /// <summary>
    /// Дробное число.
    /// </summary>
    private const string FloatRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_FLOAT";

    /// <summary>
    /// Целое число.
    /// </summary>
    private const string IntegerRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_INTEGER";

    /// <summary>
    /// Выбор.
    /// </summary>
    private const string PickRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_PICK";

    /// <summary>
    /// Справочник.
    /// </summary>
    private const string ReferenceRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_REFERENCE";

    /// <summary>
    /// Строка.
    /// </summary>
    private const string StringRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_STRING";

    /// <summary>
    /// Текст.
    /// </summary>
    private const string TextRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_TEXT";

    /// <summary>
    /// Большое целое число.
    /// </summary>
    private const string BigIntegerRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_BIG_INTEGER";

    /// <summary>
    /// Документ.
    /// </summary>
    private const string DocumentRequisiteType = "SYSRES_SYSCOMP.DATA_TYPE_DOCUMENT";

    /// <summary>
    /// Выравнивание по левому краю.
    /// </summary>
    private const string LeftRequisiteFormat = "SYSRES_SYSCOMP.REQUISITE_FORMAT_LEFT";

    /// <summary>
    /// Выравнивание по левому краю.
    /// </summary>
    private const string RightRequisiteFormat = "SYSRES_SYSCOMP.REQUISITE_FORMAT_RIGHT";

    /// <summary>
    /// Дата.
    /// </summary>
    private const string DateRequisiteFormat = "SYSRES_SYSCOMP.REQUISITE_FORMAT_DATE_FULL";

    /// <summary>
    /// Дата и время.
    /// </summary>
    private const string DateTimeRequisiteFormat = "SYSRES_SYSCOMP.REQUISITE_FORMAT_DATE_TIME";

    /// <summary>
    /// Получить секцию реквизита.
    /// </summary>
    /// <param name="valueLocalizeID">Строка локализации.</param>
    /// <returns>Секция реквизита.</returns>
    internal static RequisiteSection GetRequisiteSection(string valueLocalizeID)
    {
      switch (valueLocalizeID)
      {
        case CardSection:
          return RequisiteSection.Card;
        case TableSection:
          return RequisiteSection.Table;
        default:
          var sectionTableNumber = valueLocalizeID.Remove(0, TableSection.Length);
          return (RequisiteSection)Enum.Parse(typeof(RequisiteSection), "Table" + sectionTableNumber);
      }
    }

    /// <summary>
    /// Получить тип реквизита.
    /// </summary>
    /// <param name="valueLocalizeID">Строка локализации.</param>
    /// <returns>Тип реквзита.</returns>
    internal static RequisiteType GetRequisiteType(string valueLocalizeID)
    {
      switch (valueLocalizeID)
      {
        case DateRequisiteType:
          return RequisiteType.Date;
        case FloatRequisiteType:
          return RequisiteType.Float;
        case IntegerRequisiteType:
          return RequisiteType.Integer;
        case PickRequisiteType:
          return RequisiteType.Pick;
        case ReferenceRequisiteType:
          return RequisiteType.Reference;
        case StringRequisiteType:
          return RequisiteType.String;
        case TextRequisiteType:
          return RequisiteType.Text;
        case BigIntegerRequisiteType:
          return RequisiteType.BigInteger;
        case DocumentRequisiteType:
          return RequisiteType.Document;
        default:
          return RequisiteType.String;
      }
    }

    /// <summary>
    /// Получить формат реквизита.
    /// </summary>
    /// <param name="valueLocalizeID">Строка локализации.</param>
    /// <returns>Формат реквизита.</returns>
    internal static RequisiteFormat GetRequisiteFormat(string valueLocalizeID)
    {
      switch (valueLocalizeID)
      {
        case LeftRequisiteFormat:
          return RequisiteFormat.Left;
        case RightRequisiteFormat:
          return RequisiteFormat.Right;
        case DateRequisiteFormat:
          return RequisiteFormat.Date;
        case DateTimeRequisiteFormat:
          return RequisiteFormat.DateAndTime;
        default:
          return RequisiteFormat.None;
      }
    }
  }
}
