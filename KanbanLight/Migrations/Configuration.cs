namespace KanbanLight.Migrations
{
	using KanbanLight.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KanbanLight.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "KanbanLight.Models.ApplicationDbContext";
        }

        protected override void Seed(KanbanLight.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

			// Backlog, Bereit, Coding, Test, Bestätigung und Erledig
			context.Lanes.AddOrUpdate( 
				l => l.LaneId,
				new Lane() { LaneId = 1, Name = "backlog", DisplayName = "Backlog", Position = 1, KanbanTasks = new List<KanbanTask>()},
				new Lane() { LaneId = 2, Name = "coding", DisplayName = "Coding", Position = 2, KanbanTasks = new List<KanbanTask>()},
				new Lane() { LaneId = 3, Name = "test", DisplayName = "Test", Position = 3, KanbanTasks = new List<KanbanTask>()},
				new Lane() { LaneId = 4, Name = "done", DisplayName = "Erledigt", Position = 4, KanbanTasks = new List<KanbanTask>()}
			);
        }
    }
}
