using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;
using Simple.Data;
using BAG.Menu.Core.Interface;
using BAG.Menu.Impl;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BAG.Menu.App_Start.AppHost), "Start")]

namespace BAG.Menu.App_Start
{
  public class AppHost : AppHostBase
  {
    public AppHost() : 
      base("StarterTemplate ASP.NET Host", typeof(HelloService).Assembly) { }

    public override void Configure(Funq.Container container)
    {
      //Set JSON web services to return idiomatic JSON camelCase properties
      ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

      //Configure User Defined REST Paths
      Routes
        .Add<Hello>("/hello")
        .Add<Hello>("/hello/{Name*}");

      //Uncomment to change the default ServiceStack configuration
      //SetConfig(new EndpointHostConfig {
      //});

      //Enable Authentication
      ConfigureAuth(container);

      //Register all your dependencies
      container.Register(new TodoRepository());

      //Requires ConnectionString configured in Web.Config
      var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
      container.Register<IDatabaseProvider>(c => new SimpleDatabaseProvider(connectionString));


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

      //Default route: /register
      Plugins.Add(new RegistrationFeature()); 

      /*
      container.Register<IUserAuthRepository>(c =>
        new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

      var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
      authRepo.CreateMissingTables();
       */
    }

    public static void Start()
    {
      new AppHost().Init();
    }
  }
}
