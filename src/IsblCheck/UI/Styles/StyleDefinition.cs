using System;
using System.Windows;

namespace IsblCheck.UI.Styles
{
  /// <summary>
  /// Определение стиля.
  /// </summary>
  public class StyleDefinition : DependencyObject
  {
    /// <summary>
    /// Тип элемента.
    /// </summary>
    public static readonly DependencyProperty TypeProperty =
      DependencyProperty.Register("Type", typeof(Type), typeof(StyleDefinition));

    /// <summary>
    /// Выбирать по типу контейнера
    /// </summary>
    public static readonly DependencyProperty SelectByContainerProperty =
      DependencyProperty.Register("SelectByContainer", typeof(bool), typeof(StyleDefinition));

    /// <summary>
    /// Стиль.
    /// </summary>
    public static readonly DependencyProperty StyleProperty =
      DependencyProperty.Register("Style", typeof(Style), typeof(StyleDefinition));


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
    /// Стиль.
    /// </summary>
    public Style Style
    {
      get { return (Style)GetValue(StyleProperty); }
      set { SetValue(StyleProperty, value); }
    }
  }
}
