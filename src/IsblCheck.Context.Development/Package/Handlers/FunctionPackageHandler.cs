using System.Linq;
using System.Collections.Generic;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Обработчик функций.
  /// </summary>
  internal class FunctionPackageHandler : IPackageHandler<Function>
  {
    #region Константы

    /// <summary>
    /// Имя реквизита признака системной функции.
    /// </summary>
    private const string FunctionCategoryReqName = "ISBFuncCategory";

    /// <summary>
    /// Имя реквизита вычисления функции.
    /// </summary>
    private const string CalculationTextReqName = "ISBFuncText";

    /// <summary>
    /// Имя реквизита справки функции.
    /// </summary>
    private const string FunctionHelpTextReqName = "ISBFuncHelp";

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

    #endregion

    #region IPackageHandler

    public IEnumerable<Function> Read(ComponentsModel packageModel)
    {
      foreach (var model in packageModel.Functions)
      {
        var entity = PackageHandlerUtils.CreateEntity<Function>(model);

        var functionCategoryReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == FunctionCategoryReqName);
        if (functionCategoryReq != null)
          entity.IsSystem = functionCategoryReq.ValueLocalizeID == SystemCategory;

        var calculationTextReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == CalculationTextReqName);
        if (calculationTextReq != null)
          entity.CalculationText = calculationTextReq.DecodedText;

        var functionHelpTextReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == FunctionHelpTextReqName);
        if (functionHelpTextReq != null)
          entity.Help = functionHelpTextReq.DecodedText;

        var functionCommentReq = model.Card.Requisites
          .FirstOrDefault(r => r.Code == FunctionCommentReqName);
        if (functionCommentReq != null)
          entity.Comment = functionCommentReq.DecodedText;

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
              argument.Name = argumentNameReq.DecodedText;

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
              argument.DefaultValue = argumentDefaultValueReq.DecodedText;
            if (!string.IsNullOrEmpty(argument.DefaultValue))
              argument.HasDefaultValue = true;

            entity.Arguments.Add(argument);
          }
        }

        yield return entity;
      }
    }

    #endregion
  }
}
