using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCa
{
    public class SalesDetails
    {
        public int IdSalesDetails { get; set; }
        public Sales oSales { get; set; }
        public Product oProduct { get; set; }
        public int Amount { get; set; }
        public decimal Total { get; set; }
        public string IdTransaction { get; set; }
    }
}


