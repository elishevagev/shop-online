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
    public  class CD_Product
    {

        public List<Product> Listar()
        {
            List<Product> list = new List<Product>();
            try
            {
                using (SqlConnection oconection = new SqlConnection(Connection.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select p.IdProduct, p.Name, p.Description,");
                    sb.AppendLine("m.IdBrand, m.Description[DesBrand],");
                    sb.AppendLine("c.IdCategory,c.Description[DesCategory],");
                    sb.AppendLine("p.Price, p.Stock, p.RImage, p.NameImage,p.Active");
                    sb.AppendLine("from PRODUCT p");
                    sb.AppendLine("inner join BRAND m on m.IdBrand = p.IdBrand");
                    sb.AppendLine("inner join CATEGORY c on c.IdCategory = p.IdCategory");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconection);
                    cmd.CommandType = CommandType.Text;
                    oconection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Product()
                            {
                                IdProduct = Convert.ToInt32(dr["IdProduct"]),
                                Name = dr["Name"].ToString(),
                                Description = dr["Description"].ToString(),
                                oBrand = new Brand() { IdBrand = Convert.ToInt32(dr["IdBrand"]), Description = dr["DesBrand"].ToString() },
                                oCategory = new Category() { IdCategory = Convert.ToInt32(dr["IdCategory"]), Description = dr["DesCategory"].ToString() },
                                Price = Convert.ToDecimal(dr["Price"], new CultureInfo("en")),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                RImage = dr["RImage"].ToString(),
                                NameImage = dr["NameImage"].ToString(),
                                Active = Convert.ToBoolean(dr["Active"])
                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Product>();
            }
            return list;
        }
        public int Register(Product obj, out string Message)
        {
            int idautogenerado = 0;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegisterProduct", oconnection);
                    cmd.Parameters.AddWithValue("Name", obj.Name);
                    cmd.Parameters.AddWithValue("Description", obj.Description);
                    cmd.Parameters.AddWithValue("IdBrand", obj.oBrand.IdBrand);
                    cmd.Parameters.AddWithValue("IdCategory", obj.oCategory.IdCategory);
                    cmd.Parameters.AddWithValue("Price", obj.Price);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
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



        public bool Edit(Product obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditProduct", oconnection);
                    cmd.Parameters.AddWithValue("IdProduct", obj.IdProduct);
                    cmd.Parameters.AddWithValue("Name", obj.Name);
                    cmd.Parameters.AddWithValue("Description", obj.Description);
                    cmd.Parameters.AddWithValue("IdBrand", obj.oBrand.IdBrand);
                    cmd.Parameters.AddWithValue("IdCategory", obj.oCategory.IdCategory);
                    cmd.Parameters.AddWithValue("Price", obj.Price);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
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

        public bool SaveImagDetails(Product obj, out string Message)
        {
            bool result = false;
            Message = string.Empty;

            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    string query = "update product set RImage = @RImage, NameImage = @NameImage where IdProduct = @idproduct";
                    SqlCommand cmd = new SqlCommand(query, oconnection);
                    cmd.Parameters.AddWithValue("RImage", obj.RImage);
                    cmd.Parameters.AddWithValue("NameImage", obj.NameImage);
                    cmd.Parameters.AddWithValue("IdProduct", obj.IdProduct);
                    cmd.CommandType = CommandType.Text;

                    oconnection.Open();

                    if (cmd.ExecuteNonQuery() >0 )
                    {
                        result = true;
                    }
                    else
                    {
                        Message = "The image couldn't be updated";
                    }

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
                    SqlCommand cmd = new SqlCommand("sp_DeleteProduct", oconnection);
                    cmd.Parameters.AddWithValue("IdProduct", id);
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
    }
}
