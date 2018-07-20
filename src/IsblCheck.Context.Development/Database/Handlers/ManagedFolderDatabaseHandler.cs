using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using IsblCheck.Context.Development.Utils;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database.Handlers
{
  /// <summary>
  /// Обработчик управляемых папок.
  /// </summary>
  internal class ManagedFolderDatabaseHandler : IDatabaseHandler<ManagedFolder>
  {
    #region IDatabaseHandler

    public IEnumerable<ManagedFolder> Read(SqlConnection connection, Version platformVersion)
    {
      var components = new Dictionary<string, ManagedFolder>();

      var query = this.GetManagedFoldersQuery(platformVersion);
      if (string.IsNullOrEmpty(query))
        return components.Values.ToList();

      var command = new SqlCommand(query, connection);
      using (var reader = command.ExecuteReader())
      {
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var managedFolder = new ManagedFolder
            {
              Name = (reader["Name"] as string).Trim(),
              Title = (reader["Title"] as string).Trim()
            };
            if (ActiveValue.Equals(reader["State"] as string))
              managedFolder.State = ComponentState.Active;
            else
              managedFolder.State = ComponentState.Closed;

            if (reader["SearchDescription"] is byte[] searchDescriptionRawData)
            {
              var searchDescriptionDfm = Encoding.GetEncoding(1251).GetString(searchDescriptionRawData);
              if (!string.IsNullOrWhiteSpace(searchDescriptionDfm))
              {
                managedFolder.SearchDescription = SearchDescriptionParser.Parse(searchDescriptionDfm);
              }
            }

            components.Add(managedFolder.Name, managedFolder);
          }
        }
      }

      query = this.GetMethodsQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var managedFolderName = reader["ManagedFolderName"] as string;
              if (!components.TryGetValue(managedFolderName, out ManagedFolder managedFolder))
                continue;

              var method = new Method { Name = reader["Name"] as string };

              if (reader["CalculationText"] is byte[] calculationValue)
                method.CalculationText = Encoding.GetEncoding(1251).GetString(calculationValue);
              else
                method.CalculationText = string.Empty;

              managedFolder.Methods.Add(method);
            }
          }
        }
      }

      query = this.GetMethodParamsQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var managedFolderName = reader["ManagedFolderName"] as string;
              if (!components.TryGetValue(managedFolderName, out ManagedFolder managedFolder))
                continue;

              var methodName = reader["MethodName"] as string;
              var method = managedFolder.Methods
                .FirstOrDefault(m => m.Name == methodName);
              if (method == null)
                continue;

              var parameter = new MethodParam
              {
                Number = (int)reader["Number"],
                Name = reader["Name"] as string
              };

              var typeValue = reader["Type"] as string;
              if (TypeValues.TryGetValue(typeValue, out MethodParamType methodParamType))
              {
                parameter.Type = methodParamType;
              }
              parameter.DefaultValue = reader["DefaultValue"] as string;
              parameter.HasDefaultValue = !string.IsNullOrEmpty(parameter.DefaultValue);

              method.Params.Add(parameter);
            }
          }
        }
      }

      query = this.GetActionQuery(platformVersion);
      if (!string.IsNullOrEmpty(query))
      {
        command = new SqlCommand(query, connection);
        using (var reader = command.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var managedFolderName = reader["ManagedFolderName"] as string;
              if (!components.TryGetValue(managedFolderName, out ManagedFolder managedFolder))
                continue;

              var action = new ActionWithHandler { Name = reader["Name"] as string };

              var methodName = reader["MethodName"] as string;
              var executionHandler = managedFolder.Methods.FirstOrDefault(m => m.Name == methodName);
              if (executionHandler != null)
              {
                action.ExecutionHandler = executionHandler;
              }
              else
              {
                action.CalculationText = string.Empty;
              }

              managedFolder.Actions.Add(action);
            }
          }
        }
      }

      return components.Values.ToList();
    }

    #endregion

    #region Константы

    /// <summary>
    /// Минимально поддерживаемая версия платформы.
    /// </summary>
    private static readonly Version MinSupportedPlatformVersion = new Version(7, 56);

    /// <summary>
    /// Запрос управляемых папок для версии 7.56 и выше.
    /// </summary>
    private const string ManagedFoldersQuery_7_56 =
@"select
  ManagedFolder.NameAn as [Name],
  ManagedFolder.Description as [Title],
  ManagedFolder.Sost as [State],
  t.SearchCondition as [SearchDescription]
from 
  MBAnalit ManagedFolder
  join MBVidAn v
    on v.Vid = ManagedFolder.Vid
  left join MBText t
    on t.SrcRecID = ManagedFolder.XRecID
where
  v.Kod = 'ManagedFolders'";

    /// <summary>
    /// Запрос действий для версии 7.56 и выше.
    /// </summary>
    private const string ActionQuery_7_56 =
@"select
  ManagedFolder.NameAn as [ManagedFolderName],
  Actions.SoderT2 as [Name],
  Actions.ISBActionMethodName as [MethodName]
from 
  MBAnalit ManagedFolder
  join MBVidAn v
    on v.Vid = ManagedFolder.Vid
  join MBAnValR2 Actions
    on Actions.Analit = ManagedFolder.Analit
where
  v.Kod = 'ManagedFolders'";

    /// <summary>
    /// Запрос прикладных методов для версии 7.56 и выше.
    /// </summary>
    private const string MethodsQuery_7_56 =
@"select
  ManagedFolder.NameAn as [ManagedFolderName],
  Methods.ISBMethodName as [Name],
  t.ISBMethodCalculation as [CalculationText]
from 
  MBAnalit ManagedFolder
  join MBVidAn v
    on v.Vid = ManagedFolder.Vid
  join MBAnValR7 Methods
    on Methods.Analit = ManagedFolder.Analit
  join MBText t
    on t.SrcRecID = Methods.XRecID
  join XObj o
    on o.XRecID = t.SrcObjID
where
  v.Kod = 'ManagedFolders'
  and o.TblName = 'MBAnValR7'";

    /// <summary>
    /// Запрос параметров прикладных методов для версии 7.56 и выше.
    /// </summary>
    private const string MethodParamsQuery_7_56 =
@"select
  ManagedFolder.NameAn as [ManagedFolderName],
  MethodParams.ISBParamMethodName as [MethodName],
  MethodParams.ISBParamName as [Name],
  MethodParams.ISBParamNumber as [Number],
  MethodParams.ISBParamValueType as [Type],
  MethodParams.ISBParamDefaultValue as [DefaultValue]
from 
  MBAnalit ManagedFolder
  join MBVidAn v
    on v.Vid = ManagedFolder.Vid
  join MBAnValR8 MethodParams
    on MethodParams.Analit = ManagedFolder.Analit
where
  v.Kod = 'ManagedFolders'";

    /// <summary>
    /// Значение Активный.
    /// </summary>
    private const string ActiveValue = "Д";

    /// <summary>
    /// Типы аргументов методов.
    /// </summary>
    private static readonly Dictionary<string, MethodParamType> TypeValues
      = new Dictionary<string, MethodParamType>
    {
      { "V", MethodParamType.Variant },
      { "D", MethodParamType.Date },
      { "F", MethodParamType.Float },
      { "L", MethodParamType.Boolean },
      { "S", MethodParamType.String },
      { "I", MethodParamType.Integer }
    };

    #endregion

    #region Методы

    /// <summary>
    /// Запрос управляемых папок.
    /// </summary>
    public string GetManagedFoldersQuery(Version platformVersion)
    {
      var minVersion = MinSupportedPlatformVersion;
      if (platformVersion > minVersion)
        return ManagedFoldersQuery_7_56;
      return null;
    }

    /// <summary>
    /// Запрос действий.
    /// </summary>
    public string GetActionQuery(Version platformVersion)
    {
      var minVersion = MinSupportedPlatformVersion;
      if (platformVersion > minVersion)
        return ActionQuery_7_56;
      return null;
    }

    /// <summary>
    /// Запрос прикладных методов.
    /// </summary>
    public string GetMethodsQuery(Version platformVersion)
    {
      var minVersion = MinSupportedPlatformVersion;
      if (platformVersion > minVersion)
        return MethodsQuery_7_56;
      return null;
    }

    /// <summary>
    /// Запрос параметров прикладных методов.
    /// </summary>
    public string GetMethodParamsQuery(Version platformVersion)
    {
      var minVersion = MinSupportedPlatformVersion;
      if (platformVersion > minVersion)
        return MethodParamsQuery_7_56;
      return null;
    }

    #endregion
  }
}
