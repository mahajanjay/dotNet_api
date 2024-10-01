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

    }
}
