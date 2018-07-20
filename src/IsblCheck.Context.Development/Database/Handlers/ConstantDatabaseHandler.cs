using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик констант.
  /// </summary>
  internal class ConstantDatabaseHandler : IDatabaseHandler<Constant>
  {
    #region IDatabaseHandler

    public IEnumerable<Constant> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<Constant>();
      var query = this.GetConstantQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var constant = new Constant
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string,
              IsCommon = YesValue.Equals(reader["IsCommon"] as string),
              IsReplicated = YesValue.Equals(reader["IsReplicated"] as string)
            };

            components.Add(constant);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос констант для версии 7.7 и выше.
    /// </summary>
    private const string ConstantQuery_7_7 = @"
      SELECT
        [Const] as [Name],
        [Nazn] as [Title],
        [IsAllFirm] as [IsCommon],
        [IsGlob] as [IsReplicated]
      FROM
        [dbo].[MBConstLst]
      ORDER BY
        [Name]";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string YesValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос констант.
    /// </summary>
    public string GetConstantQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ConstantQuery_7_7;
      return null;
    }

    #endregion
  }
}
