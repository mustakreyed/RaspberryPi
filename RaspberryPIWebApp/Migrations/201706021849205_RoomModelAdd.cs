namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoomModelAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(),
                    })
                .PrimaryKey(t => t.RoomId);
            
            AddColumn("dbo.PiPins", "RoomId", c => c.Int(nullable: false));
            CreateIndex("dbo.PiPins", "RoomId");
            AddForeignKey("dbo.PiPins", "RoomId", "dbo.Rooms", "RoomId", cascadeDelete: true);
            DropColumn("dbo.PiPins", "Room");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PiPins", "Room", c => c.String());
            DropForeignKey("dbo.PiPins", "RoomId", "dbo.Rooms");
            DropIndex("dbo.PiPins", new[] { "RoomId" });
            DropColumn("dbo.PiPins", "RoomId");
            DropTable("dbo.Rooms");
        }
    }
}
