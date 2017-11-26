using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using RifleRange.DAL;
using RifleRange.Models;
using System;

namespace RifleRange.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            LinkedList<rrNews> llNews = rrNewsDB.GetNewsList();

            NewsModel[] ModelArray = new NewsModel[llNews.Count];

            int i = 0;
            foreach(rrNews News in llNews)
            {
                ModelArray[i] = new NewsModel(News);
                i++;
            }

            if (User.IsInRole("admin"))
                return View("EditIndex", ModelArray);
            else
                return View("Index", ModelArray);
        }

        // GET: News/Details/5
        public ActionResult Details(int id)
        {
            rrNews News = rrNewsDB.GetNews(id);
            NewsModel Model = new NewsModel(News);

            return View(Model);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsModel Model)
        {
            if (!ModelState.IsValid) return View();

            string FileName = null;
            if (Model.File != null)
            {
                FileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), Model.File.FileName);
                var FilePath = Server.MapPath(Path.Combine("~/Files", FileName));
                Model.File.SaveAs(FilePath);
            }
            string Body = Uri.UnescapeDataString(Model.Body);

            rrNewsDB.InsertNews(Title: Model.Title, Body: Body, FileName: FileName);
            return RedirectToAction("Index");
        }

        // GET: News/Edit/5
        public ActionResult Edit(int id)
        {
            rrNews News = rrNewsDB.GetNews(id);
            NewsModel Model = new NewsModel(News);

            return View(Model);
        }

        // POST: News/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, NewsModel Model)
        {
            if (!ModelState.IsValid) return View();

            rrNews News = rrNewsDB.GetNews(id);
            string FileName = News.FileName;

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
            string Body = Uri.UnescapeDataString(Model.Body);

            rrNewsDB.UpdateNews(NewsId: id, Title: Model.Title, Body: Body, FileName: FileName);

            return RedirectToAction("Index");
        }

        // GET: News/Delete/5
        public ActionResult Delete(int id)
        {
            rrNews News = rrNewsDB.GetNews(id);
            NewsModel Model = new NewsModel(News);

            return View(Model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteIt(int id)
        {
            rrNews News = rrNewsDB.GetNews(id);
            if (!string.IsNullOrEmpty(News.FileName))
            {
                string FilePath = Server.MapPath(string.Format("~/Files/{0}", News.FileName));
                System.IO.File.Delete(FilePath);
            }
            rrNewsDB.DeleteNews(NewsId: id);


            return RedirectToAction("Index");
        }
    }
}
