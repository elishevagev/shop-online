using EntityCa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DataCa
{
    public class CD_Cart
    {
        public bool ExistCart(int idcustomer, int idproduct)
        {
            bool result = true;
            
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ExistCart", oconnection);
                    cmd.Parameters.AddWithValue("IdCustomer", idcustomer);
                    cmd.Parameters.AddWithValue("IdProduct", idproduct);
                    cmd.Parameters.Add("Result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["Result"].Value);
           
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public bool cartOperation(int idcustomer, int idproduct, bool sum, out string Message)
        {
            bool result = true;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_cartOperation", oconnection);
                    cmd.Parameters.AddWithValue("IdCustomer", idcustomer);
                    cmd.Parameters.AddWithValue("IdProduct", idproduct);
                    cmd.Parameters.AddWithValue("Sum", sum);

                    cmd.Parameters.Add("Result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["Result"].Value);
                    Message = cmd.Parameters["Message"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;
        }

        public int TotalCart(int idcustomer)
        {
            int result = 0;
            
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from CART where idcustomer = @IdCustomer", oconnection);
                    cmd.Parameters.AddWithValue("@idcustomer", idcustomer);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                result = 0;
            
            }
            return result;
        }
        public List<Cart> ListarProduct(int idcustomer )
        {
            List<Cart> list = new List<Cart>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    string query = "select * from fn_getCartCustomer(@idcustomer)";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.Parameters.AddWithValue("@idcustomer", idcustomer);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Cart()
                            {
                                oProduct = new Product()
                                {
                                    IdProduct = Convert.ToInt32(dr["IdProduct"]),
                                    Name = dr["Name"].ToString(),
                                    Price = Convert.ToDecimal(dr["Price"], new CultureInfo("en")),
                                    RImage = dr["RImage"].ToString(),
                                    NameImage = dr["NameImage"].ToString(),
                                    oBrand = new Brand() {  Description = dr["DesBrand"].ToString() },
                                   
                                },
                                Amount = Convert.ToInt32(dr["Amount"]),

                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Cart>();
            }
            return list;
        }
        public bool DeleteCart(int idcustomer, int idproduct)
        {
            bool result = true;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_DeleteCart", oconnection);
                    cmd.Parameters.AddWithValue("IdCustomer", idcustomer);
                    cmd.Parameters.AddWithValue("IdProduct", idproduct);
                    cmd.Parameters.Add("Result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["Result"].Value);

                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
