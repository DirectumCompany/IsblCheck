using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Интерфейс обработчика пакета.
  /// </summary>
  internal interface IPackageHandler<T> where T: Component
  {
    /// <summary>
    /// Прочитать сущности заданного типа из пакета.
    /// </summary>
    /// <param name="packageModel">Модель пакета.</param>
    /// <returns>Список сущностей.</returns>
    IEnumerable<T> Read(ComponentsModel packageModel);
  }
}
