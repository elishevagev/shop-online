using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCa;
using EntityCa;

namespace ShopCa
{
    public class CN_Report
    {
        private CD_Report objDataCa = new CD_Report();


        public List<Report> Sales(string startdate, string enddate, string idtransaction)
        {
            return objDataCa.Sales(startdate, enddate, idtransaction);  
        }


            public DashBoard ShowDashBoard()
        {
            return objDataCa.ShowDashBoard();
        }
    }
}
