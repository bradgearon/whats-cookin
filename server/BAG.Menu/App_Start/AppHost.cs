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
    public AppHost() : base("StarterTemplate ASP.NET Host", typeof(EmbedService).Assembly) { }

    public override void Configure(Funq.Container container)
    {
      ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

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

          db.CreateTables(true, modelTypes.ToArray());

          var menu = db.GetByIdOrDefault<Core.Model.Menu>(1);
          if (menu == null)
          {
            menu = new Core.Model.Menu();
            db.Insert(menu);
          }

          menu.intervalStart = DateTime.Now.AddDays(-15);
          menu.intervalEnd = DateTime.Now;
          db.Update(menu);

          var newMeals = new List<Meal>();
          var existingMeals = new List<Meal>();

          for (int i = 0; i < 15; i++)
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
            meal.date = DateTime.Now.AddDays(-15 + i);
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
