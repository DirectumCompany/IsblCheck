using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик реквизитов диалогов.
  /// </summary>
  internal class DialogRequisitePackageHandler : IPackageHandler<DialogRequisite>
  {
    #region Константы

    /// <summary>
    /// Раздел.
    /// </summary>
    private const string SectionReqName = "ISBDialogRequisiteSection";

    /// <summary>
    /// Тип.
    /// </summary>
    private const string TypeReqName = "ISBDialogRequisiteType";

    /// <summary>
    /// Формат.
    /// </summary>
    private const string FormatReqName = "ISBDialogRequisiteFormat";

    /// <summary>
    /// Длина.
    /// </summary>
    private const string LengthReqName = "ISBDialogRequisiteLength";

    /// <summary>
    /// Точность.
    /// </summary>
    private const string PrecissionReqName = "ISBDialogRequisitePrecision";

    /// <summary>
    /// Тип справочника.
    /// </summary>
    private const string ReferenceTypeReqName = "ISBDialogRequisiteReference";

    /// <summary>
    /// Представление справочника.
    /// </summary>
    private const string ReferenceViewReqName = "ISBDialogRequisiteView";

    /// <summary>
    /// Представление справочника.
    /// </summary>
    private const string PickValuesReqName = "ISBDialogRequisitePickValues";

    #endregion

    #region IPackageHandler

    public IEnumerable<DialogRequisite> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.DialogRequisites)
      {
        var entity = PackageHandlerUtils.CreateEntity<DialogRequisite>(model);

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
        if (lengthReq != null && !string.IsNullOrEmpty(lengthReq.Value))
          entity.Length = int.Parse(lengthReq.Value);

        var precissionReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == PrecissionReqName);
        if (precissionReq != null && !string.IsNullOrEmpty(precissionReq.Value))
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
        if (pickValuesReq != null && !string.IsNullOrEmpty(pickValuesReq.DecodedText))
        {
          var pickValues = pickValuesReq.DecodedText.Split(';');
          foreach (var pickValue in pickValues)
          {
            var pickValueParts = pickValue.Split('=', '|');
            var reqPickValue = new RequisitePickValue();
            reqPickValue.Id = pickValueParts[0][0];
            reqPickValue.Value = pickValueParts[1];
            entity.PickValues.Add(reqPickValue);
          }
        }

        yield return entity;
      }
    }

    #endregion
  }
}
