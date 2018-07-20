using System;

namespace IsblCheck.Common.Patterns
{
  /// <summary>
  /// Класс одиночка.
  /// </summary>
  /// <typeparam name="T">Тип класса одиночки.</typeparam>
  public class Singleton<T> where T : Singleton<T>
  {
    private static readonly Lazy<T> instance = new Lazy<T>(Activator.CreateInstance<T>, true);

    /// <summary>
    /// Экземпляр объекта.
    /// </summary>
    public static T Instance => instance.Value;

    protected Singleton()
    {
    }
  }
}
