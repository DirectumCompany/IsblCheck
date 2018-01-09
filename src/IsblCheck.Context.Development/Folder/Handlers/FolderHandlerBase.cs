using Common.Logging;
using IsblCheck.Core.Context.Development;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  /// <summary>
  /// Базовый загрузчик компонент из папки с разработкой.
  /// </summary>
  /// <typeparam name="T">Тип загружаемых компонент.</typeparam>
  /// <typeparam name="TModel">Модель компоненты, которая используется при загрузке описания компоненты из файла Card.xml.</typeparam>
  internal abstract class FolderHandlerBase<T, TModel>: IFolderHandler<T> where T : Component
  {
    #region Константы

    /// <summary>
    /// Имя файла с описанием компоненты (карточка).
    /// </summary>
    protected const string CardFileName = "Card.xml"; 

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Имя папки с компонентами в каталоге с разработкой.
    /// </summary>
    protected abstract string FolderName { get; }

    /// <summary>
    /// Имя корневого узла модели в xml описании.
    /// </summary>
    protected abstract string CardModelRootNode { get; }

    private static readonly ILog log = LogManager.GetLogger<FolderHandlerBase<T, TModel>>(); 

    #endregion

    #region IFolderHandler<T>

    public IEnumerable<T> Read(string workspacePath)
    {
      var targetDirectory = Path.Combine(workspacePath, FolderName);

      if (!Directory.Exists(targetDirectory))
      {
        log.Warn($"The directory does not exist: {targetDirectory}");
        yield break;
      }

      foreach (var componentFolderPath in Directory.EnumerateDirectories(targetDirectory))
      {
        var model = this.ReadModel(Path.Combine(componentFolderPath, CardFileName), CardModelRootNode);
        foreach (var component in this.ReadComponents(model, componentFolderPath))
        {
          yield return component;
        }
      }
    } 

    #endregion

    #region Методы

    /// <summary>
    /// Прочитать компоненты из модели.
    /// </summary>
    /// <param name="model">Модель компоненты.</param>
    /// <param name="componentFolderPath">Путь до папки с компонентой.</param>
    /// <returns>Список загруженных компонент</returns>
    protected abstract IEnumerable<T> ReadComponents(TModel model, string componentFolderPath);

    private TModel ReadModel(string cardFilePath, string rootElementName)
    {
      using (var fileStream = File.OpenRead(cardFilePath))
      {
        var settings = new XmlReaderSettings();
        var serializer = new XmlSerializer(typeof(TModel), new XmlRootAttribute(rootElementName));
        using (XmlReader reader = XmlReader.Create(fileStream, settings))
        {
          return (TModel)serializer.Deserialize(reader);
        }
      }
    }

    #endregion
  }
}
