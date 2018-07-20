using System.Collections.Generic;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Core.Context.Application
{
  /// <summary>
  /// Контекст приложения.
  /// </summary>
  public interface IApplicationContext
  {
    /// <summary>
    /// Константы.
    /// </summary>
    IReadOnlyDictionary<string, object> Constants { get; }

    /// <summary>
    /// Перечисления.
    /// </summary>
    IReadOnlyDictionary<string, int> Enums { get; }

    /// <summary>
    /// Предопределенные переменные.
    /// </summary>
    IReadOnlyList<string> PredefinedVariables { get; }

    /// <summary>
    /// Предопределённые функции IS-Builder.
    /// </summary>
    IReadOnlyList<Function> Functions { get; }

    /// <summary>
    /// Проверить, существует ли системная константа с именем name.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Признак существования.</returns>
    bool IsExistConstant(string name);

    /// <summary>
    /// Получить значение константы с именем name.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы.</returns>
    string GetConstantValue(string name);

    /// <summary>
    /// Проверить, существует ли предопределенная переменная с именем name.
    /// </summary>
    /// <param name="name">Имя переменной.</param>
    /// <returns>Признак существования.</returns>
    bool IsExistPredefinedVariable(string name);

    /// <summary>
    /// Проверить, существует ли переменная перечисления с именем name.
    /// </summary>
    /// <param name="name">Имя переменной.</param>
    /// <returns>Признак существования.</returns>
    bool IsExistEnumValue(string name);

    /// <summary>
    /// Проверить, существует ли такой системный справочнник.
    /// </summary>
    /// <param name="name">Название справочника.</param>
    /// <param name="withOldReference">Учитывать также старые наименования справочников.</param>
    /// <returns>Возвращает false, если справочника не существует.</returns>
    bool IsExistsSysReference(string name, bool withOldReference);
  }
}
