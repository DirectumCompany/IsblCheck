using System;
using System.Windows;

namespace IsblCheck.UI.Templates
{
  /// <summary>
  /// Определение шаблона.
  /// </summary>
  public class TemplateDefinition : DependencyObject
  {
    /// <summary>
    /// Тип элемента.
    /// </summary>
    public static readonly DependencyProperty TypeProperty =
      DependencyProperty.Register("Type", typeof(Type), typeof(TemplateDefinition));

    /// <summary>
    /// Выбирать по типу контейнера
    /// </summary>
    public static readonly DependencyProperty SelectByContainerProperty =
      DependencyProperty.Register("SelectByContainer", typeof(bool), typeof(TemplateDefinition));

    /// <summary>
    /// Шаблон.
    /// </summary>
    public static readonly DependencyProperty DataTemplateProperty =
      DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(TemplateDefinition));

    /// <summary>
    /// Тип элемента.
    /// </summary>
    public Type Type
    {
      get { return (Type)GetValue(TypeProperty); }
      set { SetValue(TypeProperty, value); }
    }

    /// <summary>
    /// Выбирать по типу контейнера
    /// </summary>
    public bool SelectByContainer
    {
      get { return (bool)GetValue(SelectByContainerProperty); }
      set { SetValue(SelectByContainerProperty, value); }
    }

    /// <summary>
    /// Шаблон.
    /// </summary>
    public DataTemplate DataTemplate
    {
      get { return (DataTemplate)GetValue(DataTemplateProperty); }
      set { SetValue(DataTemplateProperty, value); }
    }
  }
}
