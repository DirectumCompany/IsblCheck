using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик приложений просмотрщиков.
  /// </summary>
  internal class ViewerDatabaseHandler : IDatabaseHandler<Viewer>
  {
    #region IDatabaseHandler

    public IEnumerable<Viewer> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<Viewer>();
      var query = this.GetViewerQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var viewer = new Viewer
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string,
              Extension = reader["Extension"] as string
            };

            if (reader["ViewerType"] is string viewerTypeValue &&
              ViewerTypeValues.ContainsKey(viewerTypeValue))
              viewer.ViewerType = ViewerTypeValues[viewerTypeValue];

            components.Add(viewer);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос приложений просмотрщиков для версии 7.7 и выше.
    /// </summary>
    private const string ViewerQuery_7_7 = @"
      SELECT
        [Viewer] as [Name],
        [AppName] as [Title],
        [AppExt] as [Extension],
        [TypeEditor] as [ViewerType]
      FROM
        [dbo].[MBRptView]
      ORDER BY
        [Name]";

    #endregion

    #region Методы

    /// <summary>
    /// Типы аргументов функций.
    /// </summary>
    private static readonly Dictionary<string, ViewerType> ViewerTypeValues
      = new Dictionary<string, ViewerType>
    {
      { "С", ViewerType.CrystalReports },
      { "E", ViewerType.Excel },
      { "R", ViewerType.Word },
      { "W", ViewerType.Internal }
    };

    /// <summary>
    /// Запрос приложений просмотрщиков.
    /// </summary>
    public string GetViewerQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ViewerQuery_7_7;
      return null;
    }

    #endregion
  }
}
