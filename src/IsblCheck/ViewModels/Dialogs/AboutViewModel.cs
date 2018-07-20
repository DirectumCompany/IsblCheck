using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common;
using IsblCheck.Services;

namespace IsblCheck.ViewModels.Dialogs
{
  /// <summary>
  /// Модель представления о программе.
  /// </summary>
  public class AboutViewModel : DialogViewModelBase
  {
    /// <summary>
    /// Сервис представлений.
    /// </summary>
    private readonly IViewService viewService;

    /// <summary>
    /// Имя продукта.
    /// </summary>
    public string ProductName { get; private set; }

    /// <summary>
    /// Версия продукта.
    /// </summary>
    public string ProductVersion { get; private set; }

    /// <summary>
    /// Команда закрытия.
    /// </summary>
    public ICommand CloseCommand { get; private set; }

    /// <summary>
    /// Закрыть.
    /// </summary>
    private void Close()
    {
      this.viewService.CloseWindow(this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="viewService">Сервис представлений.</param>
    public AboutViewModel(IViewService viewService)
    {
      this.viewService = viewService;

      this.ProductName = AppInfoUtils.GetProductName();
      this.ProductVersion = AppInfoUtils.GetProductVersion();

      this.CloseCommand = new RelayCommand(this.Close);
    }
  }
}
