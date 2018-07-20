using System.Collections.Generic;
using System.Linq;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик отчетов.
  /// </summary>
  internal class IntegratedReportPackageHandler : ReportPackageHandler, IPackageHandler<IntegratedReport>
  {
    #region Константы

    /// <summary>
    /// Тип.
    /// </summary>
    private const string IsIntegratedReqName = "Тип";

    /// <summary>
    /// Интегрированный тип.
    /// </summary>
    private const string IsIntegrated = "MBAnalitV";

    #endregion

    #region IPackageHandler

    public IEnumerable<IntegratedReport> Read(ComponentsModel packageModel)
    {
      var models = packageModel.Reports
        .Where(m => m.Card.Requisites.First(r => r.Code == IsIntegratedReqName).DecodedText == IsIntegrated);

      foreach (var model in models)
      {
        var entity = this.Read<IntegratedReport>(model);
        entity.ReferenceName = model.ReferenceName;

        yield return entity;
      }
    }

    #endregion
  }
}
