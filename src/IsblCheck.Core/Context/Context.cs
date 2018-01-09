using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Core.Context
{
  /// <summary>
  /// Контекст.
  /// </summary>
  internal class Context : IContext
  {
    #region IContext

    /// <summary>
    /// Контекст приложения.
    /// </summary>
    public IApplicationContext Application { get; internal set; }

    /// <summary>
    /// Контекст разработки.
    /// </summary>
    public IDevelopmentContext Development { get; internal set; }

    #endregion
  }
}
