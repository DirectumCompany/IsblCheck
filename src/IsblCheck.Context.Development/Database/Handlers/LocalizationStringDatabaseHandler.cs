using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик строк локализации.
  /// </summary>
  internal class LocalizationStringDatabaseHandler : IDatabaseHandler<LocalizationString>
  {
    #region IDatabaseHandler

    public IEnumerable<LocalizationString> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new Dictionary<string, LocalizationString>();
      var query = this.GetLocalizationStringQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components.Values.ToList();

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var localizationString = new LocalizationString
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string,
              Group = reader["Group"] as string
            };

            components.Add(localizationString.Name, localizationString);
          }
        }
      }

      query = this.GetLocalizationValueQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var localizationStringName = reader["LocalizationStringName"] as string;
              if (!components.ContainsKey(localizationStringName))
                continue;
              var localizationString = components[localizationStringName];

              var localizationValue = new LocalizationValue
              {
                Language = reader["Language"] as string,
                Value = reader["Value"] as string
              };

              localizationString.Values.Add(localizationValue);
            }
          }
        }
      }

      return components.Values.ToList();
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос строк локализации для версии 7.7 и выше.
    /// </summary>
    private const string LocalizationStringQuery_7_7 = @"
      SELECT DISTINCT
        [Code] as [Name],
        [Code] as [Title],
        [GroupCode] as [Group]
      FROM
        [dbo].[SBLocalizedData]
      ORDER BY
        [Name]";

    /// <summary>
    /// Запрос значений строк локализаций для версии 7.7 и выше.
    /// </summary>
    private const string LocalizationStringValueQuery_7_7 = @"
      SELECT
        [Code] as [LocalizationStringName],
        [LangCode] as [Language],
        [String] as [Value]
      FROM
        [dbo].[SBLocalizedData]
      ORDER BY
        [LocalizationStringName],
        [Language]";

    #endregion

    #region Методы

    /// <summary>
    /// Запрос строк локализации.
    /// </summary>
    public string GetLocalizationStringQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return LocalizationStringQuery_7_7;
      return null;
    }

    /// <summary>
    /// Запрос значений строк локализаций.
    /// </summary>
    public string GetLocalizationValueQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return LocalizationStringValueQuery_7_7;
      return null;
    }

    #endregion
  }
}
