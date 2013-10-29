using BAG.Menu.Core;
using BAG.Menu.Core.Interface;
using Embedly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embedly.OEmbed;
using System.Diagnostics;
using BAG.Menu.Core.Model;
namespace BAG.Menu.Impl
{
  public class EmbedlyClient : IEmbedlyClient
  {
    private string key;

    public EmbedlyClient(string key)
    {
      this.key = key;
    }

    public Embed GetLinkResponse(string url, int? maxWidth)
    {
      Client client = new Client(key);
      var options = new RequestOptions();
      if (maxWidth.HasValue)
      {
        options.MaxWidth = maxWidth.Value;
      }

      try
      {
        var result = client.GetOEmbed(new Uri(url), options).Response.AsLink;

        if (result != null)
        {
          var embed = new Embed
          {
            Author = result.Author,
            Description = result.Description,
            Provider = result.Provider,
            Thumbnail = result.ThumbnailUrl,
            Title = result.Title,
            Url = result.Url
          };

          return embed;
        }
      }
      catch(Exception ex)
      {
        Debug.WriteLine("fail during embed " + ex.ToString());
      }
      return null;
    }
  }
}
