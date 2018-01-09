using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик интегрированных отчетов.
  /// </summary>
  internal class IntegratedReportDatabaseHandler : IDatabaseHandler<IntegratedReport>
  {
    #region IDatabaseHandler

    public IEnumerable<IntegratedReport> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<IntegratedReport>();
      var query = this.GetIntegratedReportQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var integratedReport = new IntegratedReport();
            integratedReport.Name = reader["Name"] as string;
            integratedReport.Title = reader["Title"] as string;
            integratedReport.Viewer = reader["Viewer"] as string;
            if (ActiveValue.Equals(reader["State"] as string))
              integratedReport.State = ComponentState.Active;
            else
              integratedReport.State = ComponentState.Closed;

            integratedReport.CalculationText = reader["CalculationText"] as string;
            if (integratedReport.CalculationText == null)
              integratedReport.CalculationText = string.Empty;

            var templateValue = reader["TemplateText"] as byte[];
            if (templateValue != null)
              integratedReport.TemplateText = Encoding.GetEncoding(1251).GetString(templateValue);
            else
              integratedReport.TemplateText = string.Empty;

            integratedReport.ReferenceName = reader["ReferenceName"] as string;

            components.Add(integratedReport);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос интегрированных отчетов для версии 7.7 и выше.
    /// </summary>
    private const string IntegratedReportQuery_7_7 = @"
      SELECT 
        [Reports].[NameRpt] as [Name],
        [Reports].[Description] as [Title],
        [Reports].[Viewer] as [Viewer],
        [Reports].[Sost] as [State],
        [Reports].[Exprn] as [CalculationText],
        [Reports].[Report] as [TemplateText],
        [ReferenceTypes].[Kod] as [ReferenceName]
      FROM
        [dbo].[MBReports] [Reports]
      JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [Reports].[RegUnit]
      WHERE
        [Reports].[TypeRpt] = 'MBAnalitV'
      ORDER BY
        [Name]";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос интегрированных отчетов.
    /// </summary>
    public string GetIntegratedReportQuery(Version platformVersion)
    {
        var minVersion = new Version(7, 7);
        if (platformVersion > minVersion)
          return IntegratedReportQuery_7_7;
        return null;
    }

    #endregion
  }
}
