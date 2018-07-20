using System.Collections.Generic;
using System.Linq;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик реквизитов справочников.
  /// </summary>
  internal class ReferenceRequisitePackageHandler : IPackageHandler<ReferenceRequisite>
  {
    #region Константы

    /// <summary>
    /// Поле.
    /// </summary>
    private const string FieldReqName = "ISBRefReqFieldName";

    /// <summary>
    /// Раздел.
    /// </summary>
    private const string SectionReqName = "ISBRefReqSection";

    /// <summary>
    /// Тип.
    /// </summary>
    private const string TypeReqName = "ISBRefReqType";

    /// <summary>
    /// Формат.
    /// </summary>
    private const string FormatReqName = "ISBRefReqFormat";

    /// <summary>
    /// Длина.
    /// </summary>
    private const string LengthReqName = "ISBRefReqLength";

    /// <summary>
    /// Точность.
    /// </summary>
    private const string PrecissionReqName = "ISBRefReqPrecision";

    /// <summary>
    /// Тип справочника.
    /// </summary>
    private const string ReferenceTypeReqName = "ISBRefReqReference";

    /// <summary>
    /// Представление справочника.
    /// </summary>
    private const string ReferenceViewReqName = "ISBRefReqView";

    /// <summary>
    /// Представление справочника.
    /// </summary>
    private const string PickValuesReqName = "ISBRefReqTokens";

    /// <summary>
    /// Признак хранения значения.
    /// </summary>
    private const string IsStoredReqName = "ISBRefReqStored";

    /// <summary>
    /// Признак сгенерированности.
    /// </summary>
    private const string IsGeneratedReqName = "ISBRefReqGenerated";

    /// <summary>
    /// Хранимый
    /// </summary>
    private const string Stored = "SYSRES_COMMON.YES_CONST";

    /// <summary>
    /// Хранимый
    /// </summary>
    private const string Generated = "SYSRES_COMMON.YES_CONST";

    #endregion

    #region IPackageHandler

    public IEnumerable<ReferenceRequisite> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.ReferenceRequisites)
      {
        var entity = PackageHandlerUtils.CreateEntity<ReferenceRequisite>(model);

        var fieldReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == FieldReqName);
        if (fieldReq != null)
          entity.Field = fieldReq.DecodedText;

        var sectionReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == SectionReqName);
        if (sectionReq != null)
          entity.Section = RequisiteHandlerUtils.GetRequisiteSection(sectionReq.ValueLocalizeID);

        var typeReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == TypeReqName);
        if (typeReq != null)
          entity.Type = RequisiteHandlerUtils.GetRequisiteType(typeReq.ValueLocalizeID);

        var formatReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == FormatReqName);
        if (formatReq != null)
          entity.Format = RequisiteHandlerUtils.GetRequisiteFormat(formatReq.ValueLocalizeID);

        var lengthReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == LengthReqName);
        if (!string.IsNullOrEmpty(lengthReq?.Value))
          entity.Length = int.Parse(lengthReq.Value);

        var precissionReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == PrecissionReqName);
        if (!string.IsNullOrEmpty(precissionReq?.Value))
          entity.Precission = int.Parse(precissionReq.Value);

        var referenceTypeReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == ReferenceTypeReqName);
        if (referenceTypeReq != null)
          entity.ReferenceType = referenceTypeReq.Value;

        var referenceViewReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == ReferenceViewReqName);
        if (referenceViewReq != null)
          entity.ReferenceView = referenceViewReq.Value;

        var pickValuesReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == PickValuesReqName);
        if (!string.IsNullOrEmpty(pickValuesReq?.DecodedText))
        {
          var pickValues = pickValuesReq.DecodedText.Split(';');
          foreach (var pickValue in pickValues)
          {
            var pickValueParts = pickValue.Split('=', '|');
            var reqPickValue = new RequisitePickValue
            {
              Id = pickValueParts[0][0],
              Value = pickValueParts[1]
            };
            entity.PickValues.Add(reqPickValue);
          }
        }

        var isStoredReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == IsStoredReqName);
        if (isStoredReq != null)
          entity.IsStored = isStoredReq.ValueLocalizeID == Stored;

        var isGeneratedReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == IsGeneratedReqName);
        if (isGeneratedReq != null)
          entity.IsGenerated = isGeneratedReq.ValueLocalizeID == Generated;

        yield return entity;
      }
    }

    #endregion
  }
}
