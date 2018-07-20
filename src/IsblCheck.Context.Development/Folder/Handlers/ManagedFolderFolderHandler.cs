using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime.Misc;
using Common.Logging;
using IsblCheck.Context.Development.Folder.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal class ManagedFolderFolderHandler : FolderHandlerBase<ManagedFolder, RecordRefModel>
  {
    private const string IsSearchFolderRequisiteName = "ДаНет2";

    private static readonly ILog log = LogManager.GetLogger<ManagedFolderFolderHandler>();

    #region FolderHandlerBase

    protected override string FolderName => "ManagedFolders";

    protected override string CardModelRootNode => "ManagedFoldersRecord";

    protected override IEnumerable<ManagedFolder> ReadComponents(RecordRefModel model, string componentFolderPath)
    {
      var entity = new ManagedFolder { Name = model.Name };

      var title = model.Requisites
        .FirstOrDefault(r => r.Name == "ISBDescription");
      if (title != null)
        entity.Title = title.Value;

      var stateReq = model.Requisites
        .FirstOrDefault(r => r.Name == "Состояние");
      if (stateReq != null)
        entity.State = stateReq.Value == "Действующая" ? ComponentState.Active : ComponentState.Closed;

      ReadBeforeEventCalculation(model, entity, componentFolderPath);
      ReadMethods(model, entity, componentFolderPath);
      ReadActions(model, entity);

      yield return entity;
    }

    #endregion

    #region Методы

    private void ReadActions(RecordRefModel model, ManagedFolder entity)
    {
      foreach (var actionNameRequisite in model.Requisites.Where(r => r.Name == "СодержаниеТ2"))
      {
        var action = new ActionWithHandler
        {
          Name = actionNameRequisite.Value
        };

        var actionNumber = actionNameRequisite.Number;

        var actionMethodNameReq = model.Requisites
          .Where(r => r.Name == "ISBActionMethodName")
          .FirstOrDefault(r => r.Number == actionNumber);
        if (actionMethodNameReq != null)
        {
          action.ExecutionHandler = entity.Methods
            .FirstOrDefault(m => m.Name == actionMethodNameReq.Value);
        }

        entity.Actions.Add(action);
      }
    }

    private void ReadMethods(RecordRefModel model, ManagedFolder entity, string componentFolderPath)
    {
      foreach (var methodNameRequisite in model.Requisites.Where(r => r.Name == "ISBMethodName"))
      {
        var method = new Method
        {
          Name = methodNameRequisite.Value
        };
        var methodNumber = methodNameRequisite.Number;
        var calculationFilePath = Path.Combine(componentFolderPath, "Actions", $"ISBMethodCalculation_{methodNumber}.isbl");
        if (File.Exists(calculationFilePath))
        {
          method.CalculationText = File.ReadAllText(calculationFilePath, Encoding.GetEncoding(1251));
        }
        else
        {
          log.Warn($"File not found {calculationFilePath}");
          method.CalculationText = "";
        }

        var methodParamNumbers = model.Requisites
          .Where(r => r.Name == "ISBParamMethodName")
          .Where(r => r.Value == method.Name)
          .Select(r => r.Number);

        foreach (var methodParamNumber in methodParamNumbers)
        {
          var methodParam = new MethodParam();

          var methodParamNameReq = model.Requisites
            .Where(r => r.Name == "ISBParamName")
            .FirstOrDefault(r => r.Number == methodParamNumber);
          if (methodParamNameReq != null)
            methodParam.Name = methodParamNameReq.Value;

          var methodParamNumberReq = model.Requisites
            .Where(r => r.Name == "ISBParamNumber")
            .FirstOrDefault(r => r.Number == methodParamNumber);
          if (methodParamNumberReq != null)
            methodParam.Number = int.Parse(methodParamNumberReq.Value);

          var methodParamValueTypeReq = model.Requisites
            .Where(r => r.Name == "ISBParamValueType")
            .FirstOrDefault(r => r.Number == methodParamNumber);
          if (methodParamValueTypeReq != null)
          {
            switch (methodParamValueTypeReq.Value)
            {
              case "Variant":
                methodParam.Type = MethodParamType.Variant;
                break;
              case "Date":
                methodParam.Type = MethodParamType.Date;
                break;
              case "Float":
                methodParam.Type = MethodParamType.Float;
                break;
              case "Boolean":
                methodParam.Type = MethodParamType.Boolean;
                break;
              case "String":
                methodParam.Type = MethodParamType.String;
                break;
              case "Integer":
                methodParam.Type = MethodParamType.Integer;
                break;
              default:
                log.Warn($"Unknown type value \"{methodParamValueTypeReq.Value}\" for param \"{methodParam.Name}\" of managed folder \"{entity.Name}\"");
                break;
            }
          }

          var paramDefaultValueReq = model.Requisites
          .FirstOrDefault(r => r.Name == "ISBParamDefaultValue");
          if (paramDefaultValueReq != null)
            methodParam.DefaultValue = paramDefaultValueReq.Value;
          methodParam.HasDefaultValue = !string.IsNullOrEmpty(methodParam.DefaultValue);

          method.Params.Add(methodParam);
        }
        entity.Methods.Add(method);
      }
    }

    private void ReadBeforeEventCalculation(RecordRefModel model, ManagedFolder entity, string componentFolderPath)
    {
      var isSearchFolderReq = model.Requisites
        .FirstOrDefault(r => r.Name == IsSearchFolderRequisiteName);
      if (isSearchFolderReq == null)
      {
        log.Warn($"Requisite \"{IsSearchFolderRequisiteName}\" not found for managed folder \"{entity.Name}\"");
        return;
      }
      if (string.Equals(isSearchFolderReq.Value, "Да", StringComparison.OrdinalIgnoreCase))
      {
        var beforeEventFilePath = Path.Combine(componentFolderPath, "Event.isbl");
        if (File.Exists(beforeEventFilePath))
        {
          var beforeEventDecodedDfm = File.ReadAllText(beforeEventFilePath, Encoding.GetEncoding(1251));
          if (!string.IsNullOrWhiteSpace(beforeEventDecodedDfm))
          {
            var beforeSearchEventDfm = ManagedFoldersDFMConverter.DeConvert(beforeEventDecodedDfm);
            var beforeSearchEventText = AntlrUtils.Parse<string, DfmGrammarParser, StringListPropValueListener>(
              beforeSearchEventDfm, p => p.property());
            entity.SearchDescription = new SearchDescription
            {
              BeforeSearchEventText = beforeSearchEventText
            };
          }
        }
      }
    }

    #endregion

    #region Вложенные классы

    private class StringListPropValueListener : DfmGrammarBaseListener, IListenerWithResult<string>
    {
      private string stringListValue;

      public override void EnterProperty([NotNull] DfmGrammarParser.PropertyContext context)
      {
        this.stringListValue = DfmParseUtils.GetTextPropValue(context);
      }

      public string GetResult() => this.stringListValue;
    }

    /// <summary>
    /// Конвертер DFM управляемых папок. Код взят из утилиты DTU.
    /// </summary>
    private static class ManagedFoldersDFMConverter
    {
      private static readonly char[] RussianSymbols =
      {
        'ё','й','ц','у', 'к', 'е', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ъ',
        'ф','ы','в','а','п','р','о','л','д','ж','э',
        'я','ч','с','м','и','т','ь','б','ю',
        '№','»','«','…','•','“','”'
      };

      internal static string DeConvert(string decodedValue)
      {
        var data = decodedValue.Split(new[] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None).ToList();
        data = ReplaceQuotes(data);
        data = ReplaceRussianSymbols(data);
        data = data.Where(x => !string.IsNullOrEmpty(x)).Select(x => x + (string.Equals(x, data.Last()) ? "" : "\r\n")).ToList();
        return string.Join(string.Empty, data);
      }

      /// <summary>
      /// Закодировать кавычки в ISBL коде
      /// </summary>
      /// <param name="input"></param>
      /// <returns></returns>
      public static List<string> ReplaceQuotes(List<string> input)
      {
        var result = new List<string>();
        var builder = new StringBuilder();

        foreach (var line in input)
        {
          var symbols = line.ToCharArray();
          for (var i = 0; i < symbols.Length; ++i)
          {
            if (IsDecodedQuotes(symbols, i))
            {
              builder.Append(GetSharpCode(symbols[i]));
            }
            else
            {
              builder.Append(symbols[i]);
            }
          }
          result.Add(builder.ToString());
          builder.Clear();
        }
        return result;
      }

      private static bool IsDecodedQuotes(char[] symbols, int index)
      {
        if (symbols[index] != '\'')
          return false;

        if (index - 1 >= 0 && index + 1 < symbols.Length)
          return CheckNeighbor(symbols, index - 1) && CheckNeighbor(symbols, index + 1);

        if (index + 1 >= symbols.Length)
        {
          if (index > 0 && symbols[index - 1] == '\'')
            return symbols.Any(s => s != '\'' && s != ' ');

          return CheckNeighbor(symbols, index - 1);
        }

        if (index - 1 < 0)
          return CheckNeighbor(symbols, index + 1);

        return false;
      }

      private static bool CheckNeighbor(char[] symbols, int index)
      {
        return (IsRussian(symbols[index]) || symbols[index] == '\'');
      }

      /// <summary>
      /// Закодировать русские символы
      /// </summary>
      /// <param name="input"></param>
      /// <returns></returns>
      public static List<string> ReplaceRussianSymbols(List<string> input)
      {
        return input.Select(x => string.Join(string.Empty,
                                             x.Select(y =>
                                                      IsRussian(y)
                                                      ? GetSharpCode(y)
                                                      : y.ToString())
                                               .ToArray()))
                    .ToList();
      }

      private static string GetSharpCode(char symbol)
      {
        var result = char.ConvertToUtf32(symbol.ToString(), 0);
        return "#" + result;
      }

      public static bool IsRussian(char symbol)
      {
        symbol = char.ToLower(symbol);
        return RussianSymbols.Any(x => x == symbol);
      }
    } 

    #endregion
  }
}
