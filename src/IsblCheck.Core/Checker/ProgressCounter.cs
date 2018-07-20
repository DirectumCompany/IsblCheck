using System;

namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Счетчик прогресса.
  /// </summary>
  internal class ProgressCounter
  {
    /// <summary>
    /// Объект блокировки.
    /// </summary>
    private readonly object lockObj = new object();

    /// <summary>
    /// Счетчик.
    /// </summary>
    private int counter;

    /// <summary>
    /// Прогресс.
    /// </summary>
    private readonly IProgress<int> progress;

    /// <summary>
    /// Увеличить счетчик.
    /// </summary>
    internal void Increment()
    {
      lock(lockObj)
      {
        this.counter++;
        this.progress?.Report(this.counter);
      }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="progress">Прогресс.</param>
    internal ProgressCounter(IProgress<int> progress)
    {
      this.progress = progress;
    }
  }
}
