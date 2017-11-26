namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SensorModeladd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        SensorsId = c.Int(nullable: false, identity: true),
                        Temparature = c.String(),
                        Light = c.String(),
                    })
                .PrimaryKey(t => t.SensorsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sensors");
        }
    }
}
