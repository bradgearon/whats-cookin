using BAG.Cookin.Core;
using BAG.Cookin.Core.Interface;
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

namespace BAG.Cookin.Web
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