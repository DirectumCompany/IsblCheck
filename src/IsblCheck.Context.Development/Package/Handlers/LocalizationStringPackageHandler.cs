using System.Collections.Generic;
using System.Linq;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик строк локализации.
  /// </summary>
  internal class LocalizationStringPackageHandler : IPackageHandler<LocalizationString>
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

    #region IPackageHandler

    public IEnumerable<LocalizationString> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.LocalizationStrings)
      {
        var entity = PackageHandlerUtils.CreateEntity<LocalizationString>(model);

        if (model.DetailDataSets?.DetailDataSet1 != null)
        {
          var stringModels = model.DetailDataSets.DetailDataSet1.Rows;
          foreach (var stringModel in stringModels)
          {
            var localizationValue = new LocalizationValue();

            var groupReq = stringModel.Requisites
              .FirstOrDefault(r => r.Code == GroupReqName);
            if (groupReq != null)
              entity.Group = groupReq.DecodedText;

            var languageReq = stringModel.Requisites
              .FirstOrDefault(r => r.Code == LanguageReqName);
            if (languageReq != null)
              localizationValue.Language = languageReq.DecodedText;

            var valueReq = stringModel.Requisites
              .FirstOrDefault(r => r.Code == ValueReqName);
            if (valueReq != null)
              localizationValue.Value = valueReq.DecodedText;

            entity.Values.Add(localizationValue);
          }
        }

        yield return entity;
      }
    }

    #endregion
  }
}
