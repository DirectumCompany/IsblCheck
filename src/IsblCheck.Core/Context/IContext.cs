using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Core.Context
{
  /// <summary>
  /// Контекст проверки.
  /// </summary>
  public interface IContext
  {
    /// <summary>
    /// Контекст приложения.
    /// </summary>
    IApplicationContext Application { get; }

    /// <summary>
    /// Контекст разработки.
    /// </summary>
    IDevelopmentContext Development { get; }
  }
}
