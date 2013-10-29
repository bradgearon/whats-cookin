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
using System.Configuration;

namespace BAG.Cookin.Web
{

  [Route("/meals")]
  public class MealsRequest : IReturn<IEnumerable<Meal>>
  {
    public DateTime? date { get; set; }
  }

  public class MealsService : Service
  {
    public object Get(MealsRequest request)
    {
      var interval = int.Parse(ConfigurationManager.AppSettings["Interval"]);
      var menuExp = Db.CreateExpression<Menu>();
      var date = request.date ?? DateTime.Now;
      menuExp.Where(m => request.date >= m.intervalStart && request.date <= m.intervalEnd);
      var menu = Db.FirstOrDefault(menuExp);
      
      // none in the interval specified
      if (menu == null)
      {
        menuExp = Db.CreateExpression<Menu>();
        var maxIntervalStart = Db.CreateExpression().
        <DateTime>(m => Sql.Max(m.intervalStart));

        menuExp = Db.CreateExpression<Menu>();
        menuExp.Where(m => m.intervalStart == maxIntervalStart);
        menu = Db.FirstOrDefault(menuExp);
        if (menu != null)
        {
          var start = menu.intervalStart;
          var direction = DateTime.Compare(date, start);
          var daysBetween = Math.Truncate((date - start).TotalDays);
          var times = Math.Truncate(daysBetween / interval);

          menu = new Menu();
          menu.intervalStart = start.AddDays(direction * (interval * times));
          menu.intervalEnd = menu.intervalStart.AddDays(interval);
        }
      }

      // none
      if (menu == null)
      {
        menu = new Menu();
        menu.intervalStart = DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek);
        menu.intervalEnd = menu.intervalStart.AddDays(interval);
      }
      return menu;
      var mealExp = Db.CreateExpression<Meal>();
      var meals = Db.Select(mealExp);
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