using System.Collections.Generic;

namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Интерфейс фабрики правил.
  /// </summary>
  public interface IRuleFactory
  {
    /// <summary>
    /// Получить инфо правил в сборке.
    /// </summary>
    /// <returns>
    /// Коллекцию инфо правил.
    /// </returns>
    IEnumerable<IRuleInfo> GetRuleInfos();

    /// <summary>
    /// Получить правила.
    /// </summary>
    /// <returns>Список правил.</returns>
    IEnumerable<IRule> GetRules(IEnumerable<IRuleInfo> infos);
  }
}
