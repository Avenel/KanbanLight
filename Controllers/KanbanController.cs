using KanbanLight.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KanbanLight.Controllers
{
	[Authorize]
    public class KanbanController : Controller
    {
        // GET: Kanban
        public ActionResult Index()
        {
			using (var db = new ApplicationDbContext())
			{
				var viewModel = new KanbanIndexViewModel()
				{
					Lanes = db.Lanes
							.Include(l => l.KanbanTasks.Select(k => k.Performer))
							.Include(l => l.KanbanTasks.Select(k => k.Creator))
							.ToList()
				};

	            return View(viewModel);
			}
        }
    }
}