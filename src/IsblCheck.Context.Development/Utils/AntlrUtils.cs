using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;

namespace IsblCheck.Context.Development.Utils
{
  /// <summary>
  /// Слушатель для обхода синтаксического дерева, который после обхода дерева 
  /// возвращает результат указанного типа.
  /// </summary>
  /// <typeparam name="TResult">Тип результата.</typeparam>
  internal interface IListenerWithResult<TResult> : IParseTreeListener
  {
    TResult GetResult();
  }

  /// <summary>
  /// Утилиты для работы с грамматиками ANTLR.
  /// </summary>
  internal class AntlrUtils
  {
    /// <summary>
    /// Разобрать текст и получить результат.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    /// <typeparam name="TParser">Класс парсера грамматики.</typeparam>
    /// <typeparam name="TListener">Класс слушателя, который будет накапливать результат.</typeparam>
    /// <param name="text">Текст для разбора.</param>
    /// <param name="getParseTree">Функция для получения поддерева для анализа.</param>
    /// <returns>Результат разбора текста.</returns>
    public static TResult Parse<TResult, TParser, TListener>(
      string text, Func<TParser, IParseTree> getParseTree)
      where TListener : IListenerWithResult<TResult> where TParser : Parser
    {
      var inputStream = new AntlrInputStream(text);
      var lexer = new DfmGrammarLexer(inputStream);
      // TODO: Нужно как-то обрабатывать ошибки парсера.
      lexer.RemoveErrorListeners();
      var commonTokenStream = new CommonTokenStream(lexer);
      var parser = (TParser)Activator.CreateInstance(typeof(TParser), commonTokenStream);
      // TODO: Нужно как-то обрабатывать ошибки парсера.
      parser.RemoveErrorListeners();
      var tree = getParseTree(parser);
      var listener = (TListener)Activator.CreateInstance(typeof(TListener));
      var walker = new ParseTreeWalker();
      walker.Walk(listener, tree);
      return listener.GetResult();
    }
  }
}
