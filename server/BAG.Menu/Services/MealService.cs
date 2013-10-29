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

  [Route("/meals")]
  public class MealsRequest : IReturn<IEnumerable<Meal>>
  {
    public DateTime start { get; set; }
    public DateTime end { get; set; }
  }

  public class MealsService : Service
  {
    public IEnumerable<Meal> Get(MealsRequest request)
    {
      var meals = Db.Select<Meal>();
      return meals;
    }

    [Route("/meal")]
    public void Post(Meal request)
    {
      if (request.id > 0)
      {
        Db.Update<Meal>(request);
      }
      else
      {
        Db.Insert<Meal>(request);
      }

    }

  }
}