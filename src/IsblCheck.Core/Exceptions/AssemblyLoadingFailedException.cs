using System;
using IsblCheck.Core.Properties;

namespace IsblCheck.Core.Exceptions
{
  /// <summary>
  /// Ошибка загрузки сборки.
  /// </summary>
  public class AssemblyLoadingFailedException : Exception
  {
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="path">Путь.</param>
    public AssemblyLoadingFailedException(string path) : 
      base(string.Format(Resources.ERROR_COULD_NOT_LOAD_ASSEMBLY, path))
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="path">Путь.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public AssemblyLoadingFailedException(string path, Exception innerException) :
      base(string.Format(Resources.ERROR_COULD_NOT_LOAD_ASSEMBLY, path), innerException)
    {
    }
  }
}
