using System;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RifleRange.DAL;

namespace RifleRange.Models
{
    public class ForumThread
    {
        [DisplayName("Номер")]
        public int ForumThreadId { get; set; }

        public bool IsTopThread { get; private set; }
        public int? ThreadParentId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Тема")]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }

        [DisplayName("Сообщение")]
        [StringLength(4000)]
        public string Description { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime? LastUpdate { get; set; }

        public int CreatedBy;
        public int? LastUpdatedBy;

        [DisplayName("Кем создано")]
        public string CreatedByName { get; set; }
        [DisplayName("Кем изменено")]
        public string LastUpdatedByName { get; set; }

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

        public ForumThread()
        {

        }
        public ForumThread(rrForumThread ForumThread)
        {
            ForumThreadId = ForumThread.ThreadId;
            ThreadParentId = ForumThread.ThreadParentId;
            Title = ForumThread.Title;
            Description = ForumThread.Description;
            Description = Uri.EscapeDataString(ForumThread.Description);
            FileName = ForumThread.FileName;
            CreatedBy = ForumThread.CreatedBy;
            LastUpdatedBy = ForumThread.LastUpdatedBy;
            CreateDate = ForumThread.CreateDate;
            LastUpdate = ForumThread.LastUpdate;
            CreatedByName = ForumThread.CreatedByName;
            LastUpdatedByName = ForumThread.LastUpdatedByName;
            IsTopThread = (ForumThread.ThreadParentId == null);
        }
    }
}