using System;
using System.Collections.Generic;
using System.Reflection;

namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Менеджер правил.
  /// </summary>
  public interface IRuleManager
  {
    /// <summary>
    /// Событие изменения состава правил.
    /// </summary>
    event EventHandler RuleCollectionChanged;

    /// <summary>
    /// Получить все правила.
    /// </summary>
    /// <returns>Все правила.</returns>
    IEnumerable<IRule> GetAllRules();

    /// <summary>
    /// Загрузить библиотеку с правилами.
    /// </summary>
    /// <param name="path">Путь к библиотеке.</param>
    void LoadLibrary(string path);

    /// <summary>
    /// Загрузить библиотеку с правилами.
    /// </summary>
    /// <param name="assembly">Сборка.</param>
    void LoadLibrary(Assembly assembly);

    /// <summary>
    /// Загрузить сборки с правилами из заданной директории.
    /// </summary>
    /// <param name="path"></param>
    void LoadLibraries(string path);
  }
}
