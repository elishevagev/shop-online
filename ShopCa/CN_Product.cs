using DataCa;
using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCa
{
    public class CN_Product
    {
        private CD_Product objDataCa = new CD_Product();

        public List<Product> Listar()
        {
            return objDataCa.Listar();
        }

        public int Register(Product obj, out string Message)
        {
            Message = string.Empty;
          
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
            {
                Message = "Name is required";
            }
            else if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
            {
                Message = "Description is required";
            }
            else if(obj.oBrand.IdBrand == 0)
            {
                Message = "You must select the brand";
            }
            else if (obj.oCategory.IdCategory == 0)
            {
                Message = "You must select the category";
            }
            else if (obj.Price == 0)
            {
                Message = "You must enter the product price.";
            }
            else if (obj.Stock== 0)
            {
                Message = "You must enter the product Stock.";
            }


            if (string.IsNullOrEmpty(Message))
            {

                return objDataCa.Register(obj, out Message);


            }
            else
            {
                return 0;
            }

        }
        public bool Edit(Product obj, out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
            {
                Message = "Name is required";
            }
            else if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
            {
                Message = "Description is required";
            }
            else if (obj.oBrand.IdBrand == 0)
            {
                Message = "You must select the brand";
            }
            else if (obj.oCategory.IdCategory == 0)
            {
                Message = "You must select the category";
            }
            else if (obj.Price == 0)
            {
                Message = "You must enter the product price.";
            }
            else if (obj.Stock == 0)
            {
                Message = "You must enter the product Stock.";
            }

            if (string.IsNullOrEmpty(Message))
            {
                return objDataCa.Edit(obj, out Message);
            }
            else
            {
                return false;
            }
        }

        public bool SaveImagDetails(Product obj, out string Message)
        {
            return objDataCa.SaveImagDetails(obj, out Message);
        }

        public bool Delete(int id, out string Message)
        {
            return objDataCa.Delete(id, out Message);
        }
    }
}
