using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KanbanLight.Models;

namespace KanbanLight.Controllers
{
	[Authorize]
    public class KanbanTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KanbanTasks
        public ActionResult Index()
        {
            return View(db.Tasks.ToList());
        }

        // GET: KanbanTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KanbanTask kanbanTask = db.Tasks.Find(id);
            if (kanbanTask == null)
            {
                return HttpNotFound();
            }
            return View(kanbanTask);
        }

        // GET: KanbanTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KanbanTasks/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Subject,Description,Tags")] KanbanTask kanbanTask)
        {
            if (ModelState.IsValid)
            {
				kanbanTask.CreatedAt = DateTime.Now;
				kanbanTask.ChangedAt = DateTime.Now;
				kanbanTask.MovedAt = DateTime.Now;
				kanbanTask.DueDate = DateTime.Now.AddDays(7);
				kanbanTask.Creator = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
				kanbanTask.Performer = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
				kanbanTask.LastChangeBy = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                db.Tasks.Add(kanbanTask);
                db.SaveChanges();

				var firstLane = db.Lanes
					.Include(l => l.KanbanTasks)
					.FirstOrDefault(l => l.Position == 1);
				if (firstLane != null)
				{
					kanbanTask = db.Tasks
						.OrderByDescending(t => t.CreatedAt)
						.FirstOrDefault();

					firstLane.KanbanTasks.Add(kanbanTask);
					db.SaveChanges();
				}
			}
			else
			{
				return RedirectToAction("Create");
			}

			return RedirectToAction("Index", "Kanban");
        }

        // GET: KanbanTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KanbanTask kanbanTask = db.Tasks.Find(id);
            if (kanbanTask == null)
            {
                return HttpNotFound();
            }
            return View(kanbanTask);
        }

        // POST: KanbanTasks/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KanbanTaskId,Subject,Description,Tags,CreatedAt,DueDate,MovedAt,ChangedAt")] KanbanTask kanbanTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kanbanTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kanbanTask);
        }

        // GET: KanbanTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KanbanTask kanbanTask = db.Tasks.Find(id);
            if (kanbanTask == null)
            {
                return HttpNotFound();
            }
            return View(kanbanTask);
        }

		public ActionResult ToNext(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			KanbanTask kanbanTask = db.Tasks.Find(id);
			ChangeLane(kanbanTask, 1);

			return RedirectToAction("Index", "Kanban");
		}

		public ActionResult ToPrevious(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			KanbanTask kanbanTask = db.Tasks.Find(id);
			ChangeLane(kanbanTask, -1);

			return RedirectToAction("Index", "Kanban");
		}

		private void ChangeLane(KanbanTask kanbanTask, int diff)
		{
			var currLane = db.Lanes
				.Include(l => l.KanbanTasks)
				.FirstOrDefault(l => l.KanbanTasks.Any(t => t.KanbanTaskId == kanbanTask.KanbanTaskId));

			if (currLane != null)
			{
				var nextLanePos = currLane.Position + diff;
				var nextLane = db.Lanes
					.Include(l => l.KanbanTasks)
					.FirstOrDefault(l => l.Position == nextLanePos);

				if (nextLane != null)
				{
					kanbanTask.Performer = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
					kanbanTask.LastChangeBy = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
					kanbanTask.MovedAt = DateTime.Now;
					kanbanTask.ChangedAt = DateTime.Now;

                    if (nextLane.Position > 1 && nextLane.Position < db.Lanes.ToList().Count())
                    {
                        kanbanTask.IsInProgress = true;
                    }

					currLane.KanbanTasks.Remove(kanbanTask);
					nextLane.KanbanTasks.Add(kanbanTask);
					db.SaveChanges();
				}
			}
		}

		public ActionResult toggleIsInProgress(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			KanbanTask kanbanTask = db.Tasks.Find(id);
			kanbanTask.IsInProgress = !kanbanTask.IsInProgress;
			db.SaveChanges();

			return RedirectToAction("Index", "Kanban");
		}

        // POST: KanbanTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KanbanTask kanbanTask = db.Tasks.Find(id);
            db.Tasks.Remove(kanbanTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
