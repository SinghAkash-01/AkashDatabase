using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkashDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            //1. Address of SQL Server and database(ConnectionString)
            string ConnectionString = "Data Source=DESKTOP-1NGBV9B\\SQLEXPRESS;Initial Catalog=MyDB;Integrated Security=True;Encrypt=False";

            //2. Establish connection(C# SqlConnection class)
            SqlConnection con = new SqlConnection(ConnectionString);

            //3. Open connection(C# Sqlconnection open)
            con.Open();

            //4. Prepare Sql Query
            string Query = "INSERT INTO Names (FirstName, SecondName, Email) VALUES (@1, @2, @3)";

            //5. Execute Query(C# Command class)
            SqlCommand cmd = new SqlCommand(Query, con);

            cmd.Parameters.AddWithValue("@1", tbFirstName.Text);
            cmd.Parameters.AddWithValue("@2", tbSecondName.Text);
            cmd.Parameters.AddWithValue("@3", tbEmail.Text);

            cmd.ExecuteNonQuery();

            //6. Close Connection(C# SqlConnection class)
            con.Close();

            MessageBox.Show("Data has been Saved.");

            tbFirstName.Text = "";
            tbSecondName.Text = "";
            tbEmail.Text = "";
            tbID.Text = "";

            btShowData_Click(null, null);


        }

        private void btShowData_Click(object sender, EventArgs e)
        {
            
            var reader = new DB().Select("SELECT * FROM Names");

            dataGridView1.Rows.Clear();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["ID"], reader["FirstName"], reader["SecondName"], reader["Email"], "Edit", "Delete");
            }
        }

        private void tbUpdate_Click(object sender, EventArgs e)
        {
            new DB().Execute("UPDATE Names SET FirstName = '" + tbFirstName.Text + "', SecondName = '" + tbSecondName.Text + "', Email = '" + tbEmail.Text + "' WHERE ID = " + tbID.Text);

            MessageBox.Show("Data has been updated.");

            tbFirstName.Text = "";
            tbSecondName.Text = "";
            tbEmail.Text = "";
            tbID.Text = "";

            btShowData_Click(null, null);
        }

        private void tbFetch_Click(object sender, EventArgs e)
        {
            var reader = new DB().Select("SELECT * FROM Names WHERE ID = " + tbID.Text);
            if (reader.Read())
            {
                tbFirstName.Text = reader["FirstName"].ToString();
                tbSecondName.Text = reader["SecondName"].ToString();
                tbEmail.Text = reader["Email"].ToString();
            }
            else
                MessageBox.Show("No Record Found");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4 && e.RowIndex > -1)
            {
                tbID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            else if(e.ColumnIndex == 5 && e.RowIndex > -1)
            {
                tbID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (MessageBox.Show("Do you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    new DB().Execute("DELETE FROM Names WHERE ID = " + tbID.Text);
                    
                    MessageBox.Show("Data has been deleted.");

                    tbFirstName.Text = "";
                    tbSecondName.Text = "";
                    tbEmail.Text = "";
                    tbID.Text = "";

                    btShowData_Click(null, null);
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            new DB().Execute("DELETE FROM Names WHERE ID = " + tbID.Text);
            
            MessageBox.Show("Data has been deleted.");

            tbFirstName.Text = "";
            tbSecondName.Text = "";
            tbEmail.Text = "";
            tbID.Text = "";

            btShowData_Click(null, null);
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            var reader = new DB().Select("SELECT * FROM Names WHERE FirstName LIKE '%" + tbSearch.Text + "%' OR SecondName LIKE '%" + tbSearch.Text + "%' OR Email LIKE '%" + tbSearch.Text + "%'");

            dataGridView1.Rows.Clear();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["ID"], reader["FirstName"], reader["SecondName"], reader["Email"], "Edit", "Delete");
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string SearchData = tbSearch.Text;

            string Query = "SELECT * FROM Names";
            
            if(cbColumn.SelectedIndex == 0)
            {
                Query += " WHERE FirstName LIKE '%" + SearchData + "%' OR SecondName LIKE '%" + SearchData + "%' OR Email LIKE '%" + SearchData + "%'";

                if (int.TryParse(SearchData, out _))
                {
                    Query += " OR ID = " + SearchData;
                }
            }
            else
            {
                if(cbColumn.SelectedIndex == 1 && SearchData.Length > 0)
                {
                    Query += " WHERE ID = " + SearchData;
                }
                else if(cbColumn.SelectedIndex == 2)
                {
                    Query += " WHERE FirstName LIKE '%" + SearchData + "%'";
                }
                else if(cbColumn.SelectedIndex == 3)
                {
                    Query += " WHERE SecondName LIKE '%" + SearchData + "%'";
                }
                else if (cbColumn.SelectedIndex == 4)
                {
                    Query += " WHERE Email LIKE '%" + SearchData + "%'";
                }
            }

            var reader = new DB().Select(Query);

            dataGridView1.Rows.Clear();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["ID"], reader["FirstName"], reader["SecondName"], reader["Email"], "Edit", "Delete");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btShowData_Click(null, null);
            cbColumn.SelectedIndex = 0;
        }
    }
}
