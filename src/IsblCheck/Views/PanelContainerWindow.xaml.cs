using IsblCheck.Common.Panels;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace IsblCheck.Views
{
  /// <summary>
  /// Представление главного окна.
  /// </summary>
  public partial class PanelContainerWindow
  {
    /// <summary>
    /// Файл конфигурации шаблонов.
    /// </summary>
    private const string LayoutRootConfigurationFile = "IsblCheck.PanelContainer.config";

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PanelContainerWindow()
    {
      this.InitializeComponent();
      this.Loaded += this.MainWindowLoadedHandler;
      this.Closing += this.MainWindowClosingHandler;
    }

    /// <summary>
    /// Обработчик события загрузки окна.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void MainWindowLoadedHandler(object sender, RoutedEventArgs e)
    {
      if (!File.Exists(LayoutRootConfigurationFile))
        return;
      var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
      layoutSerializer.LayoutSerializationCallback += this.LayoutSerializationCallbackHandler;
      layoutSerializer.Deserialize(LayoutRootConfigurationFile);
    }

    /// <summary>
    /// Обработчик события закрытия окна.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void MainWindowClosingHandler(object sender, CancelEventArgs e)
    {
      var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
      layoutSerializer.Serialize(LayoutRootConfigurationFile);
    }

    /// <summary>
    /// Обработчик события загрузки 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LayoutSerializationCallbackHandler(object sender, LayoutSerializationCallbackEventArgs e)
    {
      if (string.IsNullOrEmpty(e.Model.ContentId))
      {
        e.Cancel = true;
        return;
      }

      var panel = PanelManager.Instance.GetNamedPanel(e.Model.ContentId);
      if (panel == null)
        panel = PanelManager.Instance.CreateNamedPanel(e.Model.ContentId, e.Model is LayoutAnchorable);

      if (panel == null)
        e.Cancel = true;
      else
        e.Content = panel;
    }
  }
}
