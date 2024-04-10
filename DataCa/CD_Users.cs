using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCa;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;


namespace DataCa
{
    public class CD_Users
    {
        public List<User_Information> Listar()
        {
            List<User_Information> list = new List<User_Information>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn) )
                {
                    string query = "SELECT IdUser, Name, LastName, Email,Password,Reset,Active FROM USER_INFORMATION";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new User_Information()
                            {
                                IdUser = Convert.ToInt32(dr["IdUser"]),
                                Name = dr["Name"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                Email = dr["Email"].ToString(),
                                Password = dr["Password"].ToString(),
                                Reset = Convert.ToBoolean(dr["Reset"]),
                                Active = Convert.ToBoolean(dr["Active"]),
                            });
                        }
                    }
                }
            }
            catch { 
                list = new List<User_Information>(); 
            }
            return list;
        }
        public int Register(User_Information obj, out string Message)
        {
            int idautogenerado = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegisterUser", oconnection);
                    cmd.Parameters.AddWithValue("Name", obj.Name);
                    cmd.Parameters.AddWithValue("LastName", obj.LastName);
                    cmd.Parameters.AddWithValue("Email", obj.Email);
                    cmd.Parameters.AddWithValue("Password", obj.Password);
                    cmd.Parameters.AddWithValue("Active", obj.Active);
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
        public bool Edit(User_Information obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditUser", oconnection);
                    cmd.Parameters.AddWithValue("IdUser", obj.IdUser);
                    cmd.Parameters.AddWithValue("Name", obj.Name);
                    cmd.Parameters.AddWithValue("LastName", obj.LastName);
                    cmd.Parameters.AddWithValue("Email", obj.Email);
                    cmd.Parameters.AddWithValue("Active", obj.Active);
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

        public bool Delete(int id, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using(SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1) from User_information where IdUser = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    result = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch(Exception ex)
            {
                result = false;
                Message = ex.Message;
            }
            return result;  
        }

        public bool ChangePassword(int iduser, string newpassword,out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update user_information set password = @newpassword , reset = 0 where iduser = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", iduser);
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
        public bool ResetPassword(int iduser, string password, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("update user_information set password = @password , reset = 1 where iduser = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", iduser);
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
