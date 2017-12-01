namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sensormodelRedesign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sensors", "TemparatureControledPin", c => c.Int(nullable: false));
            AddColumn("dbo.Sensors", "LightControledPin", c => c.Int(nullable: false));
            AddColumn("dbo.Sensors", "WaterControledPin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sensors", "WaterControledPin");
            DropColumn("dbo.Sensors", "LightControledPin");
            DropColumn("dbo.Sensors", "TemparatureControledPin");
        }
    }
}
