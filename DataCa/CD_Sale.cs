using EntityCa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;

namespace DataCa
{
    public class CD_Sale
    {

        public bool Register(Sales obj,DataTable SalesDetails, out string Message)
        {
            bool res = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegisterSale", oconnection);
                    cmd.Parameters.AddWithValue("IdCustomer", obj.IdCustomer);
                    cmd.Parameters.AddWithValue("TotalProduct",obj.TotalProduct);
                    cmd.Parameters.AddWithValue("OrderTotal", obj.OrderTotal);
                    cmd.Parameters.AddWithValue("Contact", obj.Contact);
                    cmd.Parameters.AddWithValue("IdCity", obj.IdCity);
                    cmd.Parameters.AddWithValue("Tel", obj.Tel);
                    cmd.Parameters.AddWithValue("Address", obj.Address);
                    cmd.Parameters.AddWithValue("IdTransaction", obj.IdTransaction);
                    cmd.Parameters.AddWithValue("SalesDetails", SalesDetails);
                    cmd.Parameters.Add("Result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    res = Convert.ToBoolean(cmd.Parameters["Result"].Value);
                    Message = cmd.Parameters["Message"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                res = false;
                Message = ex.Message;
            }
            return res;
        }

        public List<SalesDetails> ListarPurchase(int idcustomer)
        {
            List<SalesDetails> list = new List<SalesDetails>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    string query = "select * from fn_ListarPurchase(@idcustomer)";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@idcustomer", idcustomer);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new SalesDetails()
                            {
                                oProduct = new Product()
                                {
                                    Name = dr["Name"].ToString(),
                                    Price = Convert.ToDecimal(dr["Price"], new CultureInfo("en")),
                                    RImage = dr["RImage"].ToString(),
                                    NameImage = dr["NameImage"].ToString(),

                                },
                                Amount = Convert.ToInt32(dr["Amount"]),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("en")),
                                IdTransaction = dr["IdTransaction"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<SalesDetails>();
            }
            return list;
        }

    }
}
