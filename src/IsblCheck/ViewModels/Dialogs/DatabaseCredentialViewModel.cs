using Common.Logging;
using FluentValidation;
using GalaSoft.MvvmLight.Command;
using IsblCheck.Common.Localization;
using IsblCheck.Properties;
using IsblCheck.Services;
using IsblCheck.ViewModels.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace IsblCheck.ViewModels.Dialogs
{
  /// <summary>
  /// Презентер диалога открытия базы данных.
  /// </summary>
  public class DatabaseCredentialViewModel : DialogViewModelBase
  {
    #region Поля и свойства

    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<DatabaseCredentialViewModel>();

    /// <summary>
    /// Сервис представлений.
    /// </summary>
    private readonly IViewService viewService;

    /// <summary>
    /// Валидатор.
    /// </summary>
    private readonly IValidator<DatabaseCredentialViewModel> validator = new DatabaseCredentialValidator();

    /// <summary>
    /// Валидатор для проверки подключения к серверу.
    /// </summary>
    private readonly IValidator<DatabaseCredentialViewModel> serverConnectionValidator = new DatabaseCredentialValidator(needCheckDatabase: false);

    /// <summary>
    /// Источники баз данных.
    /// </summary>
    private readonly ObservableCollection<string> databaseSources = new ObservableCollection<string>();

    /// <summary>
    /// Базы данных.
    /// </summary>
    private readonly ObservableCollection<string> databases = new ObservableCollection<string>();

    /// <summary>
    /// Признак заполнения источников баз данных.
    /// </summary>
    private bool isDatabaseSourcesFetched = false;

    /// <summary>
    /// Признак заполнения базы данных.
    /// </summary>
    private bool isDatabasesFetched = false;

    /// <summary>
    /// Представление источников баз данных.
    /// </summary>
    public ICollectionView DatabaseSourcesView { get; private set; }

    /// <summary>
    /// Представление баз данных.
    /// </summary>
    public ICollectionView DatabasesView { get; private set; }

    /// <summary>
    /// Источник баз данных.
    /// </summary>
    public string DatabaseSource
    {
      get { return this.databaseSource; }
      set
      {
        if (this.databaseSource == value)
          return;
        this.databaseSource = value;
        this.RaisePropertyChanged();
        this.ValidateProperty();
      }
    }
    private string databaseSource;

    /// <summary>
    /// Интегрированная проверка учетных данных.
    /// </summary>
    public bool IntegratedSecurity
    {
      get { return this.integratedSecurity; }
      set
      {
        if (this.integratedSecurity == value)
          return;
        this.integratedSecurity = value;
        this.RaisePropertyChanged();
        this.ValidateProperty();
      }
    }
    private bool integratedSecurity;

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string UserName
    {
      get { return this.userName; }
      set
      {
        if (this.userName == value)
          return;
        this.userName = value;
        this.RaisePropertyChanged();
        this.ValidateProperty();
      }
    }
    private string userName;

    /// <summary>
    /// Пароль.
    /// </summary>
    public SecureString Password
    {
      get { return this.password; }
      set
      {
        if (this.password == value)
          return;
        this.password = value;
        this.RaisePropertyChanged();
        this.ValidateProperty();
      }
    }
    private SecureString password = new SecureString();

    /// <summary>
    /// База данных.
    /// </summary>
    public string Database
    {
      get { return this.database; }
      set
      {
        if (this.database == value)
          return;
        this.database = value;
        this.RaisePropertyChanged();
        this.ValidateProperty();
      }
    }
    private string database;

    #endregion

    #region Команды

    /// <summary>
    /// Команда обновления источников баз данных.
    /// </summary>
    public ICommand FetchDatabaseSourcesCommand { get; private set; }

    /// <summary>
    /// Команда обновления источников баз данных.
    /// </summary>
    public ICommand RefreshDatabaseSourcesCommand { get; private set; }

    /// <summary>
    /// Команда обновления баз данных.
    /// </summary>
    public ICommand FetchDatabasesCommand { get; private set; }

    /// <summary>
    /// Команда обновления баз данных.
    /// </summary>
    public ICommand RefreshDatabasesCommand { get; private set; }

    /// <summary>
    /// Команда проверки подключения.
    /// </summary>
    public ICommand TestConnectionCommand { get; private set; }

    #endregion

    #region Методы

    protected override void Ok()
    {
      base.Ok();
      SaveSettings();
    }

    /// <summary>
    /// Сохранить последние данные о БД.
    /// </summary>
    private void SaveSettings()
    {
      Settings.Default.LastServer = this.databaseSource;
      Settings.Default.LastIntegratedSecurity = this.integratedSecurity;
      Settings.Default.LastUser = this.userName;
      Settings.Default.LastDatabase = this.database;
      Settings.Default.Save();
    }

    /// <summary>
    /// Загрузить последние данные о БД.
    /// </summary>
    private void LoadSettings()
    {
      this.databaseSource = Settings.Default.LastServer;
      this.integratedSecurity = Settings.Default.LastIntegratedSecurity;
      this.userName = Settings.Default.LastUser;
      this.database = Settings.Default.LastDatabase;
    }

    /// <summary>
    /// Получить строку подключения.
    /// </summary>
    /// <param name="includeDataSource">Признак того, что в строку подключения нужно включать базу данных.</param>
    /// <returns>Строка подключения</returns>
    public string GetConnectionString(bool includeDataSource = true)
    {
      var connectionStringBuilder = new SqlConnectionStringBuilder();
      if (!string.IsNullOrEmpty(this.DatabaseSource))
        connectionStringBuilder.DataSource = this.DatabaseSource;
      if (includeDataSource && !string.IsNullOrEmpty(this.Database))
        connectionStringBuilder.InitialCatalog = this.Database;
      connectionStringBuilder.IntegratedSecurity = this.IntegratedSecurity;

      return connectionStringBuilder.ConnectionString;
    }

    /// <summary>
    /// Получить учетные данные.
    /// </summary>
    /// <returns>Учетные данные.</returns>
    public SqlCredential GetSqlCredential()
    {
      if (this.IntegratedSecurity)
        return null;

      if (string.IsNullOrEmpty(this.UserName))
        return null;

      var currentPassword = this.Password.Copy();
      currentPassword.MakeReadOnly();
      return new SqlCredential(this.UserName, currentPassword);
    }

    /// <summary>
    /// Проверить форму.
    /// </summary>
    protected override void ValidateForm()
    {
      this.ValidateForm(this.validator);
    }

    /// <summary>
    /// Проверить поле.
    /// </summary>
    /// <param name="propertyName">Имя поля.</param>
    protected override void ValidateProperty([CallerMemberName] string propertyName = null)
    {
      // Очищаем список результатов по полю и уведомляем об этом.
      if (this.Errors.ContainsKey(propertyName))
      {
        this.Errors.Remove(propertyName);
        this.OnErrorChanged(propertyName);
      }

      var validationResult = this.validator.Validate(this, propertyName);
      if (!validationResult.IsValid)
      {
        var errorList = validationResult.Errors
          .Select(f => f.ErrorMessage)
          .ToList();
        this.Errors.Add(propertyName, errorList);
        this.OnErrorChanged(propertyName);
      }
    }

    /// <summary>
    /// Проверить форму указанным валидатором.
    /// </summary>
    /// <param name="customValidator">Валидатор.</param>
    private void ValidateForm(IValidator customValidator)
    {
      // Очищаем список результатов и уведомляем об этом.
      // Необходимо сперва вызвать событие изменения свойства.
      // Иначе ошибки не подхватятся.
      var propertyNames = this.Errors.Keys.ToList();
      this.Errors.Clear();
      foreach (var propertyName in propertyNames)
      {
        this.RaisePropertyChanged(propertyName);
        this.OnErrorChanged(propertyName);
      }

      var validationResult = customValidator.Validate(this);
      if (!validationResult.IsValid)
      {
        foreach (var error in validationResult.Errors)
        {
          if (!this.Errors.ContainsKey(error.PropertyName))
            this.Errors.Add(error.PropertyName, new List<string>());
          var errorList = this.Errors[error.PropertyName];
          errorList.Add(error.ErrorMessage);
        }

        // Уведомляем о наличии ошибок.
        // Необходимо сперва вызвать событие изменения свойства.
        // Иначе ошибки не подхватятся.
        foreach (var propertyName in this.Errors.Keys)
        {
          this.RaisePropertyChanged(propertyName);
          this.OnErrorChanged(propertyName);
        }
      }
    }

    /// <summary>
    /// Заполнить источники баз данных.
    /// </summary>
    private void FillDatabaseSources()
    {
      this.databaseSources.Clear();

      using (DataTable sqlSources = SqlDataSourceEnumerator.Instance.GetDataSources())
      {
        foreach (DataRow source in sqlSources.Rows)
        {
          var databaseSource = source["ServerName"].ToString();
          var instanceName = source["InstanceName"].ToString();
          if (!string.IsNullOrEmpty(instanceName))
            databaseSource += '\\' + instanceName;
          this.databaseSources.Add(databaseSource);
        }
      }

      this.isDatabaseSourcesFetched = true;
    }

    /// <summary>
    /// Заполнить базы данных.
    /// </summary>
    private void FillDatabases()
    {
      this.ValidateForm(this.serverConnectionValidator);
      if (this.HasErrors)
        return;

      var connectionString = this.GetConnectionString(includeDataSource: false);
      var credential = this.GetSqlCredential();

      if (string.IsNullOrEmpty(connectionString) || credential == null)
        return;

      this.databases.Clear();

      using (var sqlConnecion = new SqlConnection(connectionString, credential))
      {
        try
        {
          sqlConnecion.Open();
          var databasesTable = sqlConnecion.GetSchema("Databases");
          foreach (DataRow row in databasesTable.Rows)
          {
            var databaseName = row["database_name"].ToString();
            this.databases.Add(databaseName);
          }
          this.isDatabasesFetched = true;
        }
        catch (Exception ex)
        {
          var message = LocalizationManager.Instance.LocalizeString("WRONG_DATABASE_CREDENTIALS");
          log.Error(message, ex);
          this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Error);
        }
      }
    }

    /// <summary>
    /// Проверить подключение.
    /// </summary>
    private void TestConnection()
    {
      this.ValidateForm();
      if (this.HasErrors)
        return;

      var connectionString = this.GetConnectionString();
      var credential = this.GetSqlCredential();

      if (string.IsNullOrEmpty(connectionString) || credential == null)
        return;

      using (var sqlConnecion = new SqlConnection(connectionString, credential))
      {
        try
        {
          sqlConnecion.Open();

          var message = LocalizationManager.Instance.LocalizeString("CONNECTION_SUCCESS");
          this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
          var message = LocalizationManager.Instance.LocalizeString("WRONG_DATABASE_CREDENTIALS");
          log.Error(message, ex);
          this.viewService.ShowMessageBox(message, icon: MessageBoxImage.Error);
        }
      }
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="viewService">Сервис представлений.</param>
    public DatabaseCredentialViewModel(IViewService viewService)
    {
      this.viewService = viewService;

      this.DatabaseSourcesView = CollectionViewSource.GetDefaultView(this.databaseSources);
      this.DatabaseSourcesView.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
      this.DatabasesView = CollectionViewSource.GetDefaultView(this.databases);
      this.DatabasesView.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));

      this.FetchDatabaseSourcesCommand = new RelayCommand(this.FillDatabaseSources, () => !this.isDatabaseSourcesFetched);
      this.RefreshDatabaseSourcesCommand = new RelayCommand(this.FillDatabaseSources);
      this.FetchDatabasesCommand = new RelayCommand(this.FillDatabases, () => !this.isDatabasesFetched);
      this.RefreshDatabasesCommand = new RelayCommand(this.FillDatabases);
      this.TestConnectionCommand = new RelayCommand(this.TestConnection);
      this.LoadSettings();
    }

    #endregion
  }
}
