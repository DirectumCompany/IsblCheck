using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using IsblCheck.Core.Parser;
using System;
using System.Collections.Generic;

namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Элемент проверки.
  /// </summary>
  public class Document : IDocument
  {
    #region Поля и свойства

    private IParseTree syntaxTree = null;

    #endregion

    #region IDocument

    public string Name { get; private set; }

    public string Text { get; private set; }

    public ComponentType ComponentType { get; set; }

    public string ComponentName { get; set; }

    public string Path { get; set; }

    public List<string> ContextVariables { get; private set; }

    public IParseTree GetSyntaxTree()
    {
      if (this.syntaxTree != null)
        return syntaxTree;

      var inputStream = new AntlrInputStream(this.Text);
      var lexer = new IsblLexer(inputStream);
      // TODO: Нужно как-то обрабатывать ошибки парсера.
      lexer.RemoveErrorListeners();
      var commonTokenStream = new CommonTokenStream(lexer);
      var parser = new IsblParser(commonTokenStream);
      // TODO: Нужно как-то обрабатывать ошибки парсера
      parser.RemoveErrorListeners();
      this.syntaxTree = parser.statementBlock();
      return this.syntaxTree;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Имя документа.</param>
    /// <param name="text">Исходный код.</param>
    public Document(string name, string text)
    {
      this.Name = name;
      this.Text = text ?? string.Empty;
      this.ContextVariables = new List<string>();
    }

    #endregion
  }
}
