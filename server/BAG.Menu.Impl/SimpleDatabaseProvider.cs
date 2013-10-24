using BAG.Menu.Core.Interface;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAG.Menu.Impl
{
  public class SimpleDatabaseProvider : IDatabaseProvider
  {
    public SimpleDatabaseProvider(string connectionString)
    {
      Db = Database.OpenConnection(connectionString);
    }

    public dynamic Db { get; protected set; }
  }
}
