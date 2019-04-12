using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Data.DataContext;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Data.Initialize
{
    public class InitData : CreateDatabaseIfNotExists<VeccContext>
    {
        protected override void Seed(VeccContext context)
        {
            SysUser user = new SysUser { Name = "admin", CreateDate = DateTime.Now, IsDel = 0, Operator = 1, PassWord = "123456" };
            context.SysUser.Add(user);
            context.SaveChanges();
        }
    }
}
