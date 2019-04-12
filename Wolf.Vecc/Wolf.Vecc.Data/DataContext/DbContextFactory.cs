using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Data.DataContext
{
    public class DbContextFactory
    {
        //DbContext在System.Data.Entity;中，不过这里直接只引用这一个不行，还有EF其他的一些NameSpace所以直接添加一个实体模型，所有引用都进来了，然后再把模型删了
        public static DbContext CallDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new VeccContext();
                CallContext.SetData("dbContext", dbContext);
            }
            return dbContext;
        }
    }
}
