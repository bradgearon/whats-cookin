using BAG.Menu.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Menu.Core.Interface
{
  public interface IEmbedlyClient
  {
    Embed GetLinkResponse(string url, int? maxWidth);
  }
}
