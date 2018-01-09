using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal class CommonReportPackageHandler : ReportPackageHandler, IPackageHandler<CommonReport>
  {
    #region Константы

    /// <summary>
    /// Тип.
    /// </summary>
    private const string IsIntegratedReqName = "Тип";

    /// <summary>
    /// Интегрированный тип.
    /// </summary>
    private const string IsCommon = "MBAnAccRpt";

    #endregion

    #region IPackageHandler

    public IEnumerable<CommonReport> Read(ComponentsModel packageModel)
    {
      var models = packageModel.Reports
        .Where(m => m.Card.Requisites.First(r => r.Code == IsIntegratedReqName).DecodedText == IsCommon);

      foreach (var model in models)
      {
        var entity = this.Read<CommonReport>(model);

        yield return entity;
      }
    }

    #endregion
  }
}
