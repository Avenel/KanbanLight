using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanLight.Models
{
	public class Lane
	{
		public int LaneId { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public List<KanbanTask> KanbanTasks { get; set; }
		public int Position { get; set; }
	}
}