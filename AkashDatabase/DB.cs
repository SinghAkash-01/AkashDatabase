using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AkashDatabase
{
    internal class DB
    {
        //1. Address of SQL Server and database(ConnectionString)
        string ConnectionString = "Data Source=DESKTOP-1NGBV9B\\SQLEXPRESS;Initial Catalog=MyDB;Integrated Security=True;Encrypt=False";

        //2. Establish connection(C# SqlConnection class)
        SqlConnection con = null;

        public DB()
        {
            con = new SqlConnection(ConnectionString);
        }

        public void Execute(string Query)
        {
            try
            {
                //3. Open connection(C# Sqlconnection open)
                con.Open();

                //4. Prepare Sql Query
                SqlCommand cmd = new SqlCommand(Query, con);

                //5. Execute Query(C# Command class)
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //6. Close Connection(C# SqlConnection class)
                con.Close();
            }
        }

        public SqlDataReader Select(string Query)
        {
            try
            {
                //3. Open connection(C# Sqlconnection open)
                con.Open();

                //4. Prepare Sql Query
                SqlCommand cmd = new SqlCommand(Query, con);

                //5. Execute Query(C# Command class)
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }
    }
}
