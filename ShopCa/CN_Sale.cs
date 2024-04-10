using DataCa;
using EntityCa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCa
{
    public class CN_Sale
    {
        private CD_Sale objCapaDa = new CD_Sale();

        public bool Register(Sales obj, DataTable SalesDetails, out string Message)
        {
            return objCapaDa.Register(obj, SalesDetails, out Message);
        }



        public List<SalesDetails> ListarPurchase(int idcustomer)
        {
            return objCapaDa.ListarPurchase(idcustomer);
        }
    }
}
