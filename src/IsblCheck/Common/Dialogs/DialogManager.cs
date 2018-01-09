using Autofac;
using IsblCheck.Common.Patterns;
using System.Linq;

namespace IsblCheck.Common.Dialogs
{
  /// <summary>
  /// Менеджер панелей.
  /// </summary>
  public class DialogManager : Singleton<DialogManager>
  {
    #region Поля и свойства

    private ContainerBuilder builder;

    /// <summary>
    /// Контейнер.
    /// </summary>
    private IContainer container;

    #endregion

    #region Методы

    public DialogManager BeginConfig()
    {
      this.builder = new ContainerBuilder();
      return this;
    }

    public DialogManager EndConfig()
    {
      this.container = builder.Build();
      return this;
    }

    /// <summary>
    /// Зарегистрировать диалог.
    /// </summary>
    /// <typeparam name="TDialog">Тип диалога.</typeparam>
    public DialogManager RegisterDialog<TDialog>() where TDialog : IDialog
    {
      this.builder.RegisterType<TDialog>();
      return this;
    }

    /// <summary>
    /// Зарегистрировать сервис.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TInterface">Тип интерфейса сервиса.</typeparam>
    public DialogManager RegisterService<TService, TInterface>()
    {
      this.builder.RegisterType<TService>()
        .As<TInterface>()
        .SingleInstance();
      return this;
    }

    /// <summary>
    /// Создать диалог.
    /// </summary>
    /// <typeparam name="TDialog">Тип панели.</typeparam>
    /// <returns>Панель.</returns>
    public TDialog CreateDialog<TDialog>() where TDialog : IDialog
    {
      return this.container.Resolve<TDialog>();
    }

    /// <summary>
    /// Создать диалог.
    /// </summary>
    /// <typeparam name="TDialog">Тип диалога.</typeparam>
    /// <param name="args">Аргументы.</param>
    /// <returns>Панель.</returns>
    public TDialog CreateDialog<TDialog>(params object[] args) where TDialog : IDialog
    {
      var parameters = args.Select((value, index) => new PositionalParameter(index, value));
      return this.container.Resolve<TDialog>(parameters);
    }

    #endregion
  }
}
