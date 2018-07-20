using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable InconsistentNaming
namespace IsblCheck.Common.Native
{
  /// <summary>
  /// Нативные методы.
  /// </summary>
  internal static class NativeMethods
  {
    /// <summary>
    /// Показать окно.
    /// </summary>
    /// <param name="hWnd">Хендл окна.</param>
    /// <param name="nCmdShow">Параметры показа.</param>
    /// <returns>true, в случае успешности выполнения метода, иначе false.</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

    /// <summary>
    /// Получить прямоугольник окна.
    /// </summary>
    /// <param name="hwnd">Хендл окна.</param>
    /// <param name="lpRect">Прямоугольник.</param>
    /// <returns>true, в случае успешности выполнения метода, иначе false.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    /// <summary>
    /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered
    /// according to their appearance on the screen. The topmost window receives the highest rank and is the first window
    /// in the Z order.
    /// <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545%28v=vs.85%29.aspx for more information.</para>
    /// </summary>
    /// <param name="hWnd">C++ ( hWnd [in]. Type: HWND )<br />A handle to the window.</param>
    /// <param name="hWndInsertAfter">C++ ( hWndInsertAfter [in, optional]. Type: HWND )<br />A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the following values.
    /// </param>
    /// <param name="X">C++ ( X [in]. Type: int )<br />The new position of the left side of the window, in client coordinates.</param>
    /// <param name="Y">C++ ( Y [in]. Type: int )<br />The new position of the top of the window, in client coordinates.</param>
    /// <param name="cx">C++ ( cx [in]. Type: int )<br />The new width of the window, in pixels.</param>
    /// <param name="cy">C++ ( cy [in]. Type: int )<br />The new height of the window, in pixels.</param>
    /// <param name="uFlags">C++ ( uFlags [in]. Type: UINT )<br />The window sizing and positioning flags. This parameter can be a combination of the following values.</param>
    /// <returns><c>true</c> or nonzero if the function succeeds, <c>false</c> or zero otherwise or if function fails.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    /// <summary>
    /// Преобразовать в юникод.
    /// </summary>
    /// <param name="wVirtKey">виртуальная клавиша.</param>
    /// <param name="wScanCode">Скан код.</param>
    /// <param name="lpKeyState">Состояние клавиши.</param>
    /// <param name="pwszBuff">Буфер.</param>
    /// <param name="cchBuff">Буфер.</param>
    /// <param name="wFlags">Флаги.</param>
    /// <returns>Юникод значение.</returns>
    [DllImport("user32.dll")]
    public static extern int ToUnicode(uint wVirtKey, uint wScanCode,byte[] lpKeyState,
      [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

    /// <summary>
    /// Получить состояние клавиатуры.
    /// </summary>
    /// <param name="lpKeyState">Состояние клавиш.</param>
    /// <returns>true, в случае успешности выполнения метода, иначе false.</returns>
    [DllImport("user32.dll")]
    public static extern bool GetKeyboardState(byte[] lpKeyState);

    /// <summary>
    /// Замапить виртуальные клавиши.
    /// </summary>
    /// <param name="uCode">Код.</param>
    /// <param name="uMapType">Тип маппинга.</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, MapType uMapType);
  }
}
