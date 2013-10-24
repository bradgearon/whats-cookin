using BAG.Menu.Core;
using BAG.Menu.Impl;
using ServiceStack.ServiceHost;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAG.Menu
{
  public class Meal
  {
    public DateTime date { get; set; }
    public string name { get; set; }
    public string url { get; set; }
  }

  [Route("/meal", "GET")]
  public class MealRequest : IReturn<IEnumerable<Meal>>
  {
    public DateTime start { get; set; }
    public DateTime end { get; set; }
  }

  public class MealsService : Service
  {
    public IEnumerable<Meal> Get(MealRequest request)
    {
      var meals = Db.Meals.All()
        .Select();
      if (request.start != null)
      {
        meals.Where(request.start >= Db.Meals.Date);
      }


      return meals;
    }
  }
}