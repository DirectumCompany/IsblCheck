using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace IsblCheck.Common
{
  /// <summary>
  /// Утилиты для получения информации о приложении.
  /// </summary>
  public static class AppInfoUtils
  {
    /// <summary>
    /// Получить путь до папки приложения.
    /// </summary>
    /// <returns>Путь до папки приложения.</returns>
    public static string GetDirectoryPath()
    {
      return Path.GetDirectoryName(new System.Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
    }

    /// <summary>
    /// Получить имя продукта.
    /// </summary>
    /// <returns>Имя продукта.</returns>
    public static string GetProductName()
    {
      var assembly = Assembly.GetExecutingAssembly();
      var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

      return fvi.ProductName;
    }

    /// <summary>
    /// Получить версию продукта.
    /// </summary>
    /// <returns>Версия продукта.</returns>
    public static string GetProductVersion()
    {
      var assembly = Assembly.GetExecutingAssembly();
      var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

      return fvi.ProductVersion;
    }
  }
}
