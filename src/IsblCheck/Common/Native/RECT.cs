using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace IsblCheck.Common.Native
{
  /// <summary>
  /// Прямоугольник.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct RECT
  {
    #region Поля и свойства

    /// <summary>
    /// Координата левой стороны.
    /// </summary>
    public int Left;

    /// <summary>
    /// Координата верхней стороны.
    /// </summary>
    public int Top;

    /// <summary>
    /// Координата правой стороны.
    /// </summary>
    public int Right;

    /// <summary>
    /// Координата нижней стороны.
    /// </summary>
    public int Bottom;

    /// <summary>
    /// Координата X верхнего левого угла.
    /// </summary>
    public int X
    {
      get { return this.Left; }
      set
      {
        this.Right -= (this.Left - value);
        this.Left = value;
      }
    }

    /// <summary>
    /// Координата Y верхнего левого угла.
    /// </summary>
    public int Y
    {
      get { return Top; }
      set
      {
        this.Bottom -= (this.Top - value);
        this.Top = value;
      }
    }

    /// <summary>
    /// Высота.
    /// </summary>
    public int Height
    {
      get { return this.Bottom - this.Top; }
      set { this.Bottom = value + this.Top; }
    }

    /// <summary>
    /// Ширина.
    /// </summary>
    public int Width
    {
      get { return this.Right - this.Left; }
      set { this.Right = value + this.Left; }
    }

    /// <summary>
    /// Координаты верхнего левого угла.
    /// </summary>
    public Point Location
    {
      get { return new Point(this.Left, this.Top); }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    /// <summary>
    /// Размеры прямоугольника.
    /// </summary>
    public Size Size
    {
      get { return new Size(this.Width, this.Height); }
      set
      {
        this.Width = value.Width;
        this.Height = value.Height;
      }
    }

    #endregion

    #region Методы

    /// <summary>
    /// Преобразовать в Rectangle.
    /// </summary>
    /// <param name="r">RECT.</param>
    public static implicit operator Rectangle(RECT r)
    {
      return new Rectangle(r.Left, r.Top, r.Width, r.Height);
    }

    /// <summary>
    /// Преобразовать в RECT.
    /// </summary>
    /// <param name="r">Rectangle</param>
    public static implicit operator RECT(Rectangle r)
    {
      return new RECT(r);
    }

    /// <summary>
    /// Сравнить 2 экземпляра класса.
    /// </summary>
    /// <param name="r1">1 экземпляр класса.</param>
    /// <param name="r2">2 экземпляр класса.</param>
    /// <returns>true, в случае если классы равны, иначе false.</returns>
    public static bool operator ==(RECT r1, RECT r2)
    {
      return r1.Equals(r2);
    }

    /// <summary>
    /// Сравнить 2 экземпляра класса.
    /// </summary>
    /// <param name="r1">1 экземпляр класса.</param>
    /// <param name="r2">2 экземпляр класса.</param>
    /// <returns>true, в случае если классы не равны, иначе false.</returns>
    public static bool operator !=(RECT r1, RECT r2)
    {
      return !r1.Equals(r2);
    }

    /// <summary>
    /// Сравнить с другим экземпляром класса.
    /// </summary>
    /// <param name="r">Другой экземпляр класса.</param>
    /// <returns>true, в случае если классы равны, иначе false.</returns>
    public bool Equals(RECT r)
    {
      return r.Left == this.Left && r.Top == this.Top && r.Right == this.Right && r.Bottom == this.Bottom;
    }

    /// <summary>
    /// Сравнить с другим экземпляром класса.
    /// </summary>
    /// <param name="obj">Другой экземпляр класса.</param>
    /// <returns>true, в случае если классы равны, иначе false.</returns>
    public override bool Equals(object obj)
    {
      if (obj is RECT)
        return Equals((RECT)obj);
      else if (obj is Rectangle)
        return Equals(new RECT((Rectangle)obj));
      return false;
    }

    /// <summary>
    /// Получить хешкод.
    /// </summary>
    /// <returns>хешкод.</returns>
    public override int GetHashCode()
    {
      return ((Rectangle)this).GetHashCode();
    }

    /// <summary>
    /// Преобразовать в строку.
    /// </summary>
    /// <returns>Строковое представление.</returns>
    public override string ToString()
    {
      return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}",
        this.Left, this.Top, this.Right, this.Bottom);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="left">Координата левой стороны.</param>
    /// <param name="top">Координата верхней стороны.</param>
    /// <param name="right">Координата правой стороны.</param>
    /// <param name="bottom">Координата нижней стороны.</param>
    public RECT(int left, int top, int right, int bottom)
    {
      Left = left;
      Top = top;
      Right = right;
      Bottom = bottom;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="r">Прямоугольник.</param>
    public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

    #endregion
  }
}
