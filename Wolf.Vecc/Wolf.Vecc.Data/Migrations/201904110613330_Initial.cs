namespace Wolf.Vecc.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sys_User", "Name", c => c.String(maxLength: 100, storeType: "nvarchar"));
            AlterColumn("dbo.Sys_User", "PassWord", c => c.String(maxLength: 200, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sys_User", "PassWord", c => c.String(unicode: false));
            AlterColumn("dbo.Sys_User", "Name", c => c.String(unicode: false));
        }
    }
}
