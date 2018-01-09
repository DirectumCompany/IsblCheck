using IsblCheck.Common.Windows;
using System.Collections.ObjectModel;

namespace IsblCheck.Common.Panels
{
  /// <summary>
  /// Интерфейс контейнера панелей.
  /// </summary>
  public interface IPanelContainer : IWindow
  {
    /// <summary>
    /// Панели.
    /// </summary>
    ObservableCollection<IPanel> Panels { get; }

    /// <summary>
    /// Закрепляемые панели.
    /// </summary>
    ObservableCollection<IPanel> AnchorablePanels { get; }

    /// <summary>
    /// Видимость прогресса.
    /// </summary>
    bool IsProgressVisible { get; set; }

    /// <summary>
    /// Значение прогресса.
    /// </summary>
    int ProgressValue { get; set; }

    /// <summary>
    /// Максимальное значение прогресса.
    /// </summary>
    int ProgressMaximum { get; set; }

    /// <summary>
    /// Видимость индикатора загрузки.
    /// </summary>
    bool IsBusyIndicatorVisible { get; set; }

    /// <summary>
    /// Заголовок индикатора загрузки.
    /// </summary>
    string BusyIndicatorCaption { get; set; }
  }
}
