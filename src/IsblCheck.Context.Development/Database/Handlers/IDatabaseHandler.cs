using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Интерфейс хендлера БД.
  /// </summary>
  internal interface IDatabaseHandler<out T> where T: Component
  {
    /// <summary>
    /// Прочитать сущности заданного типа из БД.
    /// </summary>
    /// <param name="connection">Подключение к БД.</param>
    /// <param name="platformVersion">Версия платформы.</param>
    /// <returns>Список сущностей.</returns>
    IEnumerable<T> Read(SqlConnection connection, Version platformVersion);
  }
}
