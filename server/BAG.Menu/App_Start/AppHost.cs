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
using Simple.Data;
using BAG.Menu.Core.Interface;
using BAG.Menu.Impl;
using ServiceStack.ServiceInterface.Cors;
using BAG.Menu.Core;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BAG.Menu.App_Start.AppHost), "Start")]

namespace BAG.Menu.App_Start
{
  public class AppHost : AppHostBase
  {
    public AppHost() :
      base("StarterTemplate ASP.NET Host", typeof(EmbedService).Assembly) { }

    public override void Configure(Funq.Container container)
    {
      //Set JSON web services to return idiomatic JSON camelCase properties
      ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

      Plugins.Add(new CorsFeature());
      this.PreRequestFilters.Add((httpReq, httpRes) =>
      {
        //Handles Request and closes Responses after emitting global HTTP Headers
        if (httpReq.HttpMethod == "OPTIONS")
        {
          httpRes.AddHeader("Access-Control-Allow-Methods", "PUT, DELETE, POST, GET, OPTIONS");
          httpRes.AddHeader("Access-Control-Allow-Headers", "X-Requested-With, Content-Type");
          httpRes.EndRequest();
        }
      });

      //Uncomment to change the default ServiceStack configuration
      SetConfig(new EndpointHostConfig
      {
        DefaultContentType = "application/json"
      });

      //Enable Authentication
      ConfigureAuth(container);

      var embedKey = ConfigurationManager.AppSettings["EmbedKey"];
      container.Register<IEmbedlyClient>(new EmbedlyClient(embedKey));


      //Requires ConnectionString configured in Web.Config
      var connectionString = ConfigurationManager.AppSettings["AppDbPath"].MapAbsolutePath();



      container.Register<IDatabaseProvider>(c =>
      {
        var provider = new SimpleDatabaseProvider(connectionString);
        var dbFactory = new OrmLiteConnectionFactory(connectionString, new SqliteOrmLiteDialectProvider());
        dbFactory.Run(db =>
        {
          var modelTypes = typeof(Meal).Assembly.GetTypes().Where(t => t.Namespace.Contains("Model"));
          db.CreateTables(false, modelTypes.ToArray());

          var menu = db.GetByIdOrDefault<Core.Menu>(1);
          if (menu == null)
          {
            menu = new Core.Menu();
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

        });

        return provider;
      });

      


    }

    private void ConfigureAuth(Funq.Container container)
    {
      var appSettings = new AppSettings();

      //Default route: /auth/{provider}
      Plugins.Add(new AuthFeature(() => new CustomUserSession(),
        new IAuthProvider[] {
          new CredentialsAuthProvider(appSettings), 
          new FacebookAuthProvider(appSettings), 
          new TwitterAuthProvider(appSettings), 
          new BasicAuthProvider(appSettings), 
        }));


      Plugins.Add(new RegistrationFeature());
      Plugins.Add(new CorsFeature());

    }

    public static void Start()
    {
      new AppHost().Init();
    }
  }
}
