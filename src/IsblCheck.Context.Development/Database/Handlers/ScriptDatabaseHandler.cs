using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик сценариев.
  /// </summary>
  internal class ScriptDatabaseHandler : IDatabaseHandler<Script>
  {
    #region IDatabaseHandler

    public IEnumerable<Script> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<Script>();
      var query = this.GetScriptQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var script = new Script
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string
            };
            if (ActiveValue.Equals(reader["State"] as string))
              script.State = ComponentState.Active;
            else
              script.State = ComponentState.Closed;
            if (reader["CalculationText"] is byte[] calculationValue)
              script.CalculationText = Encoding.GetEncoding(1251).GetString(calculationValue);
            else
              script.CalculationText = string.Empty;

            components.Add(script);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос сценариев для версии 7.7 и выше.
    /// </summary>
    private const string ScriptQuery_7_7 = @"
      SELECT 
        [NameRpt] as [Name],
        [Description] as [Title],
        [Sost] as [State],
        [Report] as [CalculationText]
      FROM
        [dbo].[MBReports]
      WHERE
        [TypeRpt] = 'Function'
      ORDER BY
        [Name]";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос сценариев.
    /// </summary>
    public string GetScriptQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ScriptQuery_7_7;
      return null;
    }

    #endregion
  }
}
