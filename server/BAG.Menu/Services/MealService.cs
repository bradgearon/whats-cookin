using System.ComponentModel;
using BAG.Cookin.Core;
using BAG.Cookin.Impl;
using BAG.Cookin.Web.App_Start;
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

  [Route("/meals/{date*}")]
  public class MealsRequest : IReturn<IEnumerable<Meal>>
  {
    public DateTime? date { get; set; }
  }

  public class MealsService : Service
  {
    public object Get(MealsRequest request)
    {
      var interval = ((AppHost) AppHost.Instance).Interval;
      var menuExp = Db.CreateExpression<Menu>();
      var date = request.date.HasValue ? request.date.Value.Date : DateTime.Now.Date;
      menuExp.Where(m => date >= m.intervalStart && date <= m.intervalEnd);

      var menu = Db.FirstOrDefault(menuExp);

      // none in the interval specified
      if (menu == null)
      {
        menuExp = Db.CreateExpression<Menu>();
        menuExp.OrderByDescending(m => m.intervalStart);
        menu = Db.FirstOrDefault(menuExp);

        if (menu != null)
        {
          var start = menu.intervalStart;
          double direction = DateTime.Compare(date, start);
          double daysBetween = Math.Abs((date - start).Days);
          int times = direction > 0 ?(int) Math.Floor(daysBetween / interval) 
            :(int) Math.Ceiling(daysBetween / interval);

          menu = new Menu();
          menu.intervalStart = start.AddDays(direction * (interval * times));
        }
        else if (menu == null)
        {
          menu = new Menu();
          menu.intervalStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        }

      }

      menu.intervalEnd = menu.intervalStart.AddDays(interval);

      // none
      var mealExp = Db.CreateExpression<Meal>();

      mealExp.Where(m => m.date >= menu.intervalStart && m.date <= menu.intervalEnd);
      var meals = Db.Select(mealExp);

      var mealItems = new List<Meal>();
      for (int i = 0; i < interval; i++)
      {
        var mealDate = menu.intervalStart.AddDays(i).Date;
        var meal = meals.FirstOrDefault(m => m.date.Date == mealDate);
        if(meal == null) {
          meal = new Meal{ date = mealDate };
        }
        mealItems.Add(meal);
      }
      return mealItems;
    }

    [Route("/meal")]
    public Meal Post(Meal request)
    {
      if (request.id > 0)
      {
        Db.Update<Meal>(request, m => m.id == request.id);
      }
      else
      {
        Db.Insert<Meal>(request);
        request.id = (int)Db.GetLastInsertId();
      }
      return request;
    }

  }
}