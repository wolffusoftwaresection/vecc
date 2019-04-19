using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Core.Context
{
    public class ConnectionString
    {
        public readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["vecc"].ToString();
    }
}
