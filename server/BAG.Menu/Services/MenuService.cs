using BAG.Cookin.Core;
using BAG.Cookin.Impl;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAG.Cookin.Core.Model;
using ServiceStack.ServiceInterface;

namespace BAG.Cookin.Web
{

  [Route("/menus")]
  public class MenusRequest : IReturn<IEnumerable<Menu>>
  {

  }

  public class MenuService : Service
  {
    public IEnumerable<Menu> Get(MenusRequest request)
    {
      var menus = Db.Select<Menu>();
      return menus;
    }

    public void Post(Menu request)
    {
      if (request.id > 0)
      {
        Db.Update<Menu>(request);
      }
      else
      {
        Db.Insert<Menu>(request);
      }

    }

  }
}