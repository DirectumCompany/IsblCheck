using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Common.Logging;
using IsblCheck.Context.Development.Database.Handlers;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Database
{
  /// <summary>
  /// Провайдер базы данных.
  /// </summary>
  public class DatabaseProvider : IDevelopmentContextProvider
  {
    #region IDevelopmentContextProvider

    public IEnumerable<T> ReadComponents<T>() where T : Component
    {
      if(this.platformVersion == null)
        this.platformVersion = this.ReadPlatformVersion();

      var handlerType = ResolveComponentHandler<T>();
      if (handlerType == null)
        return Enumerable.Empty<T>();
      var handler = (IDatabaseHandler<T>)Activator.CreateInstance(handlerType);

      using (var connection = new SqlConnection(this.connectionString, this.credential))
      {
        try
        {
          connection.Open();
          return handler.Read(connection, platformVersion);
        }
        catch (SqlException ex)
        {
          log.Error("Database connection error.", ex);
          throw;
        }
      }
    }

    /// <summary>
    /// Сбросить кэш.
    /// </summary>
    public void ResetCache()
    {
    }

    #endregion

    #region Константы

    /// <summary>
    /// Запрос версии платформы.
    /// </summary>
    private const string PlatformVersionQuery = @"
      SELECT
        [ValuePar]
      FROM
        [dbo].[XIni]
      WHERE
        [NamePar] = 'PlatformVersion'";

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<DatabaseProvider>();

    /// <summary>
    /// Строка подключения.
    /// </summary>
    private readonly string connectionString;

    /// <summary>
    /// Учетные данные.
    /// </summary>
    private readonly SqlCredential credential;

    /// <summary>
    /// Версия платформы.
    /// </summary>
    private Version platformVersion;

    #endregion

    #region Методы

    /// <summary>
    /// Считать версию платформы.
    /// </summary>
    /// <returns>Версия платформы.</returns>
    private Version ReadPlatformVersion()
    {
      using (var connection = new SqlConnection(this.connectionString, this.credential))
      {
        try
        {
          connection.Open();

          var command = new SqlCommand(PlatformVersionQuery, connection);
          var platformVersionValue = command.ExecuteScalar() as string;
          if (!string.IsNullOrEmpty(platformVersionValue))
            return new Version(platformVersionValue);
          else
            throw new Exception("Wrong database version");
        }
        catch (SqlException ex)
        {
          log.Error("Database connection error.", ex);
          throw;
        }
      }
    }

    private static Type ResolveComponentHandler<T>() where T : Component
    {
      return Assembly.GetExecutingAssembly().DefinedTypes
        .FirstOrDefault(t => t.IsClass && t.ImplementedInterfaces.Contains(typeof(IDatabaseHandler<T>)));
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="connectionString">Строка подключения.</param>
    public DatabaseProvider(string connectionString)
    {
      this.connectionString = connectionString;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="connectionString">Строка подключения.</param>
    /// <param name="credential">Учетные данные аутентификации.</param>
    public DatabaseProvider(string connectionString, SqlCredential credential) : this(connectionString)
    {
      this.credential = credential;
    }

    #endregion
  }
}
