using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик блоков типовых маршрутов.
  /// </summary>
  internal class RouteBlockDatabaseHandler : IDatabaseHandler<RouteBlock>
  {
    #region IDatabaseHandler

    public IEnumerable<RouteBlock> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<RouteBlock>();

      var query = this.GetRouteBlockQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var routeBlock = new RouteBlock
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string
            };
            if (ActiveValue.Equals(reader["State"] as string))
              routeBlock.State = ComponentState.Active;
            else
              routeBlock.State = ComponentState.Closed;
            if (Enum.TryParse(reader["BaseBlockType"] as string, out RouteBlockType blockType))
              routeBlock.BaseBlockType = blockType;
            else
              routeBlock.BaseBlockType = RouteBlockType.Unknown;
            if (reader["Properties"] is byte[] blockDescriptionRawData)
            {
              var blockDescription = Encoding.GetEncoding(1251).GetString(blockDescriptionRawData);
              if (!string.IsNullOrWhiteSpace(blockDescription))
              {
                var description = WorkflowDescriptionParser.Parse(blockDescription);
                routeBlock.WorkflowBlock = description.Blocks.FirstOrDefault();
              }
            }
            components.Add(routeBlock);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос блоков типовых маршрутов для версии 7.7 и выше.
    /// </summary>
    private const string RouteBlockQuery_7_7 =
@"select
  [Name] as [Name],
  [Title] as [Title],
  [State] as [State],
  [BaseBlockType] as [BaseBlockType],
  [Properties] as [Properties]
from 
  [dbo].[SBRouteBlock]";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос блоков типовых маршрутов.
    /// </summary>
    public string GetRouteBlockQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return RouteBlockQuery_7_7;
      return null;
    }

    #endregion
  }
}
