using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCa;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace DataCa
{
    public class CD_Brand
    {
        public List<Brand> Listar()
        {
            List<Brand> list = new List<Brand>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    string query = "SELECT IdBrand, Description, Active FROM BRAND";
                    SqlCommand cmd = new SqlCommand(query, oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Brand()
                            {
                                IdBrand = Convert.ToInt32(dr["IdBrand"]),
                                Description = dr["Description"].ToString(),
                                Active = Convert.ToBoolean(dr["Active"]),
                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Brand>();
            }
            return list;
        }

        public int Register(Brand obj, out string Message)
        {
            int idautogenerado = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegisterBrand", oconnection);
                    cmd.Parameters.AddWithValue("Description", obj.Description);
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

        public bool Edit(Brand obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditBrand", oconnection);
                    cmd.Parameters.AddWithValue("IdBrand", obj.IdBrand);
                    cmd.Parameters.AddWithValue("Description", obj.Description);
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
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteBrand", oconnection);
                    cmd.Parameters.AddWithValue("IdBrand", id);
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
        public List<Brand> Listbrandsbycategory(int idcategory)
        {
            List<Brand> list = new List<Brand>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                   
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select distinct m.IdBrand, m.Description from PRODUCT p");
                    sb.AppendLine("inner join CATEGORY c on c.IdCategory = p.IdCategory");
                    sb.AppendLine("inner join BRAND m on m.IdBrand = p.idbrand and m.Active=1");
                    sb.AppendLine("where c.IdCategory = iif(@idcategory =0,c.IdCategory,@idcategory)");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconection);
                    cmd.Parameters.AddWithValue("@idcategory", idcategory);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Brand()
                            {
                                IdBrand = Convert.ToInt32(dr["IdBrand"]),
                                Description = dr["Description"].ToString(),

                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Brand>();
            }
            return list;
        }

    }
}
