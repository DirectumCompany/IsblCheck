using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик сценариев.
  /// </summary>
  internal class ScriptFolderHandler : FolderHandlerBase<Script, ComponentModel>
  {
    #region Константы

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    /// <summary>
    /// Активная.
    /// </summary>
    private const string Active = "SYSRES_SYSCOMP.OPERATING_RECORD_FLAG_VALUE_MASCULINE";

    #endregion

    #region Поля и свойства

    private static readonly ILog log = LogManager.GetLogger<ScriptFolderHandler>(); 

    #endregion

    #region FolderHandlerBase

    protected override string FolderName => "Scripts";

    protected override string CardModelRootNode => "Script";

    protected override IEnumerable<Script> ReadComponents(ComponentModel model, string componentFolderPath)
    {
      var entity = PackageHandlerUtils.CreateEntity<Script>(model);

      var stateReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == StateReqName);
      if (stateReq != null)
        entity.State = stateReq.ValueLocalizeID == Active ? ComponentState.Active : ComponentState.Closed;

      var calculationTextFile = Path.Combine(componentFolderPath, "Text.isbl");
      if (File.Exists(calculationTextFile))
        entity.CalculationText = File.ReadAllText(calculationTextFile, Encoding.GetEncoding(1251));
      else
      {
        log.Warn($"File not found {calculationTextFile}");
        entity.CalculationText = string.Empty;
      }

      yield return entity;
    }

    #endregion
  }
}
