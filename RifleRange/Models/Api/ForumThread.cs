using System;
using RifleRange.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RifleRange.Api.Models
{
    public class ForumThreadReply
    {
        [Required]
        public int ThreadParentId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CreatedBy { get; set; }
    }
    public class ForumThread
    {
        public string URL { get; set; }

        public int ForumThreadId { get; set; }

        public bool IsTopThread { get; private set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string CreateDateString { get { return CreateDate.ToString("dd/MM/yyyy hh:mm:ss"); } }

        public int CreatedBy;
        public int? LastUpdatedBy;

        public string CreatedByName { get; set; }

        public string LastUpdatedByName { get; set; }

        public string FileName { get; set; }

        public ForumThread[] Children { get; set; }

        public ForumThread(rrForumThread ForumThread)
        {
            ForumThreadId = ForumThread.ThreadId;
            Title = ForumThread.Title;
            Description = ForumThread.Description;
            FileName = ForumThread.FileName;
            CreatedBy = ForumThread.CreatedBy;
            LastUpdatedBy = ForumThread.LastUpdatedBy;
            CreateDate = ForumThread.CreateDate;
            LastUpdate = ForumThread.LastUpdate;
            CreatedByName = ForumThread.CreatedByName;
            LastUpdatedByName = ForumThread.LastUpdatedByName;
            IsTopThread = (ForumThread.ThreadParentId == null);
        }
        public ForumThread(LinkedList<rrForumThread> List, LinkedListNode<rrForumThread> ForumThreadNode)
        {
            rrForumThread Thread = ForumThreadNode.Value;

            ForumThreadId = Thread.ThreadId;
            Title = Thread.Title;
            Description = Thread.Description;
            FileName = Thread.FileName;
            CreatedBy = Thread.CreatedBy;
            LastUpdatedBy = Thread.LastUpdatedBy;
            CreateDate = Thread.CreateDate;
            LastUpdate = Thread.LastUpdate;
            CreatedByName = Thread.CreatedByName;
            LastUpdatedByName = Thread.LastUpdatedByName;
            IsTopThread = (Thread.ThreadParentId == null);

            LinkedList<rrForumThread> llChildThread = FindThreadByParentId(List, ForumThreadId);

            if (llChildThread.Count > 0)
            {
                Children = new ForumThread[llChildThread.Count];

                int i = 0;

                LinkedListNode<rrForumThread> Node = llChildThread.First;

                while (Node != null)
                {
                    ForumThread Child = new ForumThread(List, Node);

                    Children[i] = Child;

                    i++;
                    Node = Node.Next;
                }
            }
        }
        private LinkedList<rrForumThread> FindThreadByParentId(LinkedList<rrForumThread> ThreadList, int ThreadId)
        {
            LinkedList<rrForumThread> Children = new LinkedList<rrForumThread>();

            LinkedListNode<rrForumThread> Node = ThreadList.First;

            while(Node != null)
            {
                rrForumThread Thread = Node.Value;

                if (Thread.ThreadParentId == ThreadId) Children.AddLast(Thread);

                Node = Node.Next;
            }

            return Children;
        }
    }
}