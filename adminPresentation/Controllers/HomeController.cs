using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using EntityCa;
using ShopCa;

namespace adminPresentation.Controllers
{

    [Authorize]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListUsers()
        {
            List<User_Information> oList = new List<User_Information>();
            oList = new CN_Users().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUser(User_Information obje)
        {
            object result;
            string message = string.Empty;
            if (obje.IdUser == 0)
            {
                result = new CN_Users().Register(obje, out message);
            }
            else
            {
                result = new CN_Users().Edit(obje, out message);

            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteUser(int id)
        {
            bool res = false;
            string message = string.Empty;

            res = new CN_Users().Delete(id, out message);
            return Json(new { result = res, message = message }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ListReport(string startdate, string enddate, string idtransaction)
        {
            List<Report> oList = new List<Report>();

            oList = new CN_Report().Sales(startdate,enddate, idtransaction);

            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult VistDashBoard()
        {
            DashBoard objeto = new CN_Report().ShowDashBoard();
            return Json(new { result = objeto }, JsonRequestBehavior.AllowGet);
        }

    }
}