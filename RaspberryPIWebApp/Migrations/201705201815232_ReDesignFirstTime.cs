namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReDesignFirstTime : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PinControls", "PiUserId", "dbo.PiUsers");
            DropIndex("dbo.PinControls", new[] { "PiUserId" });
            CreateTable(
                "dbo.Pis",
                c => new
                    {
                        PiId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PiModelId = c.Int(nullable: false),
                        PiHardwareId = c.Int(nullable: false),
                        ApplicatinUserId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PiId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.PiHardwares", t => t.PiHardwareId, cascadeDelete: true)
                .ForeignKey("dbo.PiModels", t => t.PiModelId, cascadeDelete: true)
                .Index(t => t.PiModelId)
                .Index(t => t.PiHardwareId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.PiHardwares",
                c => new
                    {
                        PiHardwareId = c.Int(nullable: false, identity: true),
                        Mac = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.PiHardwareId);
            
            CreateTable(
                "dbo.PiModels",
                c => new
                    {
                        PiModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(),
                        NumberofPin = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PiModelId);
            
            CreateTable(
                "dbo.PiPinforModels",
                c => new
                    {
                        PiPinforModelId = c.Int(nullable: false, identity: true),
                        PiPinNumber = c.Int(nullable: false),
                        Description = c.String(),
                        PiModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PiPinforModelId)
                .ForeignKey("dbo.PiModels", t => t.PiModelId, cascadeDelete: true)
                .Index(t => t.PiModelId);
            
            CreateTable(
                "dbo.PiPins",
                c => new
                    {
                        PiPinId = c.Int(nullable: false, identity: true),
                        ApplienceName = c.String(),
                        Location = c.String(),
                        Room = c.String(),
                        PinStatus = c.String(),
                        PinNumber = c.Int(nullable: false),
                        PiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PiPinId)
                .ForeignKey("dbo.Pis", t => t.PiId, cascadeDelete: true)
                .Index(t => t.PiId);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropTable("dbo.PinControls");
            DropTable("dbo.PiUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PiUsers",
                c => new
                    {
                        PiUserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Passward = c.String(),
                        Email = c.String(),
                        MacAddress = c.String(),
                    })
                .PrimaryKey(t => t.PiUserId);
            
            CreateTable(
                "dbo.PinControls",
                c => new
                    {
                        PinControlId = c.Int(nullable: false, identity: true),
                        Pin = c.Int(nullable: false),
                        Status = c.String(),
                        PiUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PinControlId);
            
            DropForeignKey("dbo.PiPins", "PiId", "dbo.Pis");
            DropForeignKey("dbo.Pis", "PiModelId", "dbo.PiModels");
            DropForeignKey("dbo.PiPinforModels", "PiModelId", "dbo.PiModels");
            DropForeignKey("dbo.Pis", "PiHardwareId", "dbo.PiHardwares");
            DropForeignKey("dbo.Pis", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PiPins", new[] { "PiId" });
            DropIndex("dbo.PiPinforModels", new[] { "PiModelId" });
            DropIndex("dbo.Pis", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Pis", new[] { "PiHardwareId" });
            DropIndex("dbo.Pis", new[] { "PiModelId" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.PiPins");
            DropTable("dbo.PiPinforModels");
            DropTable("dbo.PiModels");
            DropTable("dbo.PiHardwares");
            DropTable("dbo.Pis");
            CreateIndex("dbo.PinControls", "PiUserId");
            AddForeignKey("dbo.PinControls", "PiUserId", "dbo.PiUsers", "PiUserId", cascadeDelete: true);
        }
    }
}
