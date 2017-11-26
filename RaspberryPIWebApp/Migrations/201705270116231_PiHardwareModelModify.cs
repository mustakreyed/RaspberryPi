namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PiHardwareModelModify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PiHardwares", "PiModelId", c => c.Int(nullable: false));
            CreateIndex("dbo.PiHardwares", "PiModelId");
            AddForeignKey("dbo.PiHardwares", "PiModelId", "dbo.PiModels", "PiModelId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PiHardwares", "PiModelId", "dbo.PiModels");
            DropIndex("dbo.PiHardwares", new[] { "PiModelId" });
            DropColumn("dbo.PiHardwares", "PiModelId");
        }
    }
}
