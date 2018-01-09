using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Обработчик функций.
  /// </summary>
  internal class FunctionFolderHandler : FolderHandlerBase<Function, ComponentModel>
  {
    #region Константы

    /// <summary>
    /// Имя реквизита признака системной функции.
    /// </summary>
    private const string FunctionCategoryReqName = "ISBFuncCategory";

    /// <summary>
    /// Имя реквизита комментария к функции.
    /// </summary>
    private const string FunctionCommentReqName = "ISBFuncComment";

    /// <summary>
    /// Имя реквизита номера аргумента.
    /// </summary>
    private const string ArgumentNumberReqName = "НомСтр";

    /// <summary>
    /// Имя реквизита имени аргумента.
    /// </summary>
    private const string ArgumentNameReqName = "ISBFuncParamIdent";

    /// <summary>
    /// Имя реквизита типа аргумента.
    /// </summary>
    private const string ArgumentTypeReqName = "ISBFuncParamType";

    /// <summary>
    /// Имя реквизита типа аргумента.
    /// </summary>
    private const string ArgumentDefaultValueReqName = "ISBFuncParamDefValue";

    /// <summary>
    /// Системная категория функции.
    /// </summary>
    private const string SystemCategory = "SYSRES_SYSCOMP.FUNCTION_CATEGORY_SYSTEM";

    /// <summary>
    /// Прикладная категория функции.
    /// </summary>
    private const string UserCategory = "SYSRES_SYSCOMP.FUNCTION_CATEGORY_USER";

    /// <summary>
    /// Вариантный.
    /// </summary>
    private const string VariantArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_VARIANT";

    /// <summary>
    /// Дата.
    /// </summary>
    private const string DateArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_DATE";

    /// <summary>
    /// Дробное число.
    /// </summary>
    private const string FloatArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_FLOAT";

    /// <summary>
    /// Логический.
    /// </summary>
    private const string BooleanArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_BOOLEAN";

    /// <summary>
    /// Целое число.
    /// </summary>
    private const string StringArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_STRING";

    /// <summary>
    /// Строка.
    /// </summary>
    private const string IntegerArgumentType = "SYSRES_SYSCOMP.DATA_TYPE_INTEGER";

    /// <summary>
    /// Имя файла кода функции.
    /// </summary>
    protected const string TextFileName = "Text.isbl";

    /// <summary>
    /// Имя файла справки функции.
    /// </summary>
    protected const string HelpFileName = "Help.xml";

    #endregion

    #region Поля и свойства

    private readonly ILog log = LogManager.GetLogger<FunctionFolderHandler>(); 

    #endregion

    #region FolderHandlerBase

    protected override string FolderName { get { return "Functions"; } }

    protected override string CardModelRootNode { get { return "Function"; } }

    protected override IEnumerable<Function> ReadComponents(ComponentModel model, string componentFolderPath)
    {
      var entity = PackageHandlerUtils.CreateEntity<Function>(model);

      var functionCategoryReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == FunctionCategoryReqName);
      if (functionCategoryReq != null)
        entity.IsSystem = functionCategoryReq.ValueLocalizeID == SystemCategory;

      var functionTextFile = Path.Combine(componentFolderPath, TextFileName);
      if (File.Exists(functionTextFile))
        entity.CalculationText = File.ReadAllText(functionTextFile, Encoding.GetEncoding(1251));
      else
        log.Warn($"File not found {functionTextFile}");

      var functionHelpTextFile = Path.Combine(componentFolderPath, HelpFileName);
      if (File.Exists(functionHelpTextFile))
        entity.Help = File.ReadAllText(functionHelpTextFile, Encoding.GetEncoding(1251));
      else
        log.Warn($"File not found {functionHelpTextFile}");

      var functionCommentReq = model.Card.Requisites
        .FirstOrDefault(r => r.Code == FunctionCommentReqName);
      if (functionCommentReq != null)
        entity.Comment = functionCommentReq.Value;

      if (model.DetailDataSets != null &&
        model.DetailDataSets.DetailDataSet1 != null)
      {
        var argModels = model.DetailDataSets.DetailDataSet1.Rows;
        foreach (var argModel in argModels)
        {
          var argument = new FunctionArgument();

          var argumentNumberReq = argModel.Requisites
            .FirstOrDefault(r => r.Code == ArgumentNumberReqName);
          if (argumentNumberReq != null)
            argument.Number = int.Parse(argumentNumberReq.Value);

          var argumentNameReq = argModel.Requisites
            .FirstOrDefault(r => r.Code == ArgumentNameReqName);
          if (argumentNameReq != null)
            argument.Name = argumentNameReq.Value;

          var argumentTypeReq = argModel.Requisites
            .FirstOrDefault(r => r.Code == ArgumentTypeReqName);
          if (argumentTypeReq != null)
            switch (argumentTypeReq.ValueLocalizeID)
            {
              case VariantArgumentType:
                argument.Type = FunctionArgumentType.Variant;
                break;
              case DateArgumentType:
                argument.Type = FunctionArgumentType.Date;
                break;
              case FloatArgumentType:
                argument.Type = FunctionArgumentType.Float;
                break;
              case BooleanArgumentType:
                argument.Type = FunctionArgumentType.Boolean;
                break;
              case StringArgumentType:
                argument.Type = FunctionArgumentType.String;
                break;
              case IntegerArgumentType:
                argument.Type = FunctionArgumentType.Integer;
                break;
            }

          var argumentDefaultValueReq = argModel.Requisites
            .FirstOrDefault(r => r.Code == ArgumentDefaultValueReqName);
          if (argumentDefaultValueReq != null)
            argument.DefaultValue = argumentDefaultValueReq.Value;
          if (!string.IsNullOrEmpty(argument.DefaultValue))
            argument.HasDefaultValue = true;

          entity.Arguments.Add(argument);
        }
      }

      yield return entity;
    } 

    #endregion
  }
}
