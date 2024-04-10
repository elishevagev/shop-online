using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCa
{
    public class Report
    {
        public string DateSale {  get; set; }
        public string Customer { get; set; }
        public string Product{ get; set; }
        public decimal Price{ get; set; }
        public int Amount{ get; set; }
        public decimal Total { get; set; }
        public string IdTransaction{ get; set; }



    }
}
