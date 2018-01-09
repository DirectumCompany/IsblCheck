using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик мастеров действий.
  /// </summary>
  internal class WizardDatabaseHandler : IDatabaseHandler<Wizard>
  {
    #region IDatabaseHandler

    public IEnumerable<Wizard> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<Wizard>();

      var query = this.GetWizardsQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var wizard = new Wizard();
            wizard.Name = (reader["Name"] as string).Trim();
            wizard.Title = (reader["Title"] as string).Trim();
            if (ActiveValue.Equals(reader["State"] as string))
              wizard.State = ComponentState.Active;
            else
              wizard.State = ComponentState.Closed;
            var wizardDfmRawData = reader["WizardDfm"] as byte[];
            if (wizardDfmRawData != null)
            {
              var wizardDfm = Encoding.GetEncoding(1251).GetString(wizardDfmRawData);
              if (!string.IsNullOrWhiteSpace(wizardDfm))
              {
                var dfmWizard = WizardDfmParser.Parse(wizardDfm);
                if (dfmWizard != null)
                {
                  wizard.Events.AddRange(dfmWizard.Events);
                  wizard.Steps.AddRange(dfmWizard.Steps);
                }
              }
            }
            components.Add(wizard);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос мастеров действий для версии 7.7 и выше.
    /// </summary>
    private const string WizardsQuery_7_7 =
@"select
  a.Kod as [Name],
  a.NameAn as [Title],
  a.Sost as [State],
  t.SearchCondition as [WizardDfm]
from 
  MBAnalit a
  join MBVidAn v
    on v.Vid = a.Vid
  join MBText t
    on t.SrcRecID = a.XRecID
where
  v.Kod = 'WIZARDS'";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос мастеров действий.
    /// </summary>
    public string GetWizardsQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return WizardsQuery_7_7;
      return null;
    }

    #endregion
  }
}
