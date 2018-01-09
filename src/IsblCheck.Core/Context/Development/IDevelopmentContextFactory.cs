using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Интерфейс фабрики контекста разработки. 
  /// </summary>
  public interface IDevelopmentContextFactory
  {
    /// <summary>
    /// Список провайдеров контекста разработки.
    /// </summary>
    IList<IDevelopmentContextProvider> Providers { get; }

    /// <summary>
    /// Создать контекст разработки.
    /// </summary>
    /// <returns>Контекст разработки.</returns>
    IDevelopmentContext Create();
  }
}
