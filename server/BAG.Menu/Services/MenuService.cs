using BAG.Menu.Core;
using BAG.Menu.Impl;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAG.Menu.Core.Model;
using ServiceStack.ServiceInterface;

namespace BAG.Menu
{

  [Route("/menus")]
  public class MenusRequest : IReturn<IEnumerable<Core.Model.Menu>>
  {

  }

  public class MenuService : Service
  {
    public IEnumerable<Core.Model.Menu> Get(MenusRequest request)
    {
      var menus = Db.Select<Core.Model.Menu>();
      return menus;
    }

    public void Post(Core.Model.Menu request)
    {
      if (request.id > 0)
      {
        Db.Update<Core.Model.Menu>(request);
      }
      else
      {
        Db.Insert<Core.Model.Menu>(request);
      }

    }

  }
}