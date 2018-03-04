using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using RifleRange.DAL;
using RifleRange.Models;
using System.Web.Helpers;

namespace RifleRange.Controllers
{
    public class ImageController : BaseController
    {
        public ActionResult PhotoByAlbum()
        {
            LinkedList<rrPhotoAlbum> AlbumList = rrPhotoAlbumDB.GetPhotoAlbum();
            LinkedList<rrImage> ImageList = rrImageDB.GetImage();

            ImageModel[][] Images = new ImageModel[AlbumList.Count][];

            int AlbumCounter = 0;

            LinkedList<ImageModel> AlbumImageList = new LinkedList<ImageModel>();

            foreach (rrPhotoAlbum Album in AlbumList)
            {

                foreach (rrImage Image in ImageList)
                {
                    if (Image.PhotoAlbumId == Album.AlbumId)
                    {
                        ImageModel Model = new ImageModel(Image);
                        AlbumImageList.AddLast(Model);
                    }
                }

                Images[AlbumCounter] = new ImageModel[AlbumImageList.Count];
                AlbumImageList.CopyTo(Images[AlbumCounter], 0);

                AlbumImageList.Clear();
                AlbumCounter++;
            }

            ViewBag.AlbumList = AlbumList;

            return View(Images);
        }

        public ActionResult Photo()
        {
            LinkedList<rrImage> ImageList = rrImageDB.GetImage();

            ImageModel[] Images = new ImageModel[ImageList.Count];

            int ImageCounter = 0;

            foreach (rrImage Image in ImageList)
            {
                ImageModel Model = new ImageModel(Image);
                Images[ImageCounter] = Model;

                ImageCounter++;
            }
            return View(Images);
        }

        // GET: Image/Create
        public ActionResult Create(int? Id)
        {         
            LinkedList<rrPhotoAlbum> AlbumList = rrPhotoAlbumDB.GetPhotoAlbum();
            SelectList Albums = new SelectList(items: AlbumList, 
                dataTextField: "Title", dataValueField: "AlbumId", selectedValue: Id);

            ViewBag.AlbumList = Albums;
            ViewBag.AlbumId = Id;

            return View();
        }
        // POST: Image/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImageModel Model)
        {
            LinkedList<rrPhotoAlbum> AlbumList = rrPhotoAlbumDB.GetPhotoAlbum();
            SelectList Albums = new SelectList(items: AlbumList,
                dataTextField: "Title", dataValueField: "AlbumId");

            ViewBag.AlbumList = Albums;

            if (!ModelState.IsValid) return View();

            if (Model.File.ContentLength > 0)
            {
                string FileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), Model.File.FileName);
                var FilePath = Server.MapPath(Path.Combine("~/Files/Image", FileName));
                Model.File.SaveAs(FilePath);

                WebImage Image = WebImage.GetImageFromRequest();
                string ThumbImagePath = Server.MapPath(Path.Combine("~/Files/Image/Thumb", FileName));
                Image.Resize(width: 256, height: 256, preserveAspectRatio: true);

                Image.Save(ThumbImagePath);

                rrImageDB.InsertImage(FileName: FileName, Description: Model.Description,
                    PhotoAlbumId: int.Parse(Model.PhotoAlbumId));
            }

            return RedirectToAction("EditAlbum", new { id = Model.PhotoAlbumId });
        }
        [Authorize]
        public ActionResult EditAlbum(int Id)
        {
            rrPhotoAlbum Album = rrPhotoAlbumDB.GetPhotoAlbum(Id);
            ViewBag.Album = Album;

            LinkedList<rrImage> List = rrImageDB.GetImage(PhotoAlbumId: Id);

            ImageModel[] Images = new ImageModel[List.Count];

            int i = 0;

            foreach(rrImage Image in List)
            {
                Images[i] = new ImageModel(Image);
                i++;
            }

            return View(Images);
        }
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int Id)
        {
            rrImage Image = rrImageDB.GetImage(Id);

            if (!string.IsNullOrEmpty(Image.FileName))
            {
                string FilePath = Server.MapPath(string.Format("~/Files/Image/{0}", Image.FileName));
                System.IO.File.Delete(FilePath);
                FilePath = Server.MapPath(string.Format("~/Files/Image/Thumb/{0}", Image.FileName));
                System.IO.File.Delete(FilePath);
            }
            rrImageDB.DeleteImage(Id);

            return RedirectToAction("Album", new { id = Image.PhotoAlbumId });
        }
    }
}