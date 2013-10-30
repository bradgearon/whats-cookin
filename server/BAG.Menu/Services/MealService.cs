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
      var date = request.date ?? DateTime.Now;
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
          var direction = DateTime.Compare(date, start);
          var daysBetween = Math.Abs(Math.Truncate((date - start).TotalDays));
          var times = Math.Ceiling(daysBetween / interval);

          menu = new Menu();
          menu.intervalStart = start.AddDays(direction * (interval * times));
          menu.intervalEnd = menu.intervalStart.AddDays(interval);
        }
        else if (menu == null)
        {
          menu = new Menu();
          menu.intervalStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
          menu.intervalEnd = menu.intervalStart.AddDays(interval + 1);
        }

      }

      // none
      var mealExp = Db.CreateExpression<Meal>();
      mealExp.Where(m => menu.intervalEnd >= m.date && date <= menu.intervalStart);
      var meals = Db.Select(mealExp).Take(interval);

      var mealItems = from index in Enumerable.Range(0, interval)
                      let mealDate = menu.intervalStart.AddDays(index).Date
                      join existingMeal in meals on mealDate equals existingMeal.date.Date into spanMeals
                      from meal in spanMeals.DefaultIfEmpty()
                      select meal ?? new Meal
                      {
                        date = mealDate
                      };
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