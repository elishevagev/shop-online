using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCa
{
    public class Sales
    {
        public int IdSales { get; set; }
        public int IdCustomer{ get; set; }
        public int TotalProduct { get; set; }
        public decimal OrderTotal { get; set; }
        public string Contact {  get; set; }
        public string IdCity { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string IdTransaction { get; set; }
        public List<SalesDetails> oSalesDetails { get; set; }   

    }
}

