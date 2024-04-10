using DataCa;
using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCa
{
    public class CN_Cart
    {
        private CD_Cart objDataCa = new CD_Cart();

        public bool ExistCart(int idcustomer, int idproduct)
        {
            return objDataCa.ExistCart(idcustomer, idproduct);
        }

        public bool cartOperation(int idcustomer, int idproduct, bool sum, out string Message)
        {
            return objDataCa.cartOperation(idcustomer, idproduct, sum, out Message);
        }

        public int TotalCart(int idcustomer)
        {
            return objDataCa.TotalCart(idcustomer);
        }

        public List<Cart> ListarProduct(int idcustomer)
        {
            return objDataCa.ListarProduct(idcustomer);
        }

        public bool DeleteCart(int idcustomer, int idproduct)
        {
            return objDataCa.DeleteCart(idcustomer, idproduct);

        }


    }
}
