using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanLight.Models
{
	public class KanbanTask
	{
		public int KanbanTaskId { get; set; }
		public string Subject { get; set; }
		public string Description { get; set; }
		public string Tags { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime MovedAt { get; set; }
		public DateTime ChangedAt { get; set; }
		public ApplicationUser LastChangeBy { get; set; }
		public ApplicationUser Performer { get; set; }
		public ApplicationUser Creator { get; set; }

		public Lane CurrentLane { get; set; }
	}
}