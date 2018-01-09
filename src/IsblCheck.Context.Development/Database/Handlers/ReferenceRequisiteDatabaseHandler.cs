using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик реквизитов справочников.
  /// </summary>
  internal class ReferenceRequisiteDatabaseHandler : IDatabaseHandler<ReferenceRequisite>
  {
    #region IDatabaseHandler

    public IEnumerable<ReferenceRequisite> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<ReferenceRequisite>();
      var query = this.GetReferenceRequisiteQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var referenceRequisite = new ReferenceRequisite();
            referenceRequisite.Name = reader["Name"] as string;
            referenceRequisite.Title = reader["Title"] as string;

            var requisiteSectionValue = reader["Section"] as string;
            if (requisiteSectionValue != null &&
              RequisiteSectionValues.ContainsKey(requisiteSectionValue))
              referenceRequisite.Section = RequisiteSectionValues[requisiteSectionValue];

            var requisiteTypeValue = reader["Type"] as string;
            if (requisiteTypeValue != null &&
              RequisiteTypeValues.ContainsKey(requisiteTypeValue))
              referenceRequisite.Type = RequisiteTypeValues[requisiteTypeValue];

            var requisiteFormatValue = reader["Format"] as string;

            if (requisiteFormatValue != null &&
              RequisiteFormatValues.ContainsKey(requisiteFormatValue))
              referenceRequisite.Format = RequisiteFormatValues[requisiteFormatValue];

            var lengthValue = reader["Length"];
            if (lengthValue is int)
              referenceRequisite.Length = (int)lengthValue;

            var precissionValue = reader["Precission"];
            if (precissionValue is int)
              referenceRequisite.Precission = (int)precissionValue;

            referenceRequisite.ReferenceType = reader["ReferenceType"] as string;
            referenceRequisite.ReferenceView = reader["ReferenceView"] as string;

            var pickValuesValue = reader["PickValues"] as string;
            if (pickValuesValue != null)
            {
              var pickValues = pickValuesValue.Split(';');
              foreach (var pickValue in pickValues)
              {
                var pickValueParts = pickValue.Split('=', '|');
                var reqPickValue = new RequisitePickValue();
                reqPickValue.Id = pickValueParts[0][0];
                reqPickValue.Value = pickValueParts[1];
                referenceRequisite.PickValues.Add(reqPickValue);
              }
            }

            referenceRequisite.Field = reader["Field"] as string;
            referenceRequisite.IsStored = IsStoredValue.Equals(reader["IsStored"] as string);
            referenceRequisite.IsGenerated = IsGeneratedValue.Equals(reader["IsGenerated"] as string);

            components.Add(referenceRequisite);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос реквизитов справочников для версии 7.7 и выше.
    /// </summary>
    private const string ReferenceRequisiteQuery_7_7 = @"
      SELECT
        [ReferenceRequisites].[Kod] as [Name],
        [ReferenceRequisites].[Name] as [Title],
        [ReferenceRequisites].[Razd] as [Section],
        [ReferenceRequisites].[Type] as [Type],
        [ReferenceRequisites].[Align] as [Format],
        [ReferenceRequisites].[Len] as [Length],
        [ReferenceRequisites].[Toch] as [Precission],
        [ReferenceTypes].[Kod] as [ReferenceType],
        [ReferenceViews].[Kod] as [ReferenceView],
        [ReferenceRequisites].[PriznValues] as [PickValues],
        [ReferenceRequisites].[FldName] as [Field],
        [ReferenceRequisites].[Stored] as [IsStored],
        [ReferenceRequisites].[ExistFld] as [IsGenerated]
      FROM
        [dbo].[MBRecvAn] [ReferenceRequisites]
      LEFT JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [ReferenceRequisites].[VidAn]
      LEFT JOIN
        [dbo].[MBVidAnView] [ReferenceViews]
      ON
        [ReferenceViews].[XRecID] = [ReferenceRequisites].[ViewID]
      ORDER BY
        [Name]";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string IsStoredValue = "Д";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string IsGeneratedValue = "Е";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Секция.
    /// </summary>
    private static readonly Dictionary<string, RequisiteSection> RequisiteSectionValues
      = new Dictionary<string, RequisiteSection>
    {
      { "Ш", RequisiteSection.Card },
      { "Т", RequisiteSection.Table },
      { "С", RequisiteSection.Table2 },
      { "Р", RequisiteSection.Table3 },
      { "О", RequisiteSection.Table4 },
      { "Н", RequisiteSection.Table5 },
      { "М", RequisiteSection.Table6 },
      { "Q", RequisiteSection.Table7 },
      { "W", RequisiteSection.Table8 },
      { "U", RequisiteSection.Table9 },
      { "R", RequisiteSection.Table10 },
      { "I", RequisiteSection.Table11 },
      { "Y", RequisiteSection.Table12 },
      { "B", RequisiteSection.Table13 },
      { "H", RequisiteSection.Table14 },
      { "L", RequisiteSection.Table15 },
      { "M", RequisiteSection.Table16 },
      { "N", RequisiteSection.Table17 },
      { "P", RequisiteSection.Table18 },
      { "O", RequisiteSection.Table19 },
      { "S", RequisiteSection.Table20 },
      { "T", RequisiteSection.Table21 },
      { "V", RequisiteSection.Table22 },
      { "X", RequisiteSection.Table23 },
      { "Z", RequisiteSection.Table24 }
    };

    /// <summary>
    /// Тип.
    /// </summary>
    private static readonly Dictionary<string, RequisiteType> RequisiteTypeValues
      = new Dictionary<string, RequisiteType>
    {
      { "Д", RequisiteType.Date },
      { "Ч", RequisiteType.Float },
      { "П", RequisiteType.Pick },
      { "А", RequisiteType.Reference },
      { "С", RequisiteType.String },
      { "М", RequisiteType.Text },
      { "Ц", RequisiteType.Integer },
      { "L", RequisiteType.BigInteger },
      { "E", RequisiteType.Document }
    };

    /// <summary>
    /// Формат.
    /// </summary>
    private static readonly Dictionary<string, RequisiteFormat> RequisiteFormatValues
      = new Dictionary<string, RequisiteFormat>
    {
      { "Л", RequisiteFormat.Left },
      { "П", RequisiteFormat.Right },
      { "Д", RequisiteFormat.Date },
      { "Ц", RequisiteFormat.DateAndTime }
    };

    #endregion

    #region Методы

    /// <summary>
    /// Запрос реквизитов справочников.
    /// </summary>
    public string GetReferenceRequisiteQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return ReferenceRequisiteQuery_7_7;
      return null;
    }

    #endregion
  }
}
