using Autofac;
using IsblCheck.Common.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsblCheck.Common.Panels
{
  /// <summary>
  /// Менеджер панелей.
  /// </summary>
  public class PanelManager : Singleton<PanelManager>
  {
    #region Поля и свойства

    private ContainerBuilder builder;

    /// <summary>
    /// Контейнер.
    /// </summary>
    private IContainer container;

    /// <summary>
    /// Контейнер панелей.
    /// </summary>
    public IPanelContainer PanelContainer
    {
      get
      {
        return this.container.Resolve<IPanelContainer>();
      }
    }

    #endregion

    #region Методы

    public PanelManager BeginConfig()
    {
      this.builder = new ContainerBuilder();
      return this;
    }

    public PanelManager EndConfig()
    {
      this.container = builder.Build();
      return this;
    }

    /// <summary>
    /// Зарегистрировать панель.
    /// </summary>
    /// <typeparam name="TPanel">Тип панели.</typeparam>
    public PanelManager RegisterPanel<TPanel>() where TPanel : IPanel
    {
      this.builder.RegisterType<TPanel>();
      return this;
    }

    /// <summary>
    /// Зарегистрировать панель с указанным именем.
    /// </summary>
    /// <typeparam name="TPanel">Тип панели.</typeparam>
    /// <param name="name">Имя панели.</param>
    public PanelManager RegisterNamedPanel<TPanel>(string name) where TPanel : IPanel
    {
      this.builder.RegisterType<TPanel>()
        .As<TPanel>()
        .Named<IPanel>(name)
        .SingleInstance();
      return this;
    }

    /// <summary>
    /// Зарегистрировать контейнер панелей.
    /// </summary>
    /// <typeparam name="TPanelContainer">Тип контейнера панелей.</typeparam>
    public PanelManager RegisterContainer<TPanelContainer>() where TPanelContainer : IPanelContainer
    {
      this.builder.RegisterType<TPanelContainer>()
        .As<IPanelContainer>()
        .SingleInstance();
      return this;
    }

    /// <summary>
    /// Зарегистрировать сервис.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TInterface">Тип интерфейса сервиса.</typeparam>
    public PanelManager RegisterService<TService, TInterface>()
    {
      this.builder.RegisterType<TService>()
        .As<TInterface>()
        .SingleInstance();
      return this;
    }

    /// <summary>
    /// Создать именованную панель.
    /// </summary>
    /// <param name="name">Имя панели.</param>
    /// <param name="isAnchorable">Закрепляемая.</param>
    /// <returns>Панель.</returns>
    public IPanel CreateNamedPanel(string name, bool isAnchorable = false)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      var panel = this.container.ResolveNamed<IPanel>(name);
      panel.ContentId = name;
      if (isAnchorable)
        this.PanelContainer.AnchorablePanels.Add(panel);
      else
        this.PanelContainer.Panels.Add(panel);
      return panel;
    }

    /// <summary>
    /// Создать именованную панель.
    /// </summary>
    /// <param name="name">Имя панели.</param>
    /// <param name="isAnchorable">Закрепляемая.</param>
    /// <param name="args">Аргументы.</param>
    /// <returns>Панель.</returns>
    public IPanel CreateNamedPanel(string name, bool isAnchorable = false, params object[] args)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      var parameters = args.Select((value, index) => new PositionalParameter(index, value));
      var panel = this.container.ResolveNamed<IPanel>(name);
      panel.ContentId = name;
      if (isAnchorable)
        this.PanelContainer.AnchorablePanels.Add(panel);
      else
        this.PanelContainer.Panels.Add(panel);
      return panel;
    }

    /// <summary>
    /// Создать панель.
    /// </summary>
    /// <typeparam name="TPanel">Тип панели.</typeparam>
    /// <param name="isAnchorable">Закрепляемая.</param>
    /// <returns>Панель.</returns>
    public TPanel CreatePanel<TPanel>(bool isAnchorable = false) where TPanel : IPanel
    {
      var panel = this.container.Resolve<TPanel>();
      if (isAnchorable)
        this.PanelContainer.AnchorablePanels.Add(panel);
      else
        this.PanelContainer.Panels.Add(panel);
      return panel;
    }

    /// <summary>
    /// Создать панель.
    /// </summary>
    /// <typeparam name="TPanel">Тип панели.</typeparam>
    /// <param name="isAnchorable">Закрепляемая.</param>
    /// <param name="args">Аргументы.</param>
    /// <returns>Панель.</returns>
    public TPanel CreatePanel<TPanel>(bool isAnchorable = false, params object[] args) where TPanel : IPanel
    {
      var parameters = args.Select((value, index) => new PositionalParameter(index, value));
      var panel = this.container.Resolve<TPanel>(parameters);
      if (isAnchorable)
        this.PanelContainer.AnchorablePanels.Add(panel);
      else
        this.PanelContainer.Panels.Add(panel);
      return panel;
    }

    /// <summary>
    /// Получить именованную панель.
    /// </summary>
    /// <param name="name">Имя панели.</param>
    /// <returns>Панель.</returns>
    public IPanel GetNamedPanel(string name)
    {
      return this.PanelContainer.AnchorablePanels
        .Concat(this.PanelContainer.Panels)
        .FirstOrDefault(p => p.ContentId == name);
    }

    /// <summary>
    /// Получить панель.
    /// </summary>
    /// <typeparam name="TPanel">Тип панели.</typeparam>
    /// <returns>Панель.</returns>
    public TPanel GetPanel<TPanel>() where TPanel : IPanel
    {
      return this.PanelContainer.AnchorablePanels
        .Concat(this.PanelContainer.Panels)
        .OfType<TPanel>()
        .FirstOrDefault();
    }

    /// <summary>
    /// Получить список панелей.
    /// </summary>
    /// <typeparam name="TPanel">Тип панели.</typeparam>
    /// <returns>Список панелей.</returns>
    public IEnumerable<TPanel> GetPanels<TPanel>()
    {
      return this.PanelContainer.AnchorablePanels
        .Concat(this.PanelContainer.Panels)
        .OfType<TPanel>()
        .ToList();
    }

    #endregion
  }
}
