namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PinControls",
                c => new
                    {
                        PinControlId = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(),
                        PinNumber = c.Int(nullable: false),
                        PinStatus = c.String(),
                        PiUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PinControlId)
                .ForeignKey("dbo.PiUsers", t => t.PiUserId, cascadeDelete: true)
                .Index(t => t.PiUserId);
            
            CreateTable(
                "dbo.PiUsers",
                c => new
                    {
                        PiUserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Passward = c.String(),
                        Email = c.String(),
                        MacAddress = c.String(),
                    })
                .PrimaryKey(t => t.PiUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PinControls", "PiUserId", "dbo.PiUsers");
            DropIndex("dbo.PinControls", new[] { "PiUserId" });
            DropTable("dbo.PiUsers");
            DropTable("dbo.PinControls");
        }
    }
}
