namespace KanbanLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kanbanTaskAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KanbanTasks",
                c => new
                    {
                        KanbanTaskId = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Description = c.String(),
                        Tags = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        MovedAt = c.DateTime(nullable: false),
                        ChangedAt = c.DateTime(nullable: false),
                        Creator_Id = c.String(maxLength: 128),
                        LastChangeBy_Id = c.String(maxLength: 128),
                        Performer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.KanbanTaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.LastChangeBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Performer_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.LastChangeBy_Id)
                .Index(t => t.Performer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KanbanTasks", "Performer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.KanbanTasks", "LastChangeBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.KanbanTasks", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.KanbanTasks", new[] { "Performer_Id" });
            DropIndex("dbo.KanbanTasks", new[] { "LastChangeBy_Id" });
            DropIndex("dbo.KanbanTasks", new[] { "Creator_Id" });
            DropTable("dbo.KanbanTasks");
        }
    }
}
