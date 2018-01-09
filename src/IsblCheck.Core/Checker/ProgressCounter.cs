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
    private object lockObj = new object();

    /// <summary>
    /// Счетчик.
    /// </summary>
    private int counter = 0;

    /// <summary>
    /// Прогресс.
    /// </summary>
    private IProgress<int> progress;

    /// <summary>
    /// Увеличить счетчик.
    /// </summary>
    internal void Increment()
    {
      lock(lockObj)
      {
        this.counter++;
        if (this.progress != null)
          this.progress.Report(this.counter);
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
