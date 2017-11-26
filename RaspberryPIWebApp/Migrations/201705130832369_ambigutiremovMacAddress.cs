namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ambigutiremovMacAddress : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PinControls", "MacAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PinControls", "MacAddress", c => c.String());
        }
    }
}
