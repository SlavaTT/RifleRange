using RifleRange.DAL;
using RifleRange.Api.Models;
using System.Web.Http.Routing;
using System.Net.Http;
using System.Collections.Generic;

namespace RifleRange.Api
{
    public class ModelFactory
    {
        public ForumThread Create(rrForumThread Thread)
        {
            ForumThread Model = new ForumThread(Thread);
//            Model.URL = UrlHelper.Link("Forum", new { id = Thread.ThreadId.ToString() });

            return Model;
        }
        public ForumThread[] Create(LinkedList<rrForumThread> Threads)
        {
            ForumThread[] Result = null;

            if (Threads != null)
            {
                Result = new ForumThread[Threads.Count];

                int i = 0;

                LinkedListNode<rrForumThread> Node = Threads.First;

                while (Node != null)
                {
                    Result[i] = Create(Node.Value);

                    Node = Node.Next;
                    i++;
                }

            }

            return Result;
        }
    }
}