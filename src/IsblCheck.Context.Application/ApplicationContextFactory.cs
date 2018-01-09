using IsblCheck.Core.Context.Application;

namespace IsblCheck.Context.Application
{
  /// <summary>
  /// Фабрика контекста приложения.
  /// </summary>
  public class ApplicationContextFactory : IApplicationContextFactory
  {
    /// <summary>
    /// Создать контекст приложения.
    /// </summary>
    /// <returns>Контекст приложения.</returns>
    public IApplicationContext Create()
    {
      return new ApplicationContext();
    }
  }
}
