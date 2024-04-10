using EntityCa;
using Microsoft.Win32;
using Newtonsoft.Json;
using ShopCa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminPresentation.Controllers
{
    [Authorize]
    public class adminController : Controller
    {
        // GET: admin
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult Brand()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }

        // ######## CATEGORY ######## //
        #region CATEGORY

        [HttpGet]
        public JsonResult ListCategory()
        {
            List<Category> oList = new List<Category>();
            oList = new CN_Category().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveCategory(Category obje)
        {
            object result;
            string message = string.Empty;
            if (obje.IdCategory == 0)
            {
                result = new CN_Category().Register(obje, out message);
            }
            else
            {
                result = new CN_Category().Edit(obje, out message);

            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCategory(int id)
        {
            bool res = false;
            string message = string.Empty;

            res = new CN_Category().Delete(id, out message);
            return Json(new { result = res, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // ######## BRAND ######## //
        #region BRAND
        [HttpGet]
        public JsonResult ListBrand()
        {
            List<Brand> oList = new List<Brand>();
            oList = new CN_Brand().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveBrand(Brand obje)
        {
            object result;
            string message = string.Empty;
            if (obje.IdBrand == 0)
            {
                result = new CN_Brand().Register(obje, out message);
            }
            else
            {
                result = new CN_Brand().Edit(obje, out message);

            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Deletebrand(int id)
        {
            bool res = false;
            string message = string.Empty;

            res = new CN_Brand().Delete(id, out message);
            return Json(new { result = res, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        // ######## PRODUCT ######## //
        #region PRODUCT
        [HttpGet]
        public JsonResult ListProduct()
        {
            List<Product> oList = new List<Product>();
            oList = new CN_Product().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveProduct(string obje, HttpPostedFileBase fileImag)
        {
   
            string message = string.Empty;
            bool operation_successful = true;
            bool save_imag_successful = true;

            Product oProduct = new Product();
            oProduct = JsonConvert.DeserializeObject<Product>(obje);
            decimal price;

            if (decimal.TryParse(oProduct.PriceText,NumberStyles.AllowDecimalPoint, new CultureInfo("en"),out price))
            {
                oProduct.Price = price;
            }else
            {
                return Json(new { operation_successful = false, message = "the price format should be: ##.##" }, JsonRequestBehavior.AllowGet);
            }

            if (oProduct.IdProduct == 0)
            {
                int idProductGenerated = new CN_Product().Register(oProduct, out message);
                if(idProductGenerated != 0)
                {
                    oProduct.IdProduct = idProductGenerated;
                }
                else
                {
                    operation_successful = false;
                }
            }
            else
            {
                operation_successful = new CN_Product().Edit(oProduct, out message);

            }

            if(operation_successful)
            {
                if(fileImag != null)
                {
                    string path_save = ConfigurationManager.AppSettings["PhotoServer"];
                    string extension = Path.GetExtension(fileImag.FileName);
                    string name_imag = string.Concat(oProduct.IdProduct.ToString(), extension);

                    try
                    {
                        fileImag.SaveAs(Path.Combine(path_save, name_imag));
                    }
                    catch(Exception ex) 
                    {
                        string msg = ex.Message;
                        save_imag_successful = false;
                    }
                    if (save_imag_successful)
                    {
                        oProduct.RImage = path_save;
                        oProduct.NameImage = name_imag;
                        bool rspta = new CN_Product().SaveImagDetails(oProduct, out message);
                    } else
                    {
                        message = "The product was saved, but there were issues with the image.";
                    }
                }
            }

            return Json(new { operation_successful = operation_successful,idGenerated = oProduct.IdProduct, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductImg(int id)
        {
            bool conversion;
            Product oproduct = new CN_Product().Listar().Where(p => p.IdProduct == id).FirstOrDefault();
            string textBase64 = CN_Resources.ConvertBase64(Path.Combine(oproduct.RImage, oproduct.NameImage),out conversion);
            return Json(new
            {
                conversion = conversion,
                textBase64 = textBase64,
                extension = Path.GetExtension(oproduct.NameImage)
            },
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProduct(int id)
        {
            bool res = false;
            string message = string.Empty;

            res = new CN_Product().Delete(id, out message);
            return Json(new { result = res, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}