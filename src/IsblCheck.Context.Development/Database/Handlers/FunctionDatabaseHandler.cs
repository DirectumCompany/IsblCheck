using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик функций.
  /// </summary>
  internal class FunctionDatabaseHandler : IDatabaseHandler<Function>
  {
    #region IDatabaseHandler

    public IEnumerable<Function> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new Dictionary<string, Function>();
      var query = this.GetFunctionQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components.Values.ToList();

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var function = new Function
            {
              Name = reader["Name"] as string,
              Title = reader["Title"] as string,
              IsSystem = IsSystemValue.Equals(reader["IsSystem"] as string),
              Help = reader["Help"] as string,
              Comment = reader["Comment"] as string,
              CalculationText = reader["CalculationText"] as string
            };

            if (function.CalculationText == null)
              function.CalculationText = string.Empty;

            components.Add(function.Name, function);
          }
        }
      }

      query = this.GetFunctionArgumentQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var functionName = reader["FunctionName"] as string;
              if (!components.ContainsKey(functionName))
                continue;
              var function = components[functionName];

              var argument = new FunctionArgument
              {
                Number = (int) reader["Number"],
                Name = reader["Name"] as string
              };

              if (reader["Type"] is string typeValue &&
                TypeValues.ContainsKey(typeValue))
                argument.Type = TypeValues[typeValue];
              argument.DefaultValue = reader["DefaultValue"] as string;

              if (!string.IsNullOrEmpty(argument.DefaultValue))
                argument.HasDefaultValue = true;

              function.Arguments.Add(argument);
            }
          }
        }
      }

      return components.Values.ToList();
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос функций для версии 7.7 и выше.
    /// </summary>
    private const string FunctionQuery_7_7 = @"
      SELECT
        [FName] as [Name],
        [FName] as [Title],
        [SysFunc] as [IsSystem],
        [Txt] as [CalculationText],
        [Help] as [Help],
        [Comment] as [Comment]
      FROM
        [dbo].[MBFunc]
      ORDER BY
        [Name]";

    /// <summary>
    /// Запрос параметров функций для версии 7.7 и выше.
    /// </summary>
    private const string FunctionArgumentQuery_7_7 = @"
      SELECT
        [FName] as [FunctionName],
        [NumPar] as [Number],
        [Ident] as [Name],
        [Type] as [Type],
        [ValueDef] as [DefaultValue]
      FROM
        [dbo].[MBFuncRecv]
      ORDER BY
        [FunctionName],
        [Number]";

    /// <summary>
    /// Значение да.
    /// </summary>
    private const string IsSystemValue = "S";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Типы аргументов функций.
    /// </summary>
    private static readonly Dictionary<string, FunctionArgumentType> TypeValues
      = new Dictionary<string, FunctionArgumentType>
    {
      { "V", FunctionArgumentType.Variant },
      { "Д", FunctionArgumentType.Date },
      { "Ч", FunctionArgumentType.Float },
      { "L", FunctionArgumentType.Boolean },
      { "С", FunctionArgumentType.String },
      { "Ц", FunctionArgumentType.Integer }
    };

    #endregion

    #region Методы

    /// <summary>
    /// Запрос функций.
    /// </summary>
    public string GetFunctionQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return FunctionQuery_7_7;
      return null;
    }

    /// <summary>
    /// Запрос параметров функций.
    /// </summary>
    public string GetFunctionArgumentQuery(Version platformVersion)
    {
      var minVersion = new Version(7, 7);
      if (platformVersion > minVersion)
        return FunctionArgumentQuery_7_7;
      return null;
    }

    #endregion
  }
}
