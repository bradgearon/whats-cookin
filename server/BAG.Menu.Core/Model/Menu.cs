using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Cookin.Core.Model
{
  [Route("/menu")]
  public class Menu
  {
    [AutoIncrement, PrimaryKey]
    public int id { get; set; }
    public DateTime intervalStart { get; set; }
    public DateTime intervalEnd { get; set; }
  }
}