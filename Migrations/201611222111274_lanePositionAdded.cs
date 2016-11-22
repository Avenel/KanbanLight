namespace KanbanLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lanePositionAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lanes", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lanes", "Position");
        }
    }
}
