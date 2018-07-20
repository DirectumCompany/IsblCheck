using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Package
{
  /// <summary>
  /// Провайдер пакета разработки.
  /// </summary>
  public class PackageProvider : IDevelopmentContextProvider
  {
    /// <summary>
    /// Логгер.
    /// </summary>
    private static readonly ILog log = LogManager.GetLogger<PackageProvider>();

    /// <summary>
    /// Загруженный пакет разработки.
    /// </summary>
    private ComponentsModel packageComponents;

    /// <summary>
    /// Имя файла пакета.
    /// </summary>
    public string PackageFilePath { get; }

    /// <summary>
    /// Прочитать список компонент.
    /// </summary>
    /// <typeparam name="T">Тип компоненты.</typeparam>
    /// <returns>Список компонент указанного типа.</returns>
    public IEnumerable<T> ReadComponents<T>() where T : Component
    {
      if (this.packageComponents == null)
        this.ReadPackage();

      var handlerType = ResolveComponentHandler<T>();
      if (handlerType == null)
        return Enumerable.Empty<T>();
      var handler = (IPackageHandler<T>)Activator.CreateInstance(handlerType);
      try
      {
        return handler.Read(this.packageComponents);
      }
      catch (Exception ex)
      {
        log.Error("Package reading error.", ex);
        throw;
      }
    }

    /// <summary>
    /// Получить тип обработчика для компоненты.
    /// </summary>
    /// <typeparam name="T">Тип компоненты.</typeparam>
    private static Type ResolveComponentHandler<T>() where T : Component
    {
      return Assembly.GetExecutingAssembly().DefinedTypes
        .FirstOrDefault(t => t.IsClass && t.ImplementedInterfaces.Contains(typeof(IPackageHandler<T>)));
    }

    /// <summary>
    /// Прочитать пакет.
    /// </summary>
    private void ReadPackage()
    {
      try
      {
        using (var fileStream = File.OpenRead(this.PackageFilePath))
        {
          var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Document };

          var serializer = new XmlSerializer(typeof(ComponentsModel));

          using (var reader = XmlReader.Create(fileStream, settings))
          {
            this.packageComponents = (ComponentsModel)serializer.Deserialize(reader);
          }
        }
      }
      catch (Exception ex)
      {
        log.Error("Package reading error.", ex);
        throw;
      }
    }

    /// <summary>
    /// Сбросить кэш.
    /// </summary>
    public void ResetCache()
    {
      this.packageComponents = null;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="filePath">Путь к файлу пакета.</param>
    public PackageProvider(string filePath)
    {
      this.PackageFilePath = filePath;
    }
  }
}
