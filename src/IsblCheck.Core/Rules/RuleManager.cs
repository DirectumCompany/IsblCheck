using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Common.Logging;
using IsblCheck.Core.Exceptions;
using IsblCheck.Core.Properties;

namespace IsblCheck.Core.Rules
{
  /// <summary>
  /// Менеджер правил.
  /// </summary>
  internal class RuleManager : IRuleManager
  {
    #region Поля и свойства

    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<RuleManager>();

    /// <summary>
    /// Каталог со сборками правил.
    /// </summary>
    private readonly AggregateCatalog rulesCatalog;

    /// <summary>
    /// Контейнер сборок правил.
    /// </summary>
    private readonly CompositionContainer rulesContainer;

    /// <summary>
    /// Фабрики правил.
    /// </summary>
    [ImportMany(AllowRecomposition=true)]
    private IEnumerable<IRuleFactory> ruleFactories { get; set; }

    #endregion

    #region IRuleManager

    /// <summary>
    /// Событие изменения состава правил.
    /// </summary>
    public event EventHandler RuleCollectionChanged;

    /// <summary>
    /// Получить правила.
    /// </summary>
    /// <returns>Список правил.</returns>
    public IEnumerable<IRule> GetAllRules()
    {
      var result = new List<IRule>();

      foreach (var ruleFactory in this.ruleFactories)
      {
        var ruleInfos = ruleFactory.GetRuleInfos();
        var rules = ruleFactory.GetRules(ruleInfos);
        result.AddRange(rules);
      }

      return result;
    }

    /// <summary>
    /// Загрузить библиотеку с правилами.
    /// </summary>
    /// <param name="path">Путь к библиотеке.</param>
    public void LoadLibrary(string path)
    {
      if (!File.Exists(path))
        throw new FileNotFoundException();

      var fullPath = Path.GetFullPath(path);
      try
      {
        var assembly = Assembly.LoadFile(fullPath);
        this.LoadLibrary(assembly);
      }
      catch(Exception ex)
      {
        throw new AssemblyLoadingFailedException(path, ex);
      }
    }

    /// <summary>
    /// Загрузить библиотеку с правилами.
    /// </summary>
    /// <param name="assembly">Сборка.</param>
    public void LoadLibrary(Assembly assembly)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof(assembly));

      var assemblyCatalog = new AssemblyCatalog(assembly);
      this.rulesCatalog.Catalogs.Add(assemblyCatalog);

      log.Info(string.Format(Resources.INFO_RULES_ASSEMBLY_LOADED, assembly.Location));
    }

    /// <summary>
    /// Загрузить сборки с правилами из заданной директории.
    /// </summary>
    /// <param name="path"></param>
    public void LoadLibraries(string path)
    {
      if (!Directory.Exists(path))
        throw new DirectoryNotFoundException();

      var fullPath = Path.GetFullPath(path);
      var directoryCatalog = new DirectoryCatalog(fullPath);
      this.rulesCatalog.Catalogs.Add(directoryCatalog);

      log.Info(string.Format(Resources.INFO_RULES_FOLDER_LOADED, fullPath));
    }

    #endregion

    #region IDisposable Support

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          this.rulesContainer.Dispose();
          this.rulesCatalog.Dispose();
        }
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }

    #endregion

    #region Методы

    /// <summary>
    /// Сгенерировать событие изменения состава правил.
    /// </summary>
    /// <param name="args">Аргументы события.</param>
    protected virtual void OnRuleCollectionChanged(EventArgs args)
    {
      var handler = this.RuleCollectionChanged;
      handler?.Invoke(this, args);
    }

    /// <summary>
    /// Сгенерировать событие изменения состава правил.
    /// </summary>
    protected virtual void OnRuleCollectionChanged()
    {
      this.OnRuleCollectionChanged(EventArgs.Empty);
    }

    /// <summary>
    /// Обработчик события изменения экспортируемых объектов.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RulesContainerExportsChangedHandler(object sender, ExportsChangeEventArgs e)
    {
      this.OnRuleCollectionChanged();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal RuleManager()
    {
      this.rulesCatalog = new AggregateCatalog();
      this.rulesContainer = new CompositionContainer(this.rulesCatalog);
      this.rulesContainer.ComposeParts(this);
      this.rulesContainer.ExportsChanged += this.RulesContainerExportsChangedHandler;
    }

    #endregion
  }
}
