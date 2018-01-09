using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик блоков типовых маршрутов.
  /// </summary>
  internal class StandardRouteDatabaseHandler : IDatabaseHandler<StandardRoute>
  {
    #region IDatabaseHandler

    public IEnumerable<StandardRoute> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<StandardRoute>();

      var query = this.GetStandardRouteQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var standardRoute = new StandardRoute();
            standardRoute.Name = (reader["Name"] as string).Trim();
            standardRoute.Title = (reader["Title"] as string).Trim();
            if (ActiveValue.Equals(reader["State"] as string))
              standardRoute.State = ComponentState.Active;
            else
              standardRoute.State = ComponentState.Closed;
            var workflowDescriptionRawData = reader["WorkflowDescription"] as byte[];
            if (workflowDescriptionRawData != null)
            {
              var workflowDescription = Encoding.GetEncoding(1251).GetString(workflowDescriptionRawData);
              if (!string.IsNullOrWhiteSpace(workflowDescription))
              {
                standardRoute.WorkflowDescription = WorkflowDescriptionParser.Parse(workflowDescription);
              }
            }
            components.Add(standardRoute);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос типовых маршрутов для версии 7.7 и выше.
    /// </summary>
    private const string StandardRouteQuery_7_7 =
@"select
  a.Kod as [Name],
  a.NameAn as [Title],
  a.Sost as [State],
  t.SearchCondition as [WorkflowDescription]
from 
  MBAnalit a
  join MBVidAn v
    on v.Vid = a.Vid
  join MBText t
    on t.SrcRecID = a.XRecID
where
  v.Kod = 'ТМТ'";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос типовых маршрутов.
    /// </summary>
    public string GetStandardRouteQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return StandardRouteQuery_7_7;
      return null;
    }

    #endregion
  }
}
