using System;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RifleRange.DAL;

namespace RifleRange.Models
{
    public class ImageModel
    {
        [DisplayName("Номер")]
        public int ImageId { get; set; }

        [Required]
        [DisplayName("Альбом")]
        public string PhotoAlbumId { get; set; }

        public string FileName { get; set; }

        [DisplayName("Картинка")]
        [Required]
        public HttpPostedFileBase File
        {
            get;
            set;
        }

        [DisplayName("Описание")]
        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Дата")]
        public DateTime CreateDate { get; set; }

        public ImageModel()
        {

        }
        public ImageModel(rrImage Image)
        {
            PhotoAlbumId = Image.PhotoAlbumId.ToString();
            ImageId = Image.ImageId;
            FileName = Image.FileName;
            Description = Image.Description;
            CreateDate = Image.CreateDate;
        }
    }
}