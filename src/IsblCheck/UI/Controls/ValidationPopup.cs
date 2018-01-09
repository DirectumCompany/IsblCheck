using IsblCheck.Common.Native;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace IsblCheck.UI.Controls
{
  /// <summary>
  /// Popup уведомление ошибки валидации.
  /// </summary>
  public class ValidationPopup : Popup
  {
    #region Константы

    /// <summary>
    /// Флаг расположения поверх всех окон.
    /// </summary>
    public const SetWindowPosFlags TopMostFlags = SetWindowPosFlags.DoNotActivate | SetWindowPosFlags.DoNotChangeOwnerZOrder |
      SetWindowPosFlags.IgnoreResize | SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.DoNotRedraw | SetWindowPosFlags.DoNotSendChangingEvent;

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Закрывать по нажатию левой кнопкой мыши.
    /// </summary>
    public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty =
      DependencyProperty.Register("CloseOnMouseLeftButtonDown", typeof(bool),
        typeof(ValidationPopup), new PropertyMetadata(true));

    /// <summary>
    /// Закрывать по нажатию левой кнопкой мыши.
    /// </summary>
    public bool CloseOnMouseLeftButtonDown
    {
      get { return (bool)this.GetValue(CloseOnMouseLeftButtonDownProperty); }
      set { this.SetValue(CloseOnMouseLeftButtonDownProperty, value); }
    }

    /// <summary>
    /// Поверх всех.
    /// </summary>
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    /// <summary>
    /// Не поверх всех.
    /// </summary>
    private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    /// <summary>
    /// Сверху.
    /// </summary>
    private static readonly IntPtr HWND_TOP = new IntPtr(0);

    /// <summary>
    /// Снизу.
    /// </summary>
    private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    /// <summary>
    /// Применения показа поверх всех окон.
    /// </summary>
    private bool? appliedTopMost;

    /// <summary>
    /// Родительское окно.
    /// </summary>
    private Window hostWindow;

    #endregion

    #region Методы

    /// <summary>
    /// Обработка события нажатия кнопки.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (this.CloseOnMouseLeftButtonDown)
      {
        this.IsOpen = false;
      }
    }

    /// <summary>
    /// Установить режим перекрытия окна.
    /// </summary>
    /// <param name="isTop">Показывать поверх всех окон.</param>
    private void SetTopmostState(bool isTop)
    {
      if (this.appliedTopMost.HasValue && this.appliedTopMost == isTop)
        return;

      if (this.Child == null)
        return;

      var hwndSource = (PresentationSource.FromVisual(this.Child)) as HwndSource;
      if (hwndSource == null)
        return;
      var hwnd = hwndSource.Handle;

      RECT rect;
      if (!NativeMethods.GetWindowRect(hwnd, out rect))
        return;

      var left = rect.Left;
      var top = rect.Top;
      var width = rect.Width;
      var height = rect.Height;
      if (isTop)
        NativeMethods.SetWindowPos(hwnd, HWND_TOPMOST, left, top, width, height, TopMostFlags);
      else
      {
        NativeMethods.SetWindowPos(hwnd, HWND_BOTTOM, left, top, width, height, TopMostFlags);
        NativeMethods.SetWindowPos(hwnd, HWND_TOP, left, top, width, height, TopMostFlags);
        NativeMethods.SetWindowPos(hwnd, HWND_NOTOPMOST, left, top, width, height, TopMostFlags);
      }

      this.appliedTopMost = isTop;
    }

    /// <summary>
    /// Обработчик события изменения позиции родительского окна.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void HostWindowSizeOrLocationChangedHandler(object sender, EventArgs e)
    {
      var offset = this.HorizontalOffset;
      this.HorizontalOffset = offset + 1;
      this.HorizontalOffset = offset;
    }

    /// <summary>
    /// Обработчик события изменения состояния окна.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void HostWindowStateChangedHandler(object sender, EventArgs e)
    {
      if (this.hostWindow != null && this.hostWindow.WindowState != WindowState.Minimized)
      {
        var target = this.PlacementTarget as FrameworkElement;
        var holder = target != null ? target.DataContext as AdornedElementPlaceholder : null;
        if (holder != null && holder.AdornedElement != null)
        {
          this.PopupAnimation = PopupAnimation.None;
          this.IsOpen = false;
          var errorTemplate = holder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
          holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
          holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, errorTemplate);
        }
      }
    }

    /// <summary>
    /// Обработчик события изменения активации окна.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void HostWindowActivatedHandler(object sender, EventArgs e)
    {
      this.SetTopmostState(true);
    }

    /// <summary>
    /// Обработчик события изменения деактивации окна.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void HostWindowDeactivatedHandler(object sender, EventArgs e)
    {
      this.SetTopmostState(false);
    }

    /// <summary>
    /// Обработчик события загрузки.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void ValidationPopupLoadedHandler(object sender, RoutedEventArgs e)
    {
      var target = this.PlacementTarget as FrameworkElement;
      if (target == null)
        return;

      this.hostWindow = Window.GetWindow(target);
      if (this.hostWindow == null)
        return;

      this.hostWindow.LocationChanged -= this.HostWindowSizeOrLocationChangedHandler;
      this.hostWindow.LocationChanged += this.HostWindowSizeOrLocationChangedHandler;
      this.hostWindow.SizeChanged -= this.HostWindowSizeOrLocationChangedHandler;
      this.hostWindow.SizeChanged += this.HostWindowSizeOrLocationChangedHandler;
      target.SizeChanged -= this.HostWindowSizeOrLocationChangedHandler;
      target.SizeChanged += this.HostWindowSizeOrLocationChangedHandler;
      this.hostWindow.StateChanged -= this.HostWindowStateChangedHandler;
      this.hostWindow.StateChanged += this.HostWindowStateChangedHandler;
      this.hostWindow.Activated -= this.HostWindowActivatedHandler;
      this.hostWindow.Activated += this.HostWindowActivatedHandler;
      this.hostWindow.Deactivated -= this.HostWindowDeactivatedHandler;
      this.hostWindow.Deactivated += this.HostWindowDeactivatedHandler;

      this.Unloaded -= this.ValidationPopupUnloadedHandler;
      this.Unloaded += this.ValidationPopupUnloadedHandler;
    }

    /// <summary>
    /// Обработчик события выгрузки.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void ValidationPopupUnloadedHandler(object sender, RoutedEventArgs e)
    {
      var target = this.PlacementTarget as FrameworkElement;
      if (target != null)
        target.SizeChanged -= this.HostWindowSizeOrLocationChangedHandler;

      if (this.hostWindow != null)
      {
        this.hostWindow.LocationChanged -= this.HostWindowSizeOrLocationChangedHandler;
        this.hostWindow.SizeChanged -= this.HostWindowSizeOrLocationChangedHandler;
        this.hostWindow.StateChanged -= this.HostWindowStateChangedHandler;
        this.hostWindow.Activated -= this.HostWindowActivatedHandler;
        this.hostWindow.Deactivated -= this.HostWindowDeactivatedHandler;
      }

      this.Unloaded -= this.ValidationPopupUnloadedHandler;
      this.Opened -= this.ValidationPopupOpenedHandler;
      this.hostWindow = null;
    }

    /// <summary>
    /// Обработчик события открытия.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private void ValidationPopupOpenedHandler(object sender, EventArgs e)
    {
      this.SetTopmostState(true);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ValidationPopup()
    {
      this.Loaded += this.ValidationPopupLoadedHandler;
      this.Opened += this.ValidationPopupOpenedHandler;
    }

    #endregion
  }
}
