namespace Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscribeModel",
                c => new
                    {
                        subscribeID = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.subscribeID);
            
            AlterColumn("dbo.AdminModel", "adminName", c => c.String(nullable: false));
            AlterColumn("dbo.AdminModel", "email", c => c.String(nullable: false));
            AlterColumn("dbo.AdminModel", "password", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AdminModel", "confirmPassword", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdminModel", "confirmPassword", c => c.String());
            AlterColumn("dbo.AdminModel", "password", c => c.String());
            AlterColumn("dbo.AdminModel", "email", c => c.String());
            AlterColumn("dbo.AdminModel", "adminName", c => c.String());
            DropTable("dbo.SubscribeModel");
        }
    }
}
