using System.Collections.Generic;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Интерфейс обработчика пакета.
  /// </summary>
  internal interface IPackageHandler<out T> where T: Component
  {
    /// <summary>
    /// Прочитать сущности заданного типа из пакета.
    /// </summary>
    /// <param name="packageModel">Модель пакета.</param>
    /// <returns>Список сущностей.</returns>
    IEnumerable<T> Read(ComponentsModel packageModel);
  }
}
