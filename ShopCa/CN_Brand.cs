using DataCa;
using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ShopCa
{
    public class CN_Brand
    {
        private CD_Brand objDataCa = new CD_Brand();

        public List<Brand> Listar()
        {
            return objDataCa.Listar();
        }

        public int Register(Brand obj, out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(obj.Description) || string.IsNullOrWhiteSpace(obj.Description))
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
        public bool Edit(Brand obj, out string Message)
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

        public List<Brand> Listbrandsbycategory(int idcategory)
        {
            return objDataCa.Listbrandsbycategory(idcategory);
        }
    }

}
