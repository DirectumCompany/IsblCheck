using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using IsblCheck.Core.Parser;

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
    public bool IsEmpty => this.Context == null;

    /// <summary>
    /// Текст параметра.
    /// </summary>
    public string Text => this.Context == null ? string.Empty : this.Context.GetText();
  }

  public class ParameterListParser
  {    
    private readonly IsblParser.ParameterListContext parameterList;

    /// <summary>
    /// Список параметров.
    /// </summary>
    public List<ParameterValue> ParameterValues { get; }

    /// <summary>
    /// Распарсить список параметров.
    /// </summary>
    public void Parse()
    {
      var currentIndex = 0;
      var isEmpty = true;
      for (var i = 0; i < this.parameterList.ChildCount; i++)
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
        throw new ArgumentNullException(nameof(parameters));
      this.parameterList = parameters;
      this.ParameterValues = new List<ParameterValue>();
    }
  }
}
