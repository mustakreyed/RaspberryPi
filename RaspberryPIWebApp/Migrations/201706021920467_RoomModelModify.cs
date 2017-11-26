namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoomModelModify : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "PiId", c => c.Int(nullable: false));
            CreateIndex("dbo.Rooms", "PiId");
            AddForeignKey("dbo.Rooms", "PiId", "dbo.Pis", "PiId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "PiId", "dbo.Pis");
            DropIndex("dbo.Rooms", new[] { "PiId" });
            DropColumn("dbo.Rooms", "PiId");
        }
    }
}
