using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace IsblCheck.Core.Checker
{
  /// <summary>
  /// Интерфейс проверяемого элемента.
  /// </summary>
  public interface IDocument
  {
    /// <summary>
    /// Имя документа.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Исходный текст элемента.
    /// </summary>
    string Text { get; }

    /// <summary>
    /// Тип компоненты.
    /// </summary>
    ComponentType ComponentType { get; set; }

    /// <summary>
    /// Имя компоненты.
    /// </summary>
    string ComponentName { get; set; }

    /// <summary>
    /// Тип расположения.
    /// </summary>
    /// <remarks>
    /// Например Requisites["ContactT6"].Events["Change"]
    /// </remarks>
    string Path { get; set; }

    /// <summary>
    /// Получить синтаксическое дерево.
    /// </summary>
    /// <returns>Синтаксическое дерево.</returns>
    IParseTree GetSyntaxTree();

    /// <summary>
    /// Список контекстных переменных, например, аргументов функции, параметров методов и пр.
    /// </summary>
    List<string> ContextVariables { get; }
  }
}
