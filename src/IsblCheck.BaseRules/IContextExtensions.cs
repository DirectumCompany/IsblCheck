using System;
using System.Linq;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Context;

namespace IsblCheck.BaseRules
{
  /// <summary>
  /// Класс-расширение для IContext.
  /// </summary>
  public static class IContextExtensions
  {
    /// <summary>
    /// Префикс, с которого начинаются строки локализации группы CONST.
    /// </summary>
    private const string ConstLocalizationStringPrefix = "CONST";

    /// <summary>
    /// Проверить, является ли переменная пользовательской.
    /// </summary>
    /// <param name="context">Текущий контекст.</param>
    /// <param name="document">Документ для проверки.</param>
    /// <param name="name">Имя переменной.</param>
    /// <returns>Признак того, что переменная пользовательская.</returns>
    public static bool IsUserVariable(this IContext context, IDocument document, string name)
    {
      return !(document.ContextVariables.Contains(name.ToUpper())
        || context.Application.IsExistConstant(name)
        || context.Application.IsExistPredefinedVariable(name)
        || context.Application.IsExistEnumValue(name)
        || IsConstLocalizationString(context, name));
    }

    /// <summary>
    /// Проверить, является ли переменная строкой локализации группы CONST.
    /// </summary>
    /// <param name="context">Текущий контекст.</param>
    /// <param name="name">Имя переменной.</param>
    /// <returns>True, если является.</returns>
    private static bool IsConstLocalizationString(IContext context, string name)
    {
      if (name.StartsWith(ConstLocalizationStringPrefix + "_", StringComparison.OrdinalIgnoreCase) &&
        context.Development.LocalizationStrings.Any(s =>
          s.Name.Equals(name.Substring(ConstLocalizationStringPrefix.Length + 1), StringComparison.OrdinalIgnoreCase) &&
          s.Group.Equals(ConstLocalizationStringPrefix, StringComparison.OrdinalIgnoreCase)))
      {
        return true;
      }
      return false;
    }
  }
}
