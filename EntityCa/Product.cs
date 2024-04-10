using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EntityCa
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public Brand oBrand{ get; set; }
        public Category oCategory { get; set; }
        public decimal Price {  get; set; }
        public string PriceText { get; set; }   
        public int Stock { get; set; }
        public string RImage { get; set; }
        public string NameImage { get; set; }
        public bool Active {  get; set; }
        public string Base64 { get; set; }  
        public string Extension {  get; set; }

    }
}

