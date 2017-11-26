namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectionApplicationUserSpalling : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Pis", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Pis", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            DropColumn("dbo.Pis", "ApplicatinUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pis", "ApplicatinUserId", c => c.String());
            RenameIndex(table: "dbo.Pis", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Pis", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
