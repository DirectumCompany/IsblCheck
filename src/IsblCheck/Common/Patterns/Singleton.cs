using System;
using System.Runtime.CompilerServices;

namespace IsblCheck.Common.Patterns
{
  /// <summary>
  /// Класс одиночка.
  /// </summary>
  /// <typeparam name="T">Тип класса одиночки.</typeparam>
  public class Singleton<T> where T : Singleton<T>
  {
    /// <summary>
    /// Объект блокировки.
    /// </summary>
    private static object syncRoot;

    /// <summary>
    /// Экземпляр объекта.
    /// </summary>
    private static T instance;

    /// <summary>
    /// Экземпляр объекта.
    /// </summary>
    public static T Instance
    {
      get
      {
        if (instance == null)
        {
          lock (syncRoot)
          {
            if (instance == null)
              instance = (T)Activator.CreateInstance(typeof(T), true);
          }
        }
        return instance;
      }
      set
      {
        if (value != null)
          throw new ArgumentNullException("value");

        lock (syncRoot)
        {
          instance = default(T);
        }
      }
    }

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static Singleton()
    {
      syncRoot = RuntimeHelpers.GetObjectValue(new object());
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected Singleton()
    {
      instance = (T)this;
    }
  }
}
