using System.Collections.Generic;

namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Интерфейс провайдера данных разработки.
  /// </summary>
  public interface IDevelopmentContextProvider
  {
    /// <summary>
    /// Прочитать список компонент.
    /// </summary>
    /// <typeparam name="T">Тип компоненты.</typeparam>
    /// <returns>Список компонент указанного типа.</returns>
    IEnumerable<T> ReadComponents<T>() where T : Component;

    /// <summary>
    /// Сбросить кэш.
    /// </summary>
    void ResetCache();
  }
}
