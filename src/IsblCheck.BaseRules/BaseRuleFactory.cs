using System.ComponentModel.Composition;
using IsblCheck.BaseRules.Functions;
using IsblCheck.BaseRules.LogicalExpressions;
using IsblCheck.BaseRules.ObjectModel;
using IsblCheck.BaseRules.Other;
using IsblCheck.BaseRules.Variables;
using IsblCheck.Core.Rules;

namespace IsblCheck.BaseRules
{
  /// <summary>
  /// Фабрика базовых правил
  /// </summary>
  [Export(typeof(IRuleFactory))]
  public class BaseRuleFactory : AbstractRuleFactory
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    public BaseRuleFactory()
    {
      #region Functions

      this.Register<IncorrectFormatStringRule>(IncorrectFormatStringRule.Info);
      this.Register<SingleFormatArgumentRule>(SingleFormatArgumentRule.Info);
      this.Register<IncorrectFunctionParamsCountRule>(IncorrectFunctionParamsCountRule.Info);
      this.Register<FunctionWithoutHelpRule>(FunctionWithoutHelpRule.Info);
      this.Register<UsingNonExistingLocalizationStringRule>(UsingNonExistingLocalizationStringRule.Info);
      this.Register<ExceptionClassNotSpecifiedRule>(ExceptionClassNotSpecifiedRule.Info);
      this.Register<FunctionTooBigRule>(FunctionTooBigRule.Info);
      this.Register<UsingNotExistedReferenceRule>(UsingNotExistedReferenceRule.Info);
      this.Register<InteractiveWindowsOnEvents>(InteractiveWindowsOnEvents.Info);

      #endregion

      #region LogicalExpressions

      this.Register<UsingTrueFalseKeywordsRule>(UsingTrueFalseKeywordsRule.Info);

      #endregion

      #region ObjectModel

      this.Register<RecoveryObjectStateRule>(RecoveryObjectStateRule.Info);
      this.Register<UsingInfoReferenceRule>(UsingInfoReferenceRule.Info);

      #endregion

      #region Variables

      this.Register<UsingNotAssignedVarRule>(UsingNotAssignedVarRule.Info);
      this.Register<UsingRedefinedVarRule>(UsingRedefinedVarRule.Info);
      this.Register<NotUsedVarRule>(NotUsedVarRule.Info);
      this.Register<SelfAssignmentVarRule>(SelfAssignmentVarRule.Info);

      #endregion

      #region Other

      this.Register<TodoDoneCommentsRule>(TodoDoneCommentsRule.Info);

      #endregion
    }
  }
}
