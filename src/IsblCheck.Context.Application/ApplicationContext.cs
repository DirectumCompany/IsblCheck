using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IsblCheck.Context.Application
{
  /// <summary>
  /// Контекст приложения.
  /// </summary>
  internal class ApplicationContext : IApplicationContext
  {
    #region IApplicationContext

    public IReadOnlyDictionary<string, object> Constants { get; private set; }

    public IReadOnlyDictionary<string, int> Enums { get; private set; }

    public IReadOnlyList<Function> Functions { get; private set; }

    public IReadOnlyList<string> PredefinedVariables { get { return predefindedVariable; } }

    #endregion


    // TODO: Список предопределенных переменных не привязан к контексту isbl-вычислений.
    // Некоторые переменные имеют смысл только в определенных вычислениях. В остальных случаях это обычные переменные.

    /// <summary>
    /// Список предопределенных переменных.
    /// </summary>
    private static readonly List<string> predefindedVariable = new List<string>
    {
      "AltState",
      "Application",
      "CallType",
      "ComponentTokens",
      "CreatedJobs",
      "CreatedNotices",
      "ControlState",
      "DialogResult",
      "Dialogs",
      "EDocuments",
      "EDocumentVersionSource",
      "Folders",
      "GlobalIDs",
      "Job",
      "Jobs",
      "InputValue",
      "LookUpReference",
      "LookUpRequisiteNames",
      "LookUpSearch",
      "Object",
      "ParentComponent",
      "Processes",
      "References",
      "Requisite",
      "ReportName",
      "Reports",
      "Result",
      "Scripts",
      "Searches",
      "SelectedAttachments",
      "SelectedItems",
      "SelectMode",
      "Sender",
      "ServerEvents",
      "ServiceFactory",
      "ShiftState",
      "SubTask",
      "SystemDialogs",
      "Tasks",
      "Wizard",
      "Wizards",
      "Work",
      "ВызовСпособ",
      "ИмяОтчета",
      "РеквЗнач"
    };

    /// <summary>
    /// Список устаревших наименований справочников.
    /// </summary>
    private static readonly HashSet<string> oldReferenceNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
      "ВСЕ_ДОСТУПНЫЕ_КОМПОНЕНТЫ",
      "ВСЕ_РЕПЛИЦИРУЕМЫЕ_КОМПОНЕНТЫ",
      "ВЫБОР_SQL",
      "ГРУППЫ_ПОЛЬЗОВАТЕЛЕЙ",
      "ГРУППЫ_ФУНКЦИЙ",
      "ЖУРНАЛ_СЕАНСОВ_РЕПЛИКАЦИИ",
      "ЗАДАНИЯ",
      "ЗАДАНИЯ_КОНТРОЛЬ",
      "ЗАДАЧИ",
      "ИСТОРИЯ_ДОКУМЕНТОВ",
      "ИСТОРИЯ_ЗАДАНИЙ",
      "ИСТОРИЯ_ЗАДАЧ",
      "ИСТОРИЯ_ЗАПИСИ",
      "ИСТОРИЯ_ПАПОК",
      "КОМПОНЕНТЫ",
      "КОНСТАНТЫ",
      "КОНФЛИКТЫ_НАСТРОЙКИ_ФИЛЬТРАТОРОВ",
      "МОДУЛИ",
      "ОТЧЕТЫ",
      "ПАПКИ",
      "ПАРАМЕТРЫ_ФУНКЦИЙ",
      "ПОЛЬЗОВАТЕЛИ",
      "ПОЛЬЗОВАТЕЛЬСКИЕ_СТАТУСЫ",
      "ПРАВА_ЗАДАЧ",
      "ПРЕДСТАВЛЕНИЯ_СПРАВОЧНИКА",
      "ПРИЛОЖЕНИЯ_ПРОСМОТРЩИКИ",
      "ПРОТОКОЛ_ПЕРЕДАЧИ_ДАННЫХ",
      "ПРОТОКОЛ_ПРИЕМА_ДАННЫХ",
      "РЕКВИЗИТЫ_СПРАВОЧНИКА",
      "РЕКВИЗИТЫ_СПРАВОЧНИКОВ",
      "СПРАВОЧНИК_ЗАДАЧ",
      "СПРАВОЧНИК_ПАПОК",
      "СПРАВОЧНИКИ",
      "ССЫЛКИ",
      "СЦЕНАРИИ",
      "ТИПЫ_КАРТОЧЕК_ЭЛЕКТРОННЫХ_ДОКУМЕНТОВ",
      "ТИПЫ_ПРАВ_ДОСТУПА",
      "ТИПЫ_СПРАВОЧНИКОВ",
      "УВЕДОМЛЕНИЯ",
      "УДАЛЕННЫЕ_СЕРВЕРА",
      "УСТАНОВКИ_СИСТЕМЫ",
      "ФУНКЦИИ"
    };

    private readonly HashSet<string> systemReferenceNames = new HashSet<string>();

    /// <summary>
    /// Проверить, существует ли системная константа с именем name.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Признак существования.</returns>
    public bool IsExistConstant(string name)
    {
      return this.Constants.Keys.Any(c => c.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Получить значение константы с именем name.
    /// </summary>
    /// <param name="name">Имя константы.</param>
    /// <returns>Значение константы.</returns>
    public string GetConstantValue(string name)
    {
      if (!this.IsExistConstant(name))
        return null;

      return this.Constants
        .FirstOrDefault(p => p.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
        .Value
        .ToString();
    }

    /// <summary>
    /// Проверить, существует ли предопределенная переменная с именем name.
    /// </summary>
    /// <param name="name">Имя переменной.</param>
    /// <returns>Признак существования.</returns>
    public bool IsExistPredefinedVariable(string name)
    {
      if (name[0] == '!')
        name = name.Substring(1);

      return this.PredefinedVariables.Any(v => v.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Проверить, существует ли переменная перечисления с именем name.
    /// </summary>
    /// <param name="name">Имя переменной.</param>
    /// <returns>Признак существования.</returns>
    public bool IsExistEnumValue(string name)
    {
      return this.Enums.Keys.Any(c => c.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Проверить, существует ли такой системный справочнник.
    /// </summary>
    /// <param name="name">Название справочника.</param>
    /// <param name="withOldReference">Учитывать устаревшие наименования системных справочников.</param>
    /// <returns>Возвращает false, если справочника не существует.</returns>
    public bool IsExistsSysReference(string name, bool withOldReference)
    {
      return this.systemReferenceNames.Contains(name) ||
        withOldReference && oldReferenceNames.Contains(name);
    }

    private static Function ToFunction(MethodInfo methodInfo)
    {
      var function = new Function();
      function.Name = methodInfo.Name;
      function.Title = methodInfo.Name;
      function.IsSystem = true;
      var arguments = methodInfo
        .GetParameters()
        .Select(p => new FunctionArgument
        {
          Name = p.Name,
          Number = p.Position,
          HasDefaultValue = p.HasDefaultValue,
          DefaultValue = p.DefaultValue?.ToString(),
          Type = GetFunctionArgumentType(p)
        });
      foreach (var a in arguments)
        function.Arguments.Add(a);
      return function;
    }

    private static FunctionArgumentType GetFunctionArgumentType(ParameterInfo parameter)
    {
      var type = parameter.ParameterType;
      if (type == typeof(bool))
        return FunctionArgumentType.Boolean;
      if (type == typeof(DateTime) || type == typeof(DateTime?))
        return FunctionArgumentType.Date;
      if (type == typeof(float))
        return FunctionArgumentType.Float;
      if (type == typeof(int))
        return FunctionArgumentType.Integer;
      if (type == typeof(string))
        return FunctionArgumentType.String;
      if (type == typeof(object))
        return FunctionArgumentType.Variant;
      throw new ArgumentException($"Unknown \"{parameter.Name}\" parameter type");
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal ApplicationContext()
    {
      var assembly = Assembly.GetCallingAssembly();
      this.Constants = typeof(Constants)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(f => f.IsLiteral && !f.IsInitOnly)
        .ToDictionary(k => k.Name, v => v.GetRawConstantValue());
      this.Enums = assembly
        .GetTypes()
        .Where(t => t.Namespace.Equals("IsblCheck.Context.Application.Enums") && t.IsEnum)
        .SelectMany(e => Enum.GetValues(e).Cast<object>())
        .ToDictionary(k => k.ToString(), v => (int)v);
      this.Functions = typeof(Functions)
        .GetMethods()
        .Select(m => ToFunction(m))
        .ToList();
      this.systemReferenceNames = new HashSet<string>(
        Constants
          .Where(p => p.Key.StartsWith("SYSREF_"))
          .Select(p => p.Value.ToString()), 
        StringComparer.OrdinalIgnoreCase);
      this.systemReferenceNames.UnionWith(
        Constants
          .Where(p => p.Key.StartsWith("SYSREF_"))
          .Select(p => p.Key));
    }
  }
}
