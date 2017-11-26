namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class waterSensorFieldAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sensors", "Water", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sensors", "Water");
        }
    }
}
