using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using RifleRange.DAL;
using RifleRange.Models;

namespace RifleRange.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        public ActionResult Index()
        {
            LinkedList<rrForumThread> llThreads = rrForumThreadDB.GetForumTopThread(ForumId: rrForumThreadDB.ForumId);
            ForumThread[] arrModel = new ForumThread[llThreads.Count];

            int i = 0;

            foreach (rrForumThread Thread in llThreads)
            {
                arrModel[i] = new ForumThread(Thread);
                i++;
            }

            rrForum Forum = rrForumDB.GetForum(rrForumThreadDB.ForumId);

            ViewData["ForumTitle"] = Forum.Title;
            
            return View(arrModel);
        }
        // GET: Forum/Details/5
        public ActionResult Details(int Id)
        {
            LinkedList<rrForumThread> llThreads = rrForumThreadDB.GetForumThread(ForumId: rrForumThreadDB.ForumId, 
                StartThreadId: Id);

            ForumThread[] arrModel = new ForumThread[llThreads.Count];

            int i = 0;

            foreach (rrForumThread Thread in llThreads)
            {
                arrModel[i] = new ForumThread(Thread);
                i++;
            }

            return View(arrModel);
        }
        public ActionResult DetailsTree(int Id)
        {
            ViewData["DataUrl"] = "/api/forum";
            ViewData["TopThreadId"] = Id;
            if (User.Identity.IsAuthenticated && Session["User"] != null)
                ViewData["UserId"] = ((rrUser)Session["User"]).UserId;

            return View();
        }
        // GET: Forum/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(ForumThread Model)
        {
            if (!ModelState.IsValid) return View();

            string FileName = null;
            if (Model.File != null)
            {
                FileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), Model.File.FileName);
                var FilePath = Server.MapPath(Path.Combine("~/Files", FileName));
                Model.File.SaveAs(FilePath);
            }
            string Description = Uri.UnescapeDataString(Model.Description);

            rrUser User = (rrUser)Session["User"];

            rrForumThreadDB.InsertForumThread(ForumId: rrForumThreadDB.ForumId, UserId: User.UserId,
                Title: Model.Title, Description: Description, ThreadParentId: null, FileName: FileName);

            return RedirectToAction("Index");
        }
        // Get: Forum/Edit/id
        [Authorize]
        public ActionResult Edit(int Id)
        {
            LinkedList<rrForumThread> llForumThread = rrForumThreadDB.GetForumThread(ForumId: rrForumThreadDB.ForumId, ThreadId: Id);
            if (llForumThread.Count == 0)
            {
                ViewData["ErrorMessage"] = string.Format("Тема не найдена");
                return View("Error");
            }
            var Thread = llForumThread.First.Value;

            ForumThread Model = new ForumThread(Thread);

            return View(Model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, ForumThread Model)
        {
            if (!ModelState.IsValid) return View();

            LinkedList<rrForumThread> llThread = rrForumThreadDB.GetForumThread(
                ForumId: rrForumThreadDB.ForumId, 
                ThreadId: id);
            if (llThread.Count == 0)
            {
                ViewData["ErrorMessage"] = string.Format("Тема не найдена");
                return View("Error");
            }
            rrForumThread ForumThread = llThread.First.Value;

            string FileName = ForumThread.FileName;

            if (!string.IsNullOrEmpty(FileName) && (Model.DeleteFile || Model.File != null))
            {
                string FilePath = Server.MapPath(string.Format("~/Files/{0}", FileName));
                System.IO.File.Delete(FilePath);
                FileName = null;
            }
            if (Model.File != null && !Model.DeleteFile)
            {
                FileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), Model.File.FileName);
                var FilePath = Server.MapPath(Path.Combine("~/Files", FileName));
                Model.File.SaveAs(FilePath);
            }
            rrUser User = (rrUser)Session["User"];

            if (ForumThread.CreatedBy != User.UserId)
            {
                ViewData["ErrorMessage"] = "Нельзя редактировать сообщения созданные другими пользователями";
                return View("Error");
            }

            string Title = User.UserId != ForumThread.CreatedBy ? ForumThread.Title : Model.Title;
            string Description = Uri.UnescapeDataString(Model.Description);


            rrForumThreadDB.UpdateForumThread(ForumId: rrForumThreadDB.ForumId,
                UserId: User.UserId, ThreadId: ForumThread.ThreadId, Title: Model.Title,
                Description: Description, FileName: FileName);

            return RedirectToAction("Index");
        }
        public ActionResult GetServerDate()
        {
            return Content(DateTime.Now.ToString());
        }
        // GET: Forum/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            rrUser User = (rrUser)Session["User"];

            LinkedList<rrForumThread> llThread = rrForumThreadDB.GetForumThread(
                ForumId: rrForumThreadDB.ForumId,
                ThreadId: id);
            if (llThread.Count == 0)
            {
                ViewData["ErrorMessage"] = string.Format("Тема не найдена");
                return View("Error");
            }

            rrForumThread ForumThread = llThread.First.Value;

            return View(new ForumThread(ForumThread));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteIt(int id)
        {
            rrUser User = (rrUser)Session["User"];

            LinkedList<rrForumThread> llThread = rrForumThreadDB.GetForumThread(
                ForumId: rrForumThreadDB.ForumId,
                ThreadId: id);
            if (llThread.Count == 0)
            {
                ViewData["ErrorMessage"] = string.Format("Тема не найдена");
                return View("Error");
            }

            rrForumThread ForumThread = llThread.First.Value;

            if (ForumThread.CreatedBy != User.UserId)
            {
                ViewData["ErrorMessage"] = "Нельзя удалить сообщение созданное другим пользователем";
                return View("Error");
            }

            if (!string.IsNullOrEmpty(ForumThread.FileName))
            {
                string FilePath = Server.MapPath(string.Format("~/Files/{0}", ForumThread.FileName));
                System.IO.File.Delete(FilePath);
            }
            rrForumThreadDB.DeleteForumThread(
                ForumId: rrForumThreadDB.ForumId,
                ThreadId: id);

            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public ActionResult Reply(int id, string ReplyText)
        {
            rrUser User = (rrUser)Session["User"];

            if (User == null)
            {
                ViewData["ErrorMessage"] = string.Format("Вы не зарегистрированы");
                return View("Error");
            }
            LinkedList<rrForumThread> llThread = rrForumThreadDB.GetForumThread(
                ForumId: rrForumThreadDB.ForumId,
                ThreadId: id);
            if (llThread.Count == 0)
            {
                ViewData["ErrorMessage"] = string.Format("Тема не найдена");
                return View("Error");
            }

            rrForumThread ForumThread = llThread.First.Value;

            rrForumThreadDB.InsertForumThread(ForumId: rrForumThreadDB.ForumId, UserId: User.UserId,
                Title: ForumThread.Title, ThreadParentId: ForumThread.ThreadId, 
                Description: Uri.UnescapeDataString(ReplyText), FileName: null);

            LinkedList<rrForumThread> llThreads = rrForumThreadDB.GetForumThread(ForumId: rrForumThreadDB.ForumId,
                StartThreadId: id);

            ForumThread[] arrModel = new ForumThread[llThreads.Count];

            int i = 0;

            foreach (rrForumThread Thread in llThreads)
            {
                arrModel[i] = new ForumThread(Thread);
                i++;
            }

            return View("Details", arrModel);
        }
    }
}