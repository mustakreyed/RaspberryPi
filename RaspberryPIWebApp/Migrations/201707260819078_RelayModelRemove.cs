namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelayModelRemove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Relays", "PiPinforModelId", "dbo.PiPinforModels");
            DropIndex("dbo.Relays", new[] { "PiPinforModelId" });
            DropTable("dbo.Relays");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Relays",
                c => new
                    {
                        RelayId = c.Int(nullable: false, identity: true),
                        PiPinforModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RelayId);
            
            CreateIndex("dbo.Relays", "PiPinforModelId");
            AddForeignKey("dbo.Relays", "PiPinforModelId", "dbo.PiPinforModels", "PiPinforModelId", cascadeDelete: true);
        }
    }
}
