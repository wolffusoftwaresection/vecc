using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Data.Initialize;

namespace Wolf.Vecc.Data.DataContext
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            Database.SetInitializer(new InitData());
            using (var db = new VeccContext())
            {
                db.Database.Initialize(false);
            }
        }
    }
}
