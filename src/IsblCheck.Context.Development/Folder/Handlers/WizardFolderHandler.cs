using Common.Logging;
using IsblCheck.Context.Development.Folder.Models;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal class WizardFolderHandler : FolderHandlerBase<Wizard, RecordRefModel>
  {
    #region Константы

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    /// <summary>
    /// Состояние.
    /// </summary>
    private const string StateReqName = "Состояние";

    private const string StateActiveRequisiteValue = "Действующая";

    private static readonly char[] RussianSymbols =
    {
      'ё','й','ц','у', 'к', 'е', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ъ',
      'ф','ы','в','а','п','р','о','л','д','ж','э',
      'я','ч','с','м','и','т','ь','б','ю',
      '№','»','«','…','•','“','”'
    };

    #endregion

    #region Поля и свойства

    private static readonly ILog log = LogManager.GetLogger<WizardFolderHandler>();

    #endregion

    #region FolderHandlerBase

    protected override string FolderName { get { return "Wizards"; } }

    protected override string CardModelRootNode { get { return "Wizard"; } }

    protected override IEnumerable<Wizard> ReadComponents(RecordRefModel model, string componentFolderPath)
    {
      var wizard = new Wizard();
      wizard.Name = model.Code;
      wizard.Title = model.Name;
      var stateReq = model.Requisites
        .FirstOrDefault(r => r.Name == StateReqName);
      if (stateReq != null)
        wizard.State = stateReq.Value == StateActiveRequisiteValue ? ComponentState.Active : ComponentState.Closed;
      var structureFile = Path.Combine(componentFolderPath, "Structure.dfm");
      if (File.Exists(structureFile))
      {
        var wizardDfm = File.ReadAllText(structureFile, Encoding.GetEncoding(1251));
        if (!string.IsNullOrWhiteSpace(wizardDfm))
        {
          // Вернуть исходную кодировку
          wizardDfm = DeConvert(wizardDfm);
          var dfmWizard = WizardDfmParser.Parse(wizardDfm);
          if (dfmWizard != null)
          {
            wizard.Events.AddRange(dfmWizard.Events);
            wizard.Steps.AddRange(dfmWizard.Steps);
          }
        }
      }
      else
        log.Warn($"File not found {structureFile}");
      yield return wizard;
    }
    /// <summary>
    /// Преобразовать к исходной кодировке
    /// </summary>
    /// <param name="decodedValue"></param>
    /// <returns></returns>
    internal static string DeConvert(string decodedValue)
    {
      var data = decodedValue.Split(new[] { "\n\r", "\r\n", "\n", "\r" }, System.StringSplitOptions.None).ToList();
      data = ReplaceQuotes(data);
      data = ReplaceRussianSymbols(data);
      data = AddEnterSymbolsInText(data);
      data = data.Where(x => !string.IsNullOrEmpty(x)).Select(x => x + "\r\n").ToList();
      return string.Join("", data);
    }
    /// <summary>
    /// Добавить закодированные символы переноса строк в ISBL коде
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static List<string> AddEnterSymbolsInText(List<string> input)
    {
      var result = new List<string>(input.Count);
      for (var i = 0; i < input.Count - 1; ++i)
      {
        if (input[i].Contains("ValueList.Strings = ("))
        {
          i = SkipFunction(input, i, result);
        }

        if (input[i].Contains("#") || input[i].Contains("\'") || input[i].TrimStart(' ') == string.Empty)
        {
          var checkedLine = input[i + 1].TrimStart(' ');
          if (checkedLine.StartsWith("\'") || checkedLine.StartsWith("#") || checkedLine == string.Empty)
          {
            result.Add(input[i] + "#13#10");
            continue;
          }
        }

        if (input[i].TrimEnd(' ').EndsWith("=") && input[i + 1] == string.Empty)
        {
          result.Add(input[i] + "#13#10");
          continue;
        }
        result.Add(input[i]);
      }
      result.Add(input[input.Count - 1]);
      return result;
    }
    private static int SkipFunction(List<string> input, int i, List<string> result)
    {
      while (!input[i].Contains(")"))
      {
        result.Add(input[i]);
        ++i;
      }
      result.Add(input[i]);
      ++i;
      return i;
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
      if (symbols[index] != '\'') return false;

      if (index - 1 >= 0 && index + 1 < symbols.Length)
        return CheckNeighbor(symbols, index - 1) && CheckNeighbor(symbols, index + 1);

      if (index + 1 >= symbols.Length) return CheckNeighbor(symbols, index - 1);

      if (index - 1 < 0) return CheckNeighbor(symbols, index + 1);

      return false;
    }
    private static bool CheckNeighbor(char[] symbols, int index)
    {
      return (IsRussian(symbols[index]) || symbols[index] == '\'');
    }
    public static bool IsRussian(char symbol)
    {
      symbol = char.ToLower(symbol);
      return RussianSymbols.Any(x => x == symbol);
    }
    private static string GetSharpCode(char symbol)
    {
      var result = char.ConvertToUtf32(symbol.ToString(), 0);
      return "#" + result;
    }

    #endregion
  }
}
