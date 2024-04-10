using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using EntityCa;
using ShopCa;
using System.Web.Security;

namespace adminPresentation.Controllers
{
    public class AccessController : Controller
    {
        // GET: Acces
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            User_Information oUser = new User_Information();
            oUser = new CN_Users().Listar().Where(u => u.Email == email && u.Password == CN_Resources.ConvertSha256(password)).FirstOrDefault();

            if (oUser == null)
            {
                ViewBag.Error = "Incorrect email or password";
                return View();

            }else
            {

                if (oUser.Reset)
                {
                    TempData["IdUser"] = oUser.IdUser;
                    return RedirectToAction("ChangePassword");
                }

                FormsAuthentication.SetAuthCookie(oUser.Email,false);

                ViewBag.Error = null;

                return RedirectToAction("Index","Home");
            }
            
        }
        [HttpPost]
        public ActionResult ChangePassword(string iduser, string currentpassword,string newpassword, string confirmpassword)
        {

            User_Information oUser = new User_Information();

            oUser = new CN_Users().Listar().Where(u => u.IdUser == int.Parse(iduser)).FirstOrDefault();
            if(oUser.Password != CN_Resources.ConvertSha256(currentpassword))
            {
                TempData["IdUser"] = iduser;
                ViewData["vpassword"] = "";
                ViewBag.Error = "The current password is incorrect";
                return View();
            }  
            else if (newpassword != confirmpassword)
            {
                TempData["IdUser"] = iduser;
                ViewData["vpassword"] = currentpassword;
                ViewBag.Error = "The passwords do not match";
                return View();
            }
            ViewData["vpassword"] = "";
            newpassword = CN_Resources.ConvertSha256(newpassword);
            string message = string.Empty;
            bool res = new CN_Users().ChangePassword(int.Parse(iduser),newpassword, out message);
            if(res)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUser"] = iduser;
                ViewBag.Error = message;
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult Reset(string email)
        {
            User_Information oUser = new User_Information();
            oUser = new CN_Users().Listar().Where(item => item.Email == email).FirstOrDefault();
         
            if (oUser == null)
            {
                ViewBag.Error = "No user found related to that email";
                return View();
            }
            string message = string.Empty;
            bool res = new CN_Users().ResetPassword(oUser.IdUser,email, out message);

            if(res)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Access");
            }
            else
            {
                ViewBag.Error = message;
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Access");
        }
    }
}