namespace KanbanLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class laneAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lanes",
                c => new
                    {
                        LaneId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.LaneId);
            
            AddColumn("dbo.KanbanTasks", "Lane_LaneId", c => c.Int());
            CreateIndex("dbo.KanbanTasks", "Lane_LaneId");
            AddForeignKey("dbo.KanbanTasks", "Lane_LaneId", "dbo.Lanes", "LaneId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KanbanTasks", "Lane_LaneId", "dbo.Lanes");
            DropIndex("dbo.KanbanTasks", new[] { "Lane_LaneId" });
            DropColumn("dbo.KanbanTasks", "Lane_LaneId");
            DropTable("dbo.Lanes");
        }
    }
}
