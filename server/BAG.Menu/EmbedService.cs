using BAG.Menu.Core;
using BAG.Menu.Core.Interface;
using BAG.Menu.Impl;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Cors;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAG.Menu
{

  [Route("/embed")]
  public class EmbedRequest : IReturn<Embed>
  {
    public string url { get; set; }
    public int? width { get; set; }
  }

  public class EmbedService : Service
  {
    public IEmbedlyClient Client { get; set; }
    public Embed Get(EmbedRequest request)
    {
      var response = Client.GetLinkResponse(request.url, request.width);
      return response;
    }
  }
}