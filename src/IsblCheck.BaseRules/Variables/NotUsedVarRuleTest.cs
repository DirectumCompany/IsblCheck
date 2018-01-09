using IsblCheck.BaseRules.Tests;
using IsblCheck.Core.Checker;
using IsblCheck.Core.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IsblCheck.BaseRules.Variables.Tests
{
  /// <summary>
  /// Тесты для NotUsedVarRule.
  /// </summary>
  [TestClass]
  public class NotUsedVarRuleTest
  {
    /// <summary>
    /// Тест с ошибкой.
    /// </summary>
    [TestMethod]
    public void NotUsedVarRuleBadTest()
    {
      //arrange
      var reportManager = new ReportManager();
      var report = reportManager.Create();
      var document = new Document("test", "abs(4){abs=3}");//"a = 3 a : int two = 2 b = 3 abs(b)");

      //act
      var rule = new NotUsedVarRule();
      rule.Apply(report, document, ContextHelper.Context);
      var messageList = report.Messages
        .OrderBy(d => d.Position.StartIndex)
        .ToList();
      
      ////assert
      Assert.AreEqual(0, report.Messages.Count());
      
      //Assert.AreEqual(2, report.Messages.Count());

      //var firstMessage = messageList[0];
      //Assert.AreEqual("A003", firstMessage.Code);
      //Assert.AreEqual(Severity.Warning, firstMessage.Severity);
      //Assert.AreEqual(6, firstMessage.Position.StartIndex);
      //Assert.AreEqual(1, firstMessage.Position.Length);

      //var secMessage = messageList[1];
      //Assert.AreEqual("A003", secMessage.Code);
      //Assert.AreEqual(Severity.Warning, secMessage.Severity);
      //Assert.AreEqual(14, secMessage.Position.StartIndex);
      //Assert.AreEqual(3, secMessage.Position.Length);
    }

    /// <summary>
    /// Хороший тест.
    /// </summary>
    [TestMethod]
    public void NotUsedVarRuleGoodTest()
    {
      //arrange
      var reportManager = new ReportManager();
      var report = reportManager.Create();
      var document = new Document("test", "a = 3 b = 2 abs(a) abs(b) a : int b : ref");
      
      //act
      var rule = new NotUsedVarRule();
      rule.Apply(report, document, ContextHelper.Context);

      //assert
      Assert.AreEqual(0, report.Messages.Count());
     }
  }
}
