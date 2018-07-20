using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using IsblCheck.Common.Localization;

namespace IsblCheck.UI.Localization
{
  /// <summary>
  /// Конвертер биндинга локализации.
  /// </summary>
  public class LocalizationBindingConverter : IMultiValueConverter
  {
    /// <summary>
    /// Конветировать.
    /// </summary>
    /// <param name="values">Значение.</param>
    /// <param name="targetType">Требуемый тип.</param>
    /// <param name="parameter">Параметр.</param>
    /// <param name="culture">Культура.</param>
    /// <returns>Сконветированное значение.</returns>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null || values.Length < 2)
        return null;

      var key = System.Convert.ToString(values[1] ?? "");
      var value = LocalizationManager.Instance.LocalizeString(key);
      var args = (parameter as IEnumerable<object> ?? values.Skip(2)).ToArray();
      if (args.Length == 1 && !(args[0] is string) && args[0] is IEnumerable)
        args = ((IEnumerable)args[0]).Cast<object>().ToArray();
      if (args.Any())
        return string.Format(value, args);
      return value;
    }

    /// <summary>
    /// Конветировать обратно.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <param name="targetTypes">Требуемые типы.</param>
    /// <param name="parameter">Параметр.</param>
    /// <param name="culture">Культура.</param>
    /// <returns>Сконвертированные значения.</returns>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
