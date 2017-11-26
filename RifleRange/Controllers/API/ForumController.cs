using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using RifleRange.DAL;
using RifleRange.Api.Models;


namespace RifleRange.Controllers.API
{
    public class ForumController : BaseController
    {
        const int ForumId = 2;

        [HttpGet]
        public HttpResponseMessage GetForumThread(int Id)
        {
            HttpResponseMessage Response = null;

            try
            {
                string JSON = "[\r\n " + GetThreadJSON(Id) + " \r\n]";
                Response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content: JSON, encoding: UTF8Encoding.Default, mediaType: "application/json"),
                };
            }
            catch (Exception ex)
            {
                Response = Request.CreateErrorResponse(statusCode: HttpStatusCode.BadRequest, exception: ex);
            }
            return Response;
        }
        [Authorize]
        [HttpPost]
        [Route("api/Forum")]
        public IHttpActionResult Reply(ForumThreadReply Reply)
        {
            IHttpActionResult Response = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    Response = BadRequest(ModelState);
                }
                else
                {
                    int ForumThreadId = rrForumThreadDB.InsertForumThreadReply(
                        UserId: Reply.CreatedBy,
                        ThreadParentId: Reply.ThreadParentId,
                        Description: Uri.UnescapeDataString(Reply.Description));

                    string NewUrl = string.Format("api/Forum/{0}", ForumThreadId);
                    Response = Created(NewUrl, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Response = BadRequest(ex.Message);
            }
            return Response;
        }
        [HttpPut]
        public IHttpActionResult Edit(int Id, [FromBody]string Description)
        {
            IHttpActionResult Response = null;

            try
            {
                LinkedList<rrForumThread> lstThread = rrForumThreadDB.GetForumThread(ForumId: ForumId, ThreadId: Id);
                if (lstThread.Count == 0)
                    Response = NotFound();
                else
                {
                    rrForumThread Thread = lstThread.First.Value;

                    rrForumThreadDB.UpdateForumThread(ForumId: ForumId, ThreadId: Id,
                        UserId: Thread.CreatedBy, Title: Thread.Title, 
                        Description: Uri.UnescapeDataString(Description),
                        FileName: Thread.FileName);

                    Response = Ok();
                }
            }
            catch (Exception ex)
            {
                Response = BadRequest(ex.Message);
            }
            return Response;
        }
        [HttpDelete]
        public IHttpActionResult Delete(int Id)
        {
            IHttpActionResult Response = null;

            try
            {
                LinkedList<rrForumThread> lstThread = rrForumThreadDB.GetForumThread(ForumId: ForumId, ThreadId: Id);
                if (lstThread.Count == 0)
                    Response = NotFound();
                else
                {
                    rrForumThread Thread = lstThread.First.Value;

                    rrForumThreadDB.DeleteForumThread(ForumId: ForumId, ThreadId: Id);

                    Response = Ok();
                }
            }
            catch (Exception ex)
            {
                Response = BadRequest(ex.Message);
            }
            return Response;
        }


        private string GetThreadJSON(int StartThreadId)
        {
            LinkedList<rrForumThread> llThreads = rrForumThreadDB.GetForumThread(ForumId: ForumId,
                StartThreadId: StartThreadId, IncludeStartThread: true);


            return OutputThreadJSON(llThreads, StartThreadId);
        }

        private string OutputThreadJSON(LinkedList<rrForumThread> ThreadList, int ThreadId)
        {
            LinkedListNode<rrForumThread> Node = ThreadList.First;

            while (Node != null)
            {
                if (Node.Value.ThreadId == ThreadId) break;

                Node = Node.Next;
            }
            string JsonText = string.Empty;

            if (Node != null)
            {
                ForumThread Thread = new ForumThread(ThreadList, Node);

                JsonText = JsonConvert.SerializeObject(Thread, Newtonsoft.Json.Formatting.Indented);
            }
            return JsonText;
        }
        private rrForumThread FindThreadById(LinkedList<rrForumThread> ThreadList, int ThreadId)
        {
            rrForumThread Result = null;

            LinkedListNode<rrForumThread> Node = ThreadList.First;

            while (Node != null)
            {
                rrForumThread Thread = Node.Value;
                if (Thread.ThreadId == ThreadId)
                {
                    Result = Thread;
                    break;
                }

                Node = Node.Next;
            } 

            return Result;
        }
        private LinkedList<rrForumThread> FindThreadByParentId(LinkedList<rrForumThread> ThreadList, 
            int ThreadId)
        {
            LinkedList<rrForumThread> Result = new LinkedList<rrForumThread>();

            foreach (rrForumThread Thread in ThreadList)
            {
                if (Thread.ThreadParentId == ThreadId)
                    Result.AddLast(Thread);
            }

            return Result;
        }

    }
}
