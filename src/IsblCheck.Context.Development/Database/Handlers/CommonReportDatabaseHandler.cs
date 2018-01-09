using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик общих отчетов.
  /// </summary>
  internal class CommonReportDatabaseHandler : IDatabaseHandler<CommonReport>
  {
    #region IDatabaseHandler

    public IEnumerable<CommonReport> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<CommonReport>();
      var query = this.GetCommonReportQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var commonReport = new CommonReport();
            commonReport.Name = reader["Name"] as string;
            commonReport.Title = reader["Title"] as string;
            commonReport.Viewer = reader["Viewer"] as string;
            if (ActiveValue.Equals(reader["State"] as string))
              commonReport.State = ComponentState.Active;
            else
              commonReport.State = ComponentState.Closed;

            commonReport.CalculationText = reader["CalculationText"] as string;
            if (commonReport.CalculationText == null)
              commonReport.CalculationText = string.Empty;

            var templateValue = reader["TemplateText"] as byte[];
            if (templateValue != null)
              commonReport.TemplateText = Encoding.GetEncoding(1251).GetString(templateValue);
            else
              commonReport.TemplateText = string.Empty;

            components.Add(commonReport);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос общих отчетов для версии 7.7 и выше.
    /// </summary>
    private const string CommonReportQuery_7_7 = @"
      SELECT 
        [NameRpt] as [Name],
        [Description] as [Title],
        [Viewer] as [Viewer],
        [Sost] as [State],
        [Exprn] as [CalculationText],
        [Report] as [TemplateText]
      FROM
        [dbo].[MBReports]
      WHERE
        [TypeRpt] = 'MBAnAccRpt'
      ORDER BY
        [Name]";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Получить запрос общих отчетов.
    /// </summary>
    public string GetCommonReportQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return CommonReportQuery_7_7;
      return null;
    }

    #endregion
  }
}
