using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RifleRange.DAL;
using System.Web;

namespace RifleRange.Models
{
    public class NewsModel
    {
        [DisplayName("Номер")]
        public int NewsId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Заголовок")]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Текст")]
        [StringLength(4096)]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [DisplayName("Файл")]
        public HttpPostedFileBase File
        {
            get;
            set;
        }
        [StringLength(256)]
        [DisplayName("Файл")]
        public string FileName { get; set; }

        [DisplayName("Удалить")]
        public bool DeleteFile { get; set; }

        [DisplayName("Дата")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Изменено")]
        public DateTime? LastUpdate { get; set; }

        public NewsModel(rrNews News)
        {
            NewsId = News.NewsId;
            Title = News.Title;
            Body = Uri.EscapeDataString(News.Body);
            FileName = News.FileName;
            CreateDate = News.CreateDate;
            LastUpdate = News.LastUpdate;
        }
        public NewsModel()
        {

        }
    }
}