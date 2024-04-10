using DataCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCa;

namespace ShopCa
{
    public class CN_Category
    {
        private CD_Category objDataCa = new CD_Category();

        public List<Category> Listar()
        { 
            return objDataCa.Listar();
        }

        public int Register(Category obj, out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description ))
            {
                Message = "Description is required";
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
        public bool Edit(Category obj, out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
            {
                Message = "Description is required";
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
        public bool Delete(int id, out string Message)
        {
            return objDataCa.Delete(id, out Message);
        }
    }
    

}
