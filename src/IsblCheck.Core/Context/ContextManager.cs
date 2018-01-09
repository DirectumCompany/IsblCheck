using Common.Logging;
using IsblCheck.Core.Context.Application;
using IsblCheck.Core.Context.Development;
using IsblCheck.Core.Properties;
using System;

namespace IsblCheck.Core.Context
{
  /// <summary>
  /// Менеджер контекста.
  /// </summary>
  internal class ContextManager : IContextManager
  {
    #region Поля и свойства

    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<ContextManager>();

    /// <summary>
    /// Контекст.
    /// </summary>
    private readonly Context context = new Context();

    /// <summary>
    /// Фабрика контекста приложения.
    /// </summary>
    private IApplicationContextFactory applicationContextFactory;

    /// <summary>
    /// Фабрика контекста разработки.
    /// </summary>
    private IDevelopmentContextFactory developmentContextFactory;

    #endregion

    #region IContextManager

    /// <summary>
    /// Глобальный контекст.
    /// </summary>
    public IContext Context { get { return this.context; } }

    /// <summary>
    /// Событие изменения контекста.
    /// </summary>
    public event EventHandler ContextChanged;

    /// <summary>
    /// Загрузить контекст приложения.
    /// </summary>
    /// <param name="factory">Фабрика контекста приложения.</param>
    public void Load(IApplicationContextFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException("factory");

      this.applicationContextFactory = factory;
      this.context.Application = this.CreateApplicationContext();
      this.OnContextChanged();
    }

    /// <summary>
    /// Загрузить контекст разработки.
    /// </summary>
    /// <param name="factory">Фабрика контекста разработки.</param>
    public void Load(IDevelopmentContextFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException("factory");

      this.developmentContextFactory = factory;
      this.context.Development = this.CreateDevelopmentContext();
      this.OnContextChanged();
    }

    /// <summary>
    /// Перезагрузить глобальный контекст.
    /// </summary>
    public void Refresh()
    {
      this.context.Application = this.CreateApplicationContext();
      this.context.Development = this.CreateDevelopmentContext();
      this.OnContextChanged();
    }

    #endregion

    #region Методы

    /// <summary>
    /// Сгенерировать событие изменения контекста.
    /// </summary>
    /// <param name="args">Аргументы события.</param>
    protected virtual void OnContextChanged(EventArgs args)
    {
      EventHandler handler = this.ContextChanged;
      if (handler != null)
        handler.Invoke(this, args);
    }

    /// <summary>
    /// Сгенерировать событие изменения контекста.
    /// </summary>
    protected virtual void OnContextChanged()
    {
      this.OnContextChanged(EventArgs.Empty);
    }

    /// <summary>
    /// Загрузить контекст приложения.
    /// </summary>
    /// <returns>Контекст приложения.</returns>
    protected virtual IApplicationContext CreateApplicationContext()
    {
      if (this.applicationContextFactory == null)
        return null;

      try
      {
        var applicationContext = this.applicationContextFactory.Create();
        log.Info(Resources.INFO_LOAD_APP_CONTEXT);
        return applicationContext;
      }
      catch(Exception ex)
      {
        log.Error(Resources.ERROR_LOAD_APP_CONTEXT, ex);
        return null;
      }
    }

    /// <summary>
    /// Создать контекст разработки.
    /// </summary>
    /// <returns>Контекст разработки.</returns>
    protected virtual IDevelopmentContext CreateDevelopmentContext()
    {
      if (this.developmentContextFactory == null)
        return null;

      try
      {
        var developmentContext = this.developmentContextFactory.Create();
        log.Info(Resources.INFO_LOAD_DEV_CONTEXT);
        return developmentContext;
      }
      catch (Exception ex)
      {
        log.Error(Resources.ERROR_LOAD_DEV_CONTEXT, ex);
        return null;
      }
    }

    #endregion
  }
}
