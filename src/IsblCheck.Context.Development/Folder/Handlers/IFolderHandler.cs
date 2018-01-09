using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal interface IFolderHandler<T> where T: Component
  {
    IEnumerable<T> Read(string workspacePath);
  }
}
