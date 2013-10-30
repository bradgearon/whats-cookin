using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.ServiceInterface.Cors;
using BAG.Cookin.Impl;
using BAG.Cookin.Core;
using BAG.Cookin.Core.Model;
using BAG.Cookin.Core.Interface;

namespace BAG.Cookin.Web.App_Start
{
  public class AppHost : AppHostBase
  {
    public int Interval { get; set; }
    public AppHost() : base("StarterTemplate ASP.NET Host", typeof(EmbedService).Assembly) { }

    public override void Configure(Funq.Container container)
    {
      JsConfig.EmitCamelCaseNames = true;
      JsConfig.DateHandler = JsonDateHandler.ISO8601;

      Plugins.Add(new CorsFeature());
      this.PreRequestFilters.Add((httpReq, httpRes) =>
      {
        if (httpReq.HttpMethod == "OPTIONS")
        {
          httpRes.AddHeader("Access-Control-Allow-Methods", "PUT, DELETE, POST, GET, OPTIONS");
          httpRes.AddHeader("Access-Control-Allow-Headers", "X-Requested-With, Content-Type");
          httpRes.EndRequest();
        }
      });

      var embedKey = ConfigurationManager.AppSettings["EmbedKey"];
      var connectionString = ConfigurationManager.AppSettings["AppDbPath"].MapAbsolutePath();
      Interval = int.Parse(ConfigurationManager.AppSettings["Interval"]);

      var dialectProvider = new SqliteOrmLiteDialectProvider();
      var dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider);

      container.Register<IEmbedlyClient>(new EmbedlyClient(embedKey));
      container.Register<IDbConnectionFactory>(dbFactory);
      
      dbFactory.Run(db =>
      {
        if (!db.TableExists("Menu"))
        {
          var modelTypes = typeof(Meal).Assembly.GetTypes()
            .Where(t => t.Namespace.Contains("Model"));

          db.DropAndCreateTables(modelTypes.ToArray());

          var exp = db.CreateExpression<Menu>();
          var menu = db.FirstOrDefault(exp);
          if (menu == null)
          {
            menu = new Menu();
            menu.intervalStart = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Date;
            menu.intervalEnd = DateTime.Now.Date;

            db.Insert(menu);
          }

          var newMeals = new List<Meal>();
          var existingMeals = new List<Meal>();

          for (int i = 0; i < Interval; i++)
          {
            var meal = db.GetByIdOrDefault<Meal>(i + 1);
            if (meal == null)
            {
              meal = new Meal();
              newMeals.Add(meal);
            }
            else
            {
              existingMeals.Add(meal);
            }

            meal.menuId = menu.id;
            meal.date = DateTime.Now.AddDays(-Interval + i).Date;
            meal.title = "test meal " + i;
            meal.url = "http://localhost/meal/" + i;
          }

          db.InsertAll(newMeals);
          db.UpdateAll(existingMeals);
        }
      });
    }

    public static void Start()
    {
      new AppHost().Init();


    }
  }
}
