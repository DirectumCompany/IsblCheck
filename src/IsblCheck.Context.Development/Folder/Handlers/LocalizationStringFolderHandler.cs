using IsblCheck.Context.Development.Folder.ExportModels;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal class LocalizationStringFolderHandler : FolderHandlerBase<LocalizationString, LocalizationStringsExportModel>
  {
    #region Константы

    /// <summary>
    /// Группа строки локализации.
    /// </summary>
    private const string GroupReqName = "ISBGroupCode";

    /// <summary>
    /// Язык строки локализации.
    /// </summary>
    private const string LanguageReqName = "ISBLanguage";

    /// <summary>
    /// Значение строки локализации.
    /// </summary>
    private const string ValueReqName = "ISBString";

    #endregion

    #region FolderHandlerBase

    protected override string FolderName { get { return "LocalizationStrings"; } }

    protected override string CardModelRootNode { get { return "LocalizationStrings"; } }

    protected override IEnumerable<LocalizationString> ReadComponents(LocalizationStringsExportModel model, string componentFolderPath)
    {
      foreach (var localizationStringModel in model.LocalizationStrings)
      {
        var entity = PackageHandlerUtils.CreateEntity<LocalizationString>(localizationStringModel);

        if (localizationStringModel.DetailDataSets != null &&
            localizationStringModel.DetailDataSets.DetailDataSet1 != null)
        {
          var stringModels = localizationStringModel.DetailDataSets.DetailDataSet1.Rows;
          foreach (var stringModel in stringModels)
          {
            var localizationValue = new LocalizationValue();

            var groupReq = stringModel.Requisites
              .FirstOrDefault(r => r.Code == GroupReqName);
            if (groupReq != null)
              entity.Group = groupReq.Value;

            var languageReq = stringModel.Requisites
              .FirstOrDefault(r => r.Code == LanguageReqName);
            if (languageReq != null)
              localizationValue.Language = languageReq.Value;

            var valueReq = stringModel.Requisites
              .FirstOrDefault(r => r.Code == ValueReqName);
            if (valueReq != null)
              localizationValue.Value = valueReq.Value;

            entity.Values.Add(localizationValue);
          }
        }

        yield return entity;
      }
    } 

    #endregion
  }
}
