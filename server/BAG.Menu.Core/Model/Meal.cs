using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Menu.Core.Model
{
  [Route("/meal")]
  public class Meal
  {
    [AutoIncrement, PrimaryKey]
    public int id { get; set; }
    public DateTime date { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public string rating { get; set; }
    public string thumbnail_url { get; set; }
    public string description { get; set; }

    [ForeignKey(typeof(Menu))]
    public int menuId { get; set; }
  }
}
