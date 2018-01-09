namespace IsblCheck.Core.Context.Application
{
  /// <summary>
  /// Интерфейс фабрики контекста приложения.
  /// </summary>
  public interface IApplicationContextFactory
  {
    /// <summary>
    /// Создать контекст приложения.
    /// </summary>
    /// <returns>Контекст приложения.</returns>
    IApplicationContext Create();
  }
}
