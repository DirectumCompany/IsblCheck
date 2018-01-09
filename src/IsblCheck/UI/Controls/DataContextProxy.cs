using System.Windows;

namespace IsblCheck.UI.Controls
{
  /// <summary>
  /// Прокси датаконтекста.
  /// </summary>
  public class DataContextProxy : Freezable
  {
    /// <summary>
    /// Контекст данных.
    /// </summary>
    public static readonly DependencyProperty DataContextProperty =
      DependencyProperty.Register("DataContext", typeof(object), typeof(DataContextProxy),
        new UIPropertyMetadata(null));

    /// <summary>
    /// Контекст данных.
    /// </summary>
    public object DataContext
    {
      get { return this.GetValue(DataContextProperty); }
      set { this.SetValue(DataContextProperty, value); }
    }

    /// <summary>
    /// Получить экземпляр.
    /// </summary>
    /// <returns>Экземпляр объекта.</returns>
    protected override Freezable CreateInstanceCore()
    {
      return new DataContextProxy();
    }
  }
}
