using BAG.Menu.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace BAG.Menu
{
  public class Global : System.Web.HttpApplication
  {

    protected void Application_Start(object sender, EventArgs e)
    {
      AppHost.Start();
    }

    protected void Application_Error(object sender, EventArgs e)
    {
    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
  }
}