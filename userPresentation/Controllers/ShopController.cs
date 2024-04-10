using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityCa;
using ShopCa;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using EntityCa.Paypal;
using userPresentation.Filter;

namespace userPresentation.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductDetail(int idproduct = 0)
        {
            Product oProduct = new Product();
            bool conversion;
            oProduct = new CN_Product().Listar().Where(p => p.IdProduct == idproduct).FirstOrDefault();
            if(oProduct != null)
            {
                oProduct.Base64 = CN_Resources.ConvertBase64(Path.Combine(oProduct.RImage,oProduct.NameImage), out conversion);
                oProduct.Extension =Path.GetExtension(oProduct.NameImage);
            }
            return View(oProduct);
        }


        [HttpGet]
        public JsonResult listCategories()
        {
            List<Category> list = new List<Category>();
            list = new CN_Category().Listar();
            return Json(new {data=list}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Listbrandsbycategory(int idcategory)
        {
            List<Brand> list = new List<Brand>();
            list = new CN_Brand().Listbrandsbycategory(idcategory);
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProduct(int idcategory, int idbrand)
        {
            List<Product> list = new List<Product> ();

            bool conversion;

            list = new CN_Product().Listar().Select(p => new Product()
            {
                IdProduct = p.IdProduct,
                Name = p.Name,
                Description = p.Description,
                oBrand = p.oBrand,
                oCategory = p.oCategory,
                Price = p.Price,
                Stock = p.Stock,
                RImage = p.RImage,
                Base64 = CN_Resources.ConvertBase64(Path.Combine(p.RImage,p.NameImage),out conversion),
                Extension = Path.GetExtension(p.NameImage),
                Active = p.Active
            }).Where(p =>
                p.oCategory.IdCategory == (idcategory==0 ? p.oCategory.IdCategory : idcategory) &&
                p.oBrand.IdBrand == (idbrand == 0 ? p.oBrand.IdBrand : idbrand) &&
                p.Stock > 0 && p.Active == true 
            ).ToList();

            var jsonresult = Json(new { data = list }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;

        }

        [HttpPost]
        public JsonResult AddCart(int idproduct)
        {
            int idcustomer = ((Customer)Session["Customer"]).IdCustomer;
            bool exist = new CN_Cart().ExistCart(idcustomer, idproduct);
            bool res = false;
            string message = string.Empty;
            if (exist)
            {
                message = "Product already in cart";
            }
            else
            {
                res = new CN_Cart().cartOperation(idcustomer, idproduct,true, out message);

            }
            return Json(new { res = res, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TotalCart()
        {
            int idcustomer = ((Customer)Session["Customer"]).IdCustomer;
            int amount = new CN_Cart().TotalCart(idcustomer);
            return Json(new { amount = amount }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductCart()
        {
            int idcustomer = ((Customer)Session["Customer"]).IdCustomer;
            List<Cart> oList = new List<Cart>();
            bool conversion;
            oList = new CN_Cart().ListarProduct(idcustomer).Select(oc => new Cart()
            {
                oProduct = new Product()
                {
                    IdProduct = oc.oProduct.IdProduct,
                    Name = oc.oProduct.Name,
                    oBrand = oc.oProduct.oBrand,
                    Price = oc.oProduct.Price,
                    RImage = oc.oProduct.RImage,
                    Base64 = CN_Resources.ConvertBase64(Path.Combine(oc.oProduct.RImage,oc.oProduct.NameImage),out conversion),
                    Extension = Path.GetExtension(oc.oProduct.NameImage)
                },
                Amount = oc.Amount
            }).ToList();
            return Json(new {data=oList}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult cartOperation(int idproduct, bool sum)
        {
            int idcustomer = ((Customer)Session["Customer"]).IdCustomer;
            bool res = false;
            string message = string.Empty;
            res = new CN_Cart().cartOperation(idcustomer, idproduct, true, out message);

            return Json(new { res = res, message = message }, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public JsonResult DeleteCart(int idproduct)
        {
            int idcustomer = ((Customer)Session["Customer"]).IdCustomer;
            bool res = false;
            string message = string.Empty;
            res = new CN_Cart().DeleteCart(idcustomer, idproduct);

            return Json(new { res = res, message = message }, JsonRequestBehavior.AllowGet);

        }
        [ValidarSessionAttribure]
        [Authorize]
        public ActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcessPayment(List<Cart> oListCart, Sales oSales)
        {
            decimal total = 0;
            DataTable sales_details = new DataTable();
            sales_details.Locale = new CultureInfo("en");
            sales_details.Columns.Add("IdProduct", typeof(string));
            sales_details.Columns.Add("Amount", typeof(int));
            sales_details.Columns.Add("Total", typeof(decimal));

            List<Item> oListItem = new List<Item>(); 

            foreach (Cart oCart in oListCart)
            {
                decimal subtotal = Convert.ToDecimal(oCart.Amount.ToString()) * oCart.oProduct.Price;
                total += subtotal;

                oListItem.Add(new Item()
                {
                    name = oCart.oProduct.Name,
                    quantity = oCart.Amount.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = oCart.oProduct.Price.ToString("G",new CultureInfo("en"))
                    }
                });

                sales_details.Rows.Add(new object[]
                {
                    oCart.oProduct.IdProduct,
                    oCart.Amount,
                    subtotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("en")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("en")),

                        }
                    }
                },
                description = "Purchase of item from my store",
                items = oListItem
            };

            Checkout_Order oCheckOutOrder = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit> { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "ShopOnline",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "http://localhost:59230/Shop/PaymentCompleted",
                    cancel_url = "http://localhost:59230/Shop/Cart"
                }
            };

            oSales.OrderTotal = total;
            oSales.IdCustomer = ((Customer)Session["Customer"]).IdCustomer;

            TempData["Sale"] = oSales;
            TempData["SalesDetails"] = sales_details;

            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            response_paypal = await opaypal.CreateRequest(oCheckOutOrder);

            return Json( response_paypal, JsonRequestBehavior.AllowGet);

        }
        [ValidarSessionAttribure]
        [Authorize]
        public async Task<ActionResult> PaymentCompleted()
        {
            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
            
            response_paypal = await opaypal.ApprovePayment(token);

            ViewData["Status"] = response_paypal.Status;

            if(response_paypal.Status)
            {
                Sales oSale = (Sales)TempData["Sale"];
                DataTable sales_details = (DataTable)TempData["SalesDetails"];

                oSale.IdTransaction = response_paypal.Response.purchase_units[0].payments.captures[0].id;
                string message = string.Empty;
                bool res = new CN_Sale().Register(oSale, sales_details,out message);

                ViewData["IdTransaction"]= oSale.IdTransaction;
            }
            return View();
        }
        [ValidarSessionAttribure]
        [Authorize]
        public ActionResult MyPurchases()
        {
            int idcustomer = ((Customer)Session["Customer"]).IdCustomer;
            List<SalesDetails> oList = new List<SalesDetails>();
            bool conversion;
            oList = new CN_Sale().ListarPurchase(idcustomer).Select(oc => new SalesDetails()
            {
                oProduct = new Product()
                {
                    Name = oc.oProduct.Name,
                    Price = oc.oProduct.Price,
                    Base64 = CN_Resources.ConvertBase64(Path.Combine(oc.oProduct.RImage, oc.oProduct.NameImage), out conversion),
                    Extension = Path.GetExtension(oc.oProduct.NameImage)
                },
                Amount = oc.Amount,
                Total = oc.Total,
                IdTransaction = oc.IdTransaction
            }).ToList();
            return View(oList);
        }
    }
}