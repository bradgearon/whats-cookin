using BAG.Menu.Core.Interface;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Menu.Core
{
  public class Service : IService
  {
    public IDatabaseProvider DbProvider { get; set; }

    public dynamic Db { get { return DbProvider.Db; } }

    public Service()
    {

    }
  }
}
