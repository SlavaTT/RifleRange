using System.Collections.Generic;
using System.Web.Mvc;
using RifleRange.DAL;
using System.Web.Security;
using RifleRange.Models;
using System;
using System.Web;

namespace RifleRange.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel Model)
        {
            if (ModelState.IsValid)
            {
                LinkedList<rrUser> llUser = rrUserDB.GetUser(LoginName: Model.LoginName, Password: Model.Password);

                if (llUser.Count == 1)
                {
                    rrUser User = llUser.First.Value;

                    CurrentUser = User;

                    var AuthTicket = new FormsAuthenticationTicket(
                        1,
                        User.UserId.ToString(),               
                        DateTime.Now,                
                        DateTime.Now.AddMinutes(20), 
                        false,                    
                        User.Role
                        );

                    string EncryptedTicket = FormsAuthentication.Encrypt(AuthTicket);

                    var AuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);
                    HttpContext.Response.Cookies.Add(AuthCookie);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
            }

            return View();
        }
        [Authorize]
        public ActionResult ProfileEdit()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (CurrentUser != null) View(new RegisterModel(CurrentUser));
            }
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileEdit(RegisterModel Model)
        {
            if (ModelState.IsValid)
            {
                if (!rrUserDB.CheckLoginExists(LoginName: Model.LoginName, ExceptUserId: CurrentUser.UserId))
                {
                    // обновляем пользователя
                    rrUserDB.UpdateUser(UserId: CurrentUser.UserId, FirstName: Model.FirstName,
                        LastName: Model.LastName, Email: Model.Email, LoginName: Model.LoginName, 
                        Password: Model.Password);

                    FormsAuthentication.SignOut();

                    FormsAuthentication.SetAuthCookie(CurrentUser.UserId.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }

            return View(Model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel Model)
        {
            if (ModelState.IsValid)
            {
                if (!rrUserDB.CheckLoginExists(LoginName: Model.LoginName))
                {
                    // создаем нового пользователя
                    int NewUserId = rrUserDB.InsertUser(FirstName: Model.FirstName, LastName: Model.LastName,
                        Email: Model.Email, LoginName: Model.LoginName, Password: Model.Password);

                    LinkedList<rrUser> llUser = rrUserDB.GetUser(UserId: NewUserId);
                    rrUser User = llUser.First.Value;

                    CurrentUser = User;

                    var AuthTicket = new FormsAuthenticationTicket(
                        1,
                        User.UserId.ToString(),
                        DateTime.Now,
                        DateTime.Now.AddMinutes(20),
                        false,
                        User.Role
                        );

                    string EncryptedTicket = FormsAuthentication.Encrypt(AuthTicket);

                    var AuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);
                    HttpContext.Response.Cookies.Add(AuthCookie);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }

            return View(Model);
        }
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}