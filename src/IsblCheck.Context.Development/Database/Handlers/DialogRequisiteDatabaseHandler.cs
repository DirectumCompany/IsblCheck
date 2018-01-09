using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик реквизитов диалогов.
  /// </summary>
  internal class DialogRequisiteDatabaseHandler : IDatabaseHandler<DialogRequisite>
  {
    #region IDatabaseHandler

    public IEnumerable<DialogRequisite> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new List<DialogRequisite>();
      var query = this.GetDialogRequisiteQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components;

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var dialogRequisite = new DialogRequisite();
            dialogRequisite.Name = reader["Name"] as string;
            dialogRequisite.Title = reader["Title"] as string;

            var requisiteSectionValue = reader["Section"] as string;
            if (requisiteSectionValue != null &&
              RequisiteSectionValues.ContainsKey(requisiteSectionValue))
              dialogRequisite.Section = RequisiteSectionValues[requisiteSectionValue];

            var requisiteTypeValue = reader["Type"] as string;
            if (requisiteTypeValue != null &&
              RequisiteTypeValues.ContainsKey(requisiteTypeValue))
              dialogRequisite.Type = RequisiteTypeValues[requisiteTypeValue];

            var requisiteFormatValue = reader["Format"] as string;

            if (requisiteFormatValue != null &&
              RequisiteFormatValues.ContainsKey(requisiteFormatValue))
              dialogRequisite.Format = RequisiteFormatValues[requisiteFormatValue];

            var lengthValue = reader["Length"];
            if (lengthValue is int)
              dialogRequisite.Length = (int)lengthValue;

            var precissionValue = reader["Precission"];
            if (precissionValue is int)
              dialogRequisite.Precission = (int)precissionValue;

            dialogRequisite.ReferenceType = reader["ReferenceType"] as string;
            dialogRequisite.ReferenceView = reader["ReferenceView"] as string;

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
                dialogRequisite.PickValues.Add(reqPickValue);
              }
            }

            components.Add(dialogRequisite);
          }
        }
      }
      return components;
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос реквизитов диалогов для версии 7.15 и выше.
    /// </summary>
    private const string DialogRequisiteQuery_7_15 = @"
      SELECT
        [DialogRequisites].[Code] as [Name],
        [DialogRequisites].[Name] as [Title],
        [DialogRequisites].[Section] as [Section],
        [DialogRequisites].[Type] as [Type],
        [DialogRequisites].[Align] as [Format],
        [DialogRequisites].[Length] as [Length],
        [DialogRequisites].[Precision] as [Precission],
        [ReferenceTypes].[Kod] as [ReferenceType],
        [ReferenceViews].[Kod] as [ReferenceView],
        [DialogRequisites].[PickValues] as [PickValues]
      FROM
        [dbo].[SBDialogRequisite] [DialogRequisites]
      LEFT JOIN
        [dbo].[MBVidAn] [ReferenceTypes]
      ON
        [ReferenceTypes].[Vid] = [DialogRequisites].[ReferenceType]
      LEFT JOIN
        [dbo].[MBVidAnView] [ReferenceViews]
      ON
        [ReferenceViews].[XRecID] = [DialogRequisites].[ReferenceViewID]
      ORDER BY
        [Name]";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Секция.
    /// </summary>
    private static readonly Dictionary<string, RequisiteSection> RequisiteSectionValues
      = new Dictionary<string, RequisiteSection>
    {
      { "C", RequisiteSection.Card },
      { "D", RequisiteSection.Table },
      { "E", RequisiteSection.Table2 },
      { "F", RequisiteSection.Table3 },
      { "G", RequisiteSection.Table4 },
      { "J", RequisiteSection.Table5 },
      { "K", RequisiteSection.Table6 },
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
      { "D", RequisiteType.Date },
      { "F", RequisiteType.Float },
      { "P", RequisiteType.Pick },
      { "R", RequisiteType.Reference },
      { "S", RequisiteType.String },
      { "T", RequisiteType.Text },
      { "I", RequisiteType.Integer },
      { "L", RequisiteType.BigInteger },
      { "E", RequisiteType.Document }
    };


    /// <summary>
    /// Формат.
    /// </summary>
    private static readonly Dictionary<string, RequisiteFormat> RequisiteFormatValues
      = new Dictionary<string, RequisiteFormat>
    {
      { "L", RequisiteFormat.Left },
      { "R", RequisiteFormat.Right },
      { "D", RequisiteFormat.Date },
      { "T", RequisiteFormat.DateAndTime }
    };

    #endregion

    #region Методы

    /// <summary>
    /// Запрос реквизитов диалогов.
    /// </summary>
    public string GetDialogRequisiteQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 15);
      if (platformVersion > minVersion)
        return DialogRequisiteQuery_7_15;
      return null;
    }

    #endregion
  }
}
