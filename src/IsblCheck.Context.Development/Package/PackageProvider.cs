using Common.Logging;
using IsblCheck.Context.Development.Package.Handlers;
using IsblCheck.Context.Development.Package.Models;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

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
    public string PackageFilePath { get; private set; }

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
      catch(Exception ex)
      {
        log.Error("Package reading error.", ex);
        throw;
      }
    }

    /// <summary>
    /// Получить тип обработчика для компоненты.
    /// </summary>
    /// <typeparam name="T">Тип компоненты.</typeparam>
    private Type ResolveComponentHandler<T>() where T : Component
    {
      return Assembly.GetExecutingAssembly().DefinedTypes
        .Where(t => t.IsClass)
        .Where(t => t.ImplementedInterfaces.Contains(typeof(IPackageHandler<T>)))
        .FirstOrDefault();
    }

    /// <summary>
    /// Прочитать пакет.
    /// </summary>
    private void ReadPackage()
    {
      try
      {
        using (FileStream fileStream = File.OpenRead(this.PackageFilePath))
        {
          var settings = new XmlReaderSettings();
          settings.ConformanceLevel = ConformanceLevel.Document;

          var serializer = new XmlSerializer(typeof(ComponentsModel));

          using (XmlReader reader = XmlReader.Create(fileStream, settings))
          {
            this.packageComponents = (ComponentsModel)serializer.Deserialize(reader);
          }
        }
      }
      catch(Exception ex)
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
