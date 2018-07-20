using System;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package.Handlers
{
  /// <summary>
  /// Утилиты по работе с пакетом.
  /// </summary>
  internal static class PackageHandlerUtils
  {
    /// <summary>
    /// Создать сущность из модели.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <param name="model">Модель.</param>
    /// <returns>Сущность.</returns>
    internal static T CreateEntity<T>(ComponentModel model) where T : Component
    {
      var entity = Activator.CreateInstance<T>();
      entity.Name = model.KeyValue;
      entity.Title = model.DisplayValue;
      return entity;
    }
  }
}
