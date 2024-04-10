using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopCa;
using System.Web.Security;

namespace userPresentation.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Reset()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer objeto)
        {
            int result;
            string message = string.Empty;
            ViewData["Name"] = string.IsNullOrEmpty(objeto.Name) ? "" : objeto.Name;
            ViewData["LastName"] = string.IsNullOrEmpty(objeto.LastName) ? "" : objeto.LastName;
            ViewData["Email"] = string.IsNullOrEmpty(objeto.Email) ? "" : objeto.Email;

            if(objeto.Password != objeto.ConfirmPassword)
            {
                ViewBag.Error = "The passwords do not match.";
                return View();
            }
            result = new CN_Customer().Register(objeto, out message);
            if(result>0)
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
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            Customer oCustomer = null;
            oCustomer = new CN_Customer().Listar().Where(item => item.Email == email && item.Password == CN_Resources.ConvertSha256(password)).FirstOrDefault();
            if (oCustomer == null)
            {
                ViewBag.Error = "Incorrect email or password";
                return View();
            }
            else
            {
                if (oCustomer.Reset)
                {
                    TempData["IdCustomer"] = oCustomer.IdCustomer;
                    return RedirectToAction("ChangePassword", "Access");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCustomer.Email, false);
                    Session["Customer"] = oCustomer;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Shop");

                }
            }      
        }
        [HttpPost]
        public ActionResult Reset(string email)
        {
            Customer oCustomer = new Customer();
            oCustomer = new CN_Customer().Listar().Where(item => item.Email == email).FirstOrDefault();

            if (oCustomer == null)
            {
                ViewBag.Error = "No customer found related to that email";
                return View();
            }
            string message = string.Empty;
            bool res = new CN_Customer().ResetPassword(oCustomer.IdCustomer, email, out message);

            if (res)
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

        [HttpPost]
        public ActionResult ChangePassword(string idcustomer, string currentpassword, string newpassword, string confirmpassword)
        {
            Customer oCustomer = new Customer();

            oCustomer = new CN_Customer().Listar().Where(u => u.IdCustomer == int.Parse(idcustomer)).FirstOrDefault();
            if (oCustomer.Password != CN_Resources.ConvertSha256(currentpassword))
            {
                TempData["IdCustomer"] = idcustomer;
                ViewData["vpassword"] = "";
                ViewBag.Error = "The current password is incorrect";
                return View();
            }
            else if (newpassword != confirmpassword)
            {
                TempData["IdCustomer"] = idcustomer;
                ViewData["vpassword"] = currentpassword;
                ViewBag.Error = "The passwords do not match";
                return View();
            }
            ViewData["vpassword"] = "";
            newpassword = CN_Resources.ConvertSha256(newpassword);
            string message = string.Empty;
            bool res = new CN_Customer().ChangePassword(int.Parse(idcustomer), newpassword, out message);
            if (res)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCustomer"] = idcustomer;
                ViewBag.Error = message;
                return View();
            }
        }


        public ActionResult LogOut()
        {
            Session["Customer"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Access");
        }
    }

}