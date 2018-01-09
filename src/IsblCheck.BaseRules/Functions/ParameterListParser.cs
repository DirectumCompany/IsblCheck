using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using IsblCheck.Core.Parser;
using System;
using System.Collections.Generic;

namespace IsblCheck.BaseRules.Functions
{
  /// <summary>
  /// Параметр.
  /// </summary>
  public class ParameterValue
  {
    /// <summary>
    /// Индекс параметра.
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// Контекст.
    /// </summary>
    public ParserRuleContext Context { get; set; }
    /// <summary>
    /// Признак того, что параметр пустой.
    /// </summary>
    public bool IsEmpty { get { return Context == null; } }
    /// <summary>
    /// Текст параметра.
    /// </summary>
    public string Text { get { return Context == null ? string.Empty : Context.GetText(); } }
  }

  public class ParameterListParser
  {    
    private IsblParser.ParameterListContext parameterList;

    /// <summary>
    /// Список параметров.
    /// </summary>
    public List<ParameterValue> ParameterValues { get; private set; }

    /// <summary>
    /// Распарсить список параметров.
    /// </summary>
    public void Parse()
    {
      int currentIndex = 0;
      bool isEmpty = true;
      for (int i = 0; i < this.parameterList.ChildCount; i++)
      {
        if (IsComma(this.parameterList.GetChild(i)))
        {
          if (isEmpty)
          {
            this.ParameterValues.Add(new ParameterValue { Index = currentIndex });
          }
          isEmpty = true;
          currentIndex++;
        }
        else
        {
          this.ParameterValues.Add(new ParameterValue
          {
            Index = currentIndex,
            Context = this.parameterList.GetChild(i) as ParserRuleContext
          });
          isEmpty = false;
        }
      }
      // Добавить последний параметр, если он пропущен.
      if (currentIndex > 0 && isEmpty)
      {
        this.ParameterValues.Add(new ParameterValue { Index = currentIndex });
      }
    }

    private static bool IsComma(IParseTree node)
    {
      var child = node as ITerminalNode;
      return child != null && child.Symbol.Type == IsblParser.COMMA;
    }

    public ParameterListParser(IsblParser.ParameterListContext parameters)
    {
      if (parameters == null)
        throw new ArgumentNullException("parameters");
      this.parameterList = parameters;
      this.ParameterValues = new List<ParameterValue>();
    }
  }
}
