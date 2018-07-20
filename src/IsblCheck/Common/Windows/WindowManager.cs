using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using IsblCheck.Common.Patterns;

namespace IsblCheck.Common.Windows
{
  /// <summary>
  /// Менеджер окон.
  /// </summary>
  public class WindowManager : Singleton<WindowManager>
  {
    /// <summary>
    /// Зарегистрированные окна.
    /// </summary>
    private readonly Dictionary<Type, Type> registeredWindows = new Dictionary<Type, Type>();

    /// <summary>
    /// Зарегистрировать тип окна с моделью.
    /// </summary>
    /// <typeparam name="TViewModel">Тип модели.</typeparam>
    /// <typeparam name="TWindow">Тип окна.</typeparam>
    public void Register<TViewModel, TWindow>()
      where TViewModel : IWindow
      where TWindow : Window
    {
      var viewModelType = typeof(TViewModel);
      if (this.registeredWindows.ContainsKey(viewModelType))
        throw new InvalidOperationException("Current view model type already registered.");

      var windowType = typeof(TWindow);
      if (this.registeredWindows.ContainsValue(windowType))
        throw new InvalidOperationException("Current window type already registered.");

      this.registeredWindows.Add(viewModelType, windowType);
    }

    /// <summary>
    /// Создать окно по модели представления.
    /// </summary>
    /// <param name="viewModel">Модель представления.</param>
    /// <returns>Окно.</returns>
    public Window CreateWindow(IWindow viewModel)
    {
      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var viewModelType = viewModel.GetType();
      if (!this.registeredWindows.ContainsKey(viewModelType))
        throw new InvalidOperationException("Type of current view model is not associated with any type of window.");

      return Application.Current.Dispatcher.Invoke(() =>
      {
        var windowType = this.registeredWindows[viewModelType];
        var window = (Window)Activator.CreateInstance(windowType);
        window.DataContext = viewModel;

        return window;
      });
    }

    /// <summary>
    /// Создать окно по модели представления.
    /// </summary>
    /// <param name="ownerViewModel">Модель представления родителя.</param>
    /// <param name="viewModel">Модель представления.</param>
    /// <returns>Окно.</returns>
    public Window CreateWindow(IWindow ownerViewModel, IWindow viewModel)
    {
      if (ownerViewModel == null)
        throw new ArgumentNullException(nameof(ownerViewModel));

      if (viewModel == null)
        throw new ArgumentNullException(nameof(viewModel));

      var ownerWindow = this.FindWindowByViewModel(ownerViewModel);
      if (ownerWindow == null)
        throw new ArgumentException("Could not found window associated with current view model", nameof(ownerViewModel));

      var viewModelType = viewModel.GetType();
      if (!this.registeredWindows.ContainsKey(viewModelType))
        throw new InvalidOperationException("Type of current view model is not associated with any type of window.");

      return Application.Current.Dispatcher.Invoke(() =>
      {
        var windowType = this.registeredWindows[viewModelType];
        var window = (Window)Activator.CreateInstance(windowType);
        window.Owner = ownerWindow;
        window.DataContext = viewModel;

        return window;
      });
    }

    /// <summary>
    /// Найти окно с указанной моделью представления.
    /// </summary>
    /// <param name="viewModel">Модель представления.</param>
    /// <returns>Окно.</returns>
    public Window FindWindowByViewModel(IWindow viewModel)
    {
      var viewModelType = viewModel.GetType();
      if (!this.registeredWindows.ContainsKey(viewModelType))
        throw new InvalidOperationException("Type of current view model is not associated with any type of window.");

      return Application.Current.Windows
        .OfType<Window>()
        .FirstOrDefault(w => w.DataContext == viewModel);
    }
  }
}
