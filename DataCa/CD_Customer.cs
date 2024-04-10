using EntityCa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCa
{
    public class CD_Customer
    {
        public int Register(Customer obj, out string Message)
        {
            int idautogenerado = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegisterCustomer", oconnection);
                    cmd.Parameters.AddWithValue("Name", obj.Name);
                    cmd.Parameters.AddWithValue("LastName", obj.LastName);
                    cmd.Parameters.AddWithValue("Email", obj.Email);
                    cmd.Parameters.AddWithValue("Password", obj.Password);
                    cmd.Parameters.Add("Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Result"].Value);
                    Message = cmd.Parameters["Message"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Message = ex.Message;
            }
            return idautogenerado;
        }

        public List<Customer> Listar()
        {
            List<Customer> list = new List<Customer>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    string query = "SELECT IdCustomer, Name, LastName, Email,Password,Reset FROM Customer";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Customer()
                            {
                                IdCustomer = Convert.ToInt32(dr["IdCustomer"]),
                                Name = dr["Name"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                Password = dr["Password"].ToString(),
                                Reset = Convert.ToBoolean(dr["Reset"])
                          
                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Customer>();
            }
            return list;
        }

        




        public bool ChangePassword(int idcustomer, string newpassword, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Customer set password = @newpassword , reset = 0 where idcustomer = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", idcustomer);
                    cmd.Parameters.AddWithValue("@newpassword", newpassword);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;
        }

        public bool ResetPassword(int idcustomer, string password, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Customer set password = @password , reset = 1 where idcustomer = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", idcustomer);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;
        }

    }
}
