using FirstApp.Models;
using System.Data;
using System.Data.SqlClient;

namespace FirstApp.DBConnection
{
    public class DBService
    {
        public readonly SqlConnection Customer_connection;

        public DBService(IDatabseSetting databseSetting)
        {
            Customer_connection = new SqlConnection();
            Customer_connection.ConnectionString = databseSetting.CustomerConnectionString;
        }

        public void OpenConnection()
        {
            if (Customer_connection == null || Customer_connection.State == System.Data.ConnectionState.Closed)
            {
                Customer_connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (Customer_connection.State == System.Data.ConnectionState.Open) 
            {
                Customer_connection.Close();
            }
        }

        #region Customers
        public DataTable GetCustomers()
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("[dbo].[SP_Details]", Customer_connection);
                cmd.Parameters.Add("@MODE", SqlDbType.NVarChar).Value = "list";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex) { }
            finally 
            {
                CloseConnection();
            }
            
            return dt;
        }

        public int AddCustomer(CustomerModel customerModel)
        {
            int result = 0;
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("[dbo].[SP_Details]", Customer_connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "insert";
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = customerModel.Name;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = customerModel.Email;
                result = cmd.ExecuteNonQuery();
            }
            catch(Exception ex) { }
            finally { CloseConnection(); }

            return result;
        }

        public int UpdateCustomer(CustomerModel customerModel)
        {
            int result = 0;
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("[dbo].[SP_Details]", Customer_connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "update";
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = customerModel.Id;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value= customerModel.Name;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value= customerModel.Email;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { CloseConnection(); }
            return result;
        }

        public int DeleteCustomer(int id)
        {
            int result = 0;
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("[dbo].[SP_Details]", Customer_connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "delete";
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally { CloseConnection(); }
            return result;
        }

        #endregion Cutstomers

        #region File

        public int AddFile(string fileName, byte[] data)
        {
            int result = 0;
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand("[dbo].[SP_Details]", Customer_connection);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.Add("@mode", SqlDbType.NVarChar).Value = "addFile";
                cmd.Parameters.Add("@FILE_NAME", SqlDbType.NVarChar).Value = fileName;
                cmd.Parameters.Add("@FILE_DATA", SqlDbType.VarBinary).Value = data;
                object newFileId = cmd.ExecuteScalar();

                if (newFileId != null)
                {
                    result = Convert.ToInt32(newFileId); // Convert the result to an integer
                }
            }
            catch (Exception ex) { }
            finally { CloseConnection(); }
            return result;
        } 

        #endregion File

    }
}
