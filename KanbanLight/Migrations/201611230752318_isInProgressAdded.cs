namespace KanbanLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isInProgressAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KanbanTasks", "IsInProgress", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KanbanTasks", "IsInProgress");
        }
    }
}
