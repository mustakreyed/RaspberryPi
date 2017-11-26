namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealyModelincloud : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Relays",
                c => new
                    {
                        RelayId = c.Int(nullable: false, identity: true),
                        PiPinforModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RelayId)
                .ForeignKey("dbo.PiPinforModels", t => t.PiPinforModelId, cascadeDelete: true)
                .Index(t => t.PiPinforModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Relays", "PiPinforModelId", "dbo.PiPinforModels");
            DropIndex("dbo.Relays", new[] { "PiPinforModelId" });
            DropTable("dbo.Relays");
        }
    }
}
