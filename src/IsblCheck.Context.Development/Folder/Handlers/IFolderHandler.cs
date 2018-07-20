using System.Collections.Generic;
using IsblCheck.Core.Context.Development;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal interface IFolderHandler<out T> where T: Component
  {
    IEnumerable<T> Read(string workspacePath);
  }
}
