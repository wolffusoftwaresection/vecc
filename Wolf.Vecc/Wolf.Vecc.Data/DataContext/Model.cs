namespace Wolf.Vecc.Data.DataContext
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Wolf.Vecc.Model.SysModel;

    public class VeccContext : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“Model”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“Wolf.Vecc.Data.DataContext.Model”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“Model”
        //连接字符串。
        public VeccContext()
            : base("name=vecc")
        {
            //自动迁移设置
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysData> SysData { get; set; }
        public virtual DbSet<SysLog> SysLog { get; set; }
        public virtual DbSet<SysApprovaData> SysApprovaData { get; set; }
        public virtual DbSet<SysApprovaUser> SysApprovaUser { get; set; }
        public virtual DbSet<SysParams> SysParams { get; set; }
        public virtual DbSet<SysUsers> SysUser { get; set; }
        public virtual DbSet<SysPemsTask> SysPemsTask { get; set; }
        public virtual DbSet<SysTask> SysTask { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}