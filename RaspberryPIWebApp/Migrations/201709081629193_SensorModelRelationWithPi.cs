namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SensorModelRelationWithPi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sensors", "PiId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sensors", "PiId");
            AddForeignKey("dbo.Sensors", "PiId", "dbo.Pis", "PiId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sensors", "PiId", "dbo.Pis");
            DropIndex("dbo.Sensors", new[] { "PiId" });
            DropColumn("dbo.Sensors", "PiId");
        }
    }
}
