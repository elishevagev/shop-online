using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCa
{
    public class Customer
    {
        public int IdCustomer { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool Reset {  get; set; }
    }
}


