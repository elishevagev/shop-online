using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCa;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DataCa
{
    public class CD_Report
    {

        public List<Report> Sales(string startdate, string enddate, string idtransaction)
        {
            List<Report> list = new List<Report>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    
                    SqlCommand cmd = new SqlCommand("sp_SalesReport", oconection);
                    cmd.Parameters.AddWithValue("startdate", startdate);
                    cmd.Parameters.AddWithValue("enddate", enddate);
                    cmd.Parameters.AddWithValue("idtransaction", idtransaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Report()
                            {
                                
                                DateSale= dr["DateSale"].ToString(),
                                Customer = dr["Customer"].ToString(),
                                Product = dr["Product"].ToString(),
                                Price = Convert.ToDecimal(dr["Price"], new CultureInfo("en")),
                                Amount = Convert.ToInt32(dr["Amount"].ToString()),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("en")),
                                IdTransaction = dr["IdTransaction"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Report>();
            }
            return list;
        }

        public DashBoard ShowDashBoard()
        {
            DashBoard objeto = new DashBoard();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_DashboardReport", oconection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new DashBoard()
                            {
                                TotalCustomer = Convert.ToInt32(dr["TotalCustomer"]),
                                TotalSale = Convert.ToInt32(dr["TotalSale"]),
                                TotalProduct= Convert.ToInt32(dr["TotalProduct"])
                            };  
                            
                        }
                    }
                }
            }
            catch
            {
                objeto = new DashBoard();
            }
            return objeto;
        }

    }
}
