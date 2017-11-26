namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class theardMigaration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PinControls", "Pin", c => c.Int(nullable: false));
            AddColumn("dbo.PinControls", "Status", c => c.String());
            DropColumn("dbo.PinControls", "PinNumber");
            DropColumn("dbo.PinControls", "PinStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PinControls", "PinStatus", c => c.String());
            AddColumn("dbo.PinControls", "PinNumber", c => c.Int(nullable: false));
            DropColumn("dbo.PinControls", "Status");
            DropColumn("dbo.PinControls", "Pin");
        }
    }
}
