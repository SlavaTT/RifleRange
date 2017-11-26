using System.Collections.Generic;
using System.Web.Mvc;
using RifleRange.DAL;
using RifleRange.Models;


namespace RifleRange.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Restaurant()
        {
            rrPhotoAlbum Album = rrPhotoAlbumDB.GetPhotoAlbum((int)StandardPhotoAlbum.Restaurant);
            ViewBag.Album = Album;

            LinkedList<rrImage> List = rrImageDB.GetImage(PhotoAlbumId: Album.AlbumId);

            ImageModel[] Images = new ImageModel[List.Count];

            int i = 0;

            foreach (rrImage Image in List)
            {
                Images[i] = new ImageModel(Image);
                i++;
            }

            return View(Images);
        }
        public ActionResult Dock()
        {
            rrPhotoAlbum Album = rrPhotoAlbumDB.GetPhotoAlbum((int)StandardPhotoAlbum.Dock);
            ViewBag.Album = Album;

            LinkedList<rrImage> List = rrImageDB.GetImage(PhotoAlbumId: Album.AlbumId);

            ImageModel[] Images = new ImageModel[List.Count];

            int i = 0;

            foreach (rrImage Image in List)
            {
                Images[i] = new ImageModel(Image);
                i++;
            }

            return View(Images);
        }
        public ActionResult Horse()
        {
            rrPhotoAlbum Album = rrPhotoAlbumDB.GetPhotoAlbum((int)StandardPhotoAlbum.Hourse);
            ViewBag.Album = Album;

            LinkedList<rrImage> List = rrImageDB.GetImage(PhotoAlbumId: Album.AlbumId);

            ImageModel[] Images = new ImageModel[List.Count];

            int i = 0;

            foreach (rrImage Image in List)
            {
                Images[i] = new ImageModel(Image);
                i++;
            }

            return View(Images);
        }
    }
}
