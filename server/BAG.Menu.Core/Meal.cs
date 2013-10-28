using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Menu.Core
{
  public class Meal
  {
    [AutoIncrement, PrimaryKey]
    public int id { get; set; }
    public DateTime date { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public string rating { get; set; }
    [ForeignKey(typeof(Menu))]
    public int menuId { get; set; }
  }
}
