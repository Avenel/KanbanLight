namespace KanbanLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFKToKanbanTask : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.KanbanTasks", name: "Lane_LaneId", newName: "CurrentLane_LaneId");
            RenameIndex(table: "dbo.KanbanTasks", name: "IX_Lane_LaneId", newName: "IX_CurrentLane_LaneId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.KanbanTasks", name: "IX_CurrentLane_LaneId", newName: "IX_Lane_LaneId");
            RenameColumn(table: "dbo.KanbanTasks", name: "CurrentLane_LaneId", newName: "Lane_LaneId");
        }
    }
}
