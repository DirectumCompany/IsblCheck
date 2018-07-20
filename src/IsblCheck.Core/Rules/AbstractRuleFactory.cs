using System;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Базовый класс фабрики правил.
  /// </summary>
  public abstract class AbstractRuleFactory : IRuleFactory
  {
    #region IRuleFactory 

    /// <summary>
    /// Получить инфо правил в сборке.
    /// </summary>
    /// <returns>
    /// Коллекцию инфо правил.
    /// </returns>
    public virtual IEnumerable<IRuleInfo> GetRuleInfos()
    {
      foreach(var ruleInfo in this.RuleTypes.Keys)
      {
        yield return ruleInfo;
      }
    }

    /// <summary>
    /// Получить правила.
    /// </summary>
    /// <returns>Список правил.</returns>
    public virtual IEnumerable<IRule> GetRules(IEnumerable<IRuleInfo> infos)
    {
      foreach (var info in infos)
      {
        var ruleType = this.RuleTypes[info];
        yield return (IRule)Activator.CreateInstance(ruleType);
      }
    }

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Зарегистрированные типы правил.
    /// </summary>
    protected Dictionary<IRuleInfo, Type> RuleTypes = new Dictionary<IRuleInfo, Type>();

    #endregion

    #region Методы

    /// <summary>
    /// Зарегистрировать правило.
    /// </summary>
    /// <typeparam name="T">Тип правила.</typeparam>
    /// <param name="ruleInfo">Инфо правила.</param>
    public void Register<T>(IRuleInfo ruleInfo) where T : AbstractRule
    {
      if (this.RuleTypes.ContainsKey(ruleInfo))
        throw new ArgumentException("Rule with current info is already registered.", nameof(ruleInfo));

      if (this.RuleTypes.Any(pair => pair.Key.Name == ruleInfo.Name))
        throw new ArgumentException("Rule with current name is already registered.", nameof(ruleInfo));

      if (this.RuleTypes.ContainsValue(typeof(T)))
        throw new Exception("Current rule is already registered with other info.");

      this.RuleTypes.Add(ruleInfo, typeof(T));
    }

    /// <summary>
    /// Разрегистрировать правило.
    /// </summary>
    public void Unregister<T>()
    {
      if (!this.RuleTypes.ContainsValue(typeof(T)))
        throw new Exception("Current rule is not registered.");

      var ruleInfo = this.RuleTypes.Single(pair => pair.Value == typeof(T)).Key;
      this.RuleTypes.Remove(ruleInfo);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected AbstractRuleFactory()
    {
    }

    #endregion
  }
}
