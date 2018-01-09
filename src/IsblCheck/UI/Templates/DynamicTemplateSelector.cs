using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace IsblCheck.UI.Templates
{
  /// <summary>
  /// Динамический селектор шаблонов.
  /// </summary>
  [ContentProperty("TemplateDefinitions")]
  public class DynamicTemplateSelector : DataTemplateSelector
  {
    /// <summary>
    /// Определения шаблонов.
    /// </summary>
    public Collection<TemplateDefinition> TemplateDefinitions
    {
      get
      {
        if (this.templateDefinitions == null)
          this.templateDefinitions = new Collection<TemplateDefinition>();
        return templateDefinitions;
      }
    }
    private Collection<TemplateDefinition> templateDefinitions;

    /// <summary>
    /// Выбрать шаблон.
    /// </summary>
    /// <param name="item">Элемент.</param>
    /// <param name="container">Контейнер.</param>
    /// <returns>Шаблон.</returns>
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      foreach (var templateDefinition in this.TemplateDefinitions)
      {
        var targetType = templateDefinition.SelectByContainer ? container.GetType() : item.GetType();
        if (templateDefinition.Type == item.GetType())
          return templateDefinition.DataTemplate;
      }

      return base.SelectTemplate(item, container);
    }
  }
}
