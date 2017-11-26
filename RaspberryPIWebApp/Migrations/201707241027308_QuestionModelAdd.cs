namespace RaspberryPIWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionModelAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
        }
    }
}
