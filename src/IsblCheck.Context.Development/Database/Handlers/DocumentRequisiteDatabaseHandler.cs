using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик реквизитов документов.
  /// </summary>
  internal class DocumentRequisiteDatabaseHandler : IDatabaseHandler<DocumentRequisite>
  {
    #region IDatabaseHandler

    public IEnumerable<DocumentRequisite> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<DocumentRequisite>();
      var query = this.GetDocumentRequisiteQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var documentRequisite = new DocumentRequisite();
            documentRequisite.Name = reader["Name"] as string;
            documentRequisite.Title = reader["Title"] as string;

            var requisiteSectionValue = reader["Section"] as string;
            if (requisiteSectionValue != null &&
              RequisiteSectionValues.ContainsKey(requisiteSectionValue))
              documentRequisite.Section = RequisiteSectionValues[requisiteSectionValue];

            var requisiteTypeValue = reader["Type"] as string;
            if (requisiteTypeValue != null &&
              RequisiteTypeValues.ContainsKey(requisiteTypeValue))
              documentRequisite.Type = RequisiteTypeValues[requisiteTypeValue];

            var requisiteFormatValue = reader["Format"] as string;

            if (requisiteFormatValue != null &&
              RequisiteFormatValues.ContainsKey(requisiteFormatValue))
              documentRequisite.Format = RequisiteFormatValues[requisiteFormatValue];

            var lengthValue = reader["Length"];
            if (lengthValue is int)
              documentRequisite.Length = (int)lengthValue;

            var precissionValue = reader["Precission"];
            if (precissionValue is int)
              documentRequisite.Precission = (int)precissionValue;

            documentRequisite.ReferenceType = reader["ReferenceType"] as string;
            documentRequisite.ReferenceView = reader["ReferenceView"] as string;

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
                documentRequisite.PickValues.Add(reqPickValue);
              }
            }

            documentRequisite.Field = reader["Field"] as string;
            documentRequisite.IsStored = IsStoredValue.Equals(reader["IsStored"] as string);
            documentRequisite.IsGenerated = IsGeneratedValue.Equals(reader["IsGenerated"] as string);

            components.Add(documentRequisite);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос реквизитов документов для версии 7.7 и выше.
    /// </summary>
    private const string DocumentRequisiteQuery_7_7 = @"
      SELECT
        [DocumentRequisites].[Kod] as [Name],
        [DocumentRequisites].[Name] as [Title],
        [DocumentRequisites].[Razd] as [Section],
        [DocumentRequisites].[Type] as [Type],
        [DocumentRequisites].[Align] as [Format],
        [DocumentRequisites].[Len] as [Length],
        [DocumentRequisites].[Toch] as [Precission],
        [ReferenceTypes].[Kod] as [ReferenceType],
        [ReferenceViews].[Kod] as [ReferenceView],
        [DocumentRequisites].[PriznValues] as [PickValues],
        [DocumentRequisites].[FldName] as [Field],
        [DocumentRequisites].[Stored] as [IsStored],
        [DocumentRequisites].[ExistFld] as [IsGenerated]
      FROM
        [dbo].[MBRecvEDoc] [DocumentRequisites]
      LEFT JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [DocumentRequisites].[VidAn]
      LEFT JOIN
        [dbo].[MBVidAnView] [ReferenceViews]
      ON
        [ReferenceViews].[XRecID] = [DocumentRequisites].[ViewID]
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
    /// Запрос реквизитов документов.
    /// </summary>
    public string GetDocumentRequisiteQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return DocumentRequisiteQuery_7_7;
      return null;
    }

    #endregion
  }
}
