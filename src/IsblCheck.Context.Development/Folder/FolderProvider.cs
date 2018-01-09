using Common.Logging;
using IsblCheck.Context.Development.Folder.Handlers;
using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IsblCheck.Context.Development.Folder
{
  public class FolderProvider : IDevelopmentContextProvider
  {
    private static readonly ILog log = LogManager.GetLogger<FolderProvider>();

    private readonly string workspacePath;

    public IEnumerable<T> ReadComponents<T>() where T : Component
    {
      var handlerType = ResolveComponentHandler<T>();
      if (handlerType == null)
        return Enumerable.Empty<T>();
      var handler = (IFolderHandler<T>)Activator.CreateInstance(handlerType);
      try
      {
        return handler.Read(this.workspacePath);
      }
      catch (Exception ex)
      {
        log.Error("Components reading error.", ex);
        throw;
      }
    }

    public void ResetCache()
    {
    }

    public FolderProvider(string workspacePath)
    {
      this.workspacePath = workspacePath;
    }

    private Type ResolveComponentHandler<T>() where T : Component
    {
      return Assembly.GetExecutingAssembly().DefinedTypes
        .Where(t => t.IsClass)
        .Where(t => !t.IsAbstract)
        .Where(t => t.ImplementedInterfaces.Contains(typeof(IFolderHandler<T>)))
        .FirstOrDefault();
    }
  }
}
