using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace IsblCheck.UI.Styles
{
  /// <summary>
  /// Динамический селектор стилей.
  /// </summary>
  [ContentProperty("StyleDefinitions")]
  public class DynamicStyleSelector : StyleSelector
  {
    /// <summary>
    /// Определения стилей.
    /// </summary>
    public Collection<StyleDefinition> StyleDefinitions
    {
      get
      {
        if (this.styleDefinitions == null)
          this.styleDefinitions = new Collection<StyleDefinition>();
        return styleDefinitions;
      }
    }
    private Collection<StyleDefinition> styleDefinitions;

    /// <summary>
    /// Выбрать стиль.
    /// </summary>
    /// <param name="item">Элемент.</param>
    /// <param name="container">Контейнер.</param>
    /// <returns>Выбранный стиль.</returns>
    public override Style SelectStyle(object item, DependencyObject container)
    {
      foreach (var styleDefinition in this.StyleDefinitions)
      {
        var targetType = styleDefinition.SelectByContainer ? container.GetType() : item.GetType();
        if (styleDefinition.Type == targetType)
          return styleDefinition.Style;
      }

      return base.SelectStyle(item, container);
    }
  }
}
