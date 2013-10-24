using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Menu.Core.Interface
{
  public interface IDatabaseProvider
  {
    dynamic Db { get; }
  }
}
