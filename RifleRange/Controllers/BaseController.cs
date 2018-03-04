using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Web.Mvc;
using RifleRange.DAL;

namespace RifleRange.Controllers
{
    public abstract class BaseController : Controller
    {
        protected rrUser CurrentUser
        {
            get { return (rrUser)Session["User"]; }
            set { Session["User"] = value; }
        }
        protected override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Session == null || CurrentUser == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    int UserId = int.Parse(User.Identity.Name);

                    LinkedList<rrUser> llUser = rrUserDB.GetUser(UserId);

                    if (llUser.Count == 1)
                    {
                        CurrentUser = llUser.First.Value;
                    }
                    else
                    {
                        throw new ApplicationException($"User with Id {UserId} was not found");
                    }
                }
            }
        }
    }
}