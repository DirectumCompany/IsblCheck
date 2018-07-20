using System;
using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Core.Context
{
  /// <summary>
  /// Менеджер интерфейса контекста.
  /// </summary>
  public interface IContextManager
  {
    /// <summary>
    /// Глобальный контекст.
    /// </summary>
    IContext Context { get; }

    /// <summary>
    /// Событие изменения контекста.
    /// </summary>
    event EventHandler ContextChanged;

    /// <summary>
    /// Загрузить контекст приложения.
    /// </summary>
    /// <param name="factory">Фабрика контекста приложения.</param>
    void Load(IApplicationContextFactory factory);

    /// <summary>
    /// Загрузить контекст разработки.
    /// </summary>
    /// <param name="factory">Фабрика контекста разработки.</param>
    void Load(IDevelopmentContextFactory factory);

    /// <summary>
    /// Перезагрузить глобальный контекст.
    /// </summary>
    void Refresh();
  }
}
