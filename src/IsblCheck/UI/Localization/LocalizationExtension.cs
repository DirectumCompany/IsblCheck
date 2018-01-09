using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace IsblCheck.UI.Localization
{
  /// <summary>
  /// Расширение разметки для локализации.
  /// </summary>
  [ContentProperty("ArgumentBindings")]
  public class LocalizationExtension : MarkupExtension
  {
    #region Поля и свойства.

    /// <summary>
    /// Аругменты.
    /// </summary>
    private Collection<BindingBase> arguments;

    /// <summary>
    /// Ключ локализованной строки
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Привязка для ключа локализованной строки.
    /// </summary>
    public Binding KeyBinding { get; set; }

    /// <summary>
    /// Аргументы форматируемой локализованный строки
    /// </summary>
    public IEnumerable<object> Arguments { get; set; }

    /// <summary>
    /// Привязки аргументов форматируемой локализованный строки
    /// </summary>
    public Collection<BindingBase> ArgumentBindings
    {
      get { return arguments ?? (arguments = new Collection<BindingBase>()); }
      set { arguments = value; }
    }

    #endregion

    #region Методы

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      if (this.Key != null && this.KeyBinding != null)
        throw new ArgumentException("Could not set Key and KeyBinding at the same time.");

      if (this.Key == null && this.KeyBinding == null)
        throw new ArgumentException("You must specify Key or Keybinging.");

      if (this.Arguments != null && this.ArgumentBindings.Any())
        throw new ArgumentException("Could not set Argument and ArugmentBindings at the same time.");

      var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
      if (target.TargetObject.GetType().FullName == "System.Windows.SharedDp")
        return this;

      // Если заданы привязка ключа или список привязок аргументов,
      // то используем LocalizationBinding
      if (this.KeyBinding != null || this.ArgumentBindings.Any())
      {
        var listener = new LocalizationBinding();

        // Создаем привязку для слушателя
        var listenerBinding = new Binding
        {
          Source = listener
        };
        var keyBinding = this.KeyBinding ?? new Binding
        {
          Source = Key
        };
        var multiBinding = new MultiBinding
        {
          Converter = new LocalizationBindingConverter(),
          ConverterParameter = Arguments
        };
        multiBinding.Bindings.Add(listenerBinding);
        multiBinding.Bindings.Add(keyBinding);

        // Добавляем все переданные привязки аргументов
        foreach (var binding in this.ArgumentBindings)
          multiBinding.Bindings.Add(binding);

        // Сохраняем выражение привязки в слушателе
        var value = multiBinding.ProvideValue(serviceProvider);
        listener.SetBinding(value as BindingExpressionBase);
        return value;
      }

      // Если задан ключ, то используем LocalizationString
      if (!string.IsNullOrEmpty(this.Key))
      {
        var args = this.Arguments == null ? null : this.Arguments.ToArray();
        var listener = new LocalizationString(this.Key, args);

        // Если локализация навешана на DependencyProperty объекта DependencyObject или на Setter
        if ((target.TargetObject is DependencyObject && target.TargetProperty is DependencyProperty)
          || target.TargetObject is Setter)
        {
          var binding = new Binding("Value")
          {
            Source = listener,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
          };
          return binding.ProvideValue(serviceProvider);
        }

        // Если локализация навешана на Binding, то возвращаем слушателя
        var targetBinding = target.TargetObject as Binding;
        if (targetBinding != null
          && target.TargetProperty != null
          && target.TargetProperty.GetType().FullName == "System.Reflection.RuntimePropertyInfo"
          && target.TargetProperty.ToString() == "System.Object Source")
        {
          targetBinding.Path = new PropertyPath("Value");
          targetBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
          return listener;
        }

        // Иначе возвращаем локализованную строку
        return listener.Value;
      }

      return null;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LocalizationExtension()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="key">Ключ.</param>
    public LocalizationExtension(string key)
    {
      Key = key;
    }

    #endregion
  }
}
