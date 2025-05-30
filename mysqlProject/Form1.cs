using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
namespace mysqlProject
{
    public partial class Form1 : Form
    {
        MySqlConnection sqlConn = new MySqlConnection();
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable sqlDt = new DataTable();
        String sqlQuery;
        MySqlDataAdapter sqlDa = new MySqlDataAdapter();
        MySqlDataReader sqlDr;
        DataSet Ds = new DataSet();
        String Server = "localhost";
        String username = "root";
        String password = "1254";
        String database = "company";
        public Form1()
        {
            InitializeComponent();
        }

        private void upLoadData()
        {
            sqlConn.ConnectionString = "Server=" + Server + ";" +
                           "username=" + username + ";" +
                           "Password=" + password + ";" +
                           "Database=" + database + ";";
            sqlConn.Open();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = "SELECT * FROM company.company";
            sqlDr = sqlCmd.ExecuteReader();
            sqlDt.Load(sqlDr);
            sqlDr.Close();
            sqlConn.Close();
            dataGridView1.DataSource = sqlDt;
        }

        private void refNum()
        {
            Random rnd = new Random();
            int refNum = rnd.Next(541, 32554);
            txtBarCode.Text = "Additional Ref" + Convert.ToString(refNum * 12);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult iExit;
                iExit = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (iExit == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in panel3.Controls)
                {
                    if (c is TextBox)
                    {
                        ((TextBox)c).Clear();
                    }
                }
                txtSearch.Clear();
                txtBarCode.Clear();
                cbGender.Text = "";
                cbType.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refNum();
            upLoadData();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConn.ConnectionString = "Server=" + Server + ";" +
                           "username=" + username + ";" +
                           "Password=" + password + ";" +
                           "Database=" + database + ";";
                sqlConn.Open();
                sqlQuery = "insert into company.company(memid, firsrname, surname, address," +
           "postcode, dender, mobile, email, type) " +
           "values('" + txtMemberID.Text + "', '" + txtFirstname.Text + "', '" + txtSurname.Text + "', '" +
           txtAddress.Text + "', '" + txtPostCode.Text + "', '" + cbGender.Text + "', '" +
           txtMobile.Text + "', '" + txtEmail.Text + "', '" + cbType.Text + "')";

                sqlCmd = new MySqlCommand(sqlQuery, sqlConn);
                sqlDr = sqlCmd.ExecuteReader();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConn.Close();
            }
            upLoadData();
            refNum();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtMemberID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtFirstname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtSurname.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtAddress.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtPostCode.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                cbGender.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtMobile.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtEmail.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                cbType.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter = string.Format("firsrname LIKE '%{0}%'", txtSearch.Text);
                dataGridView1.DataSource = dv.ToTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConn.ConnectionString = "Server=" + Server + ";" +
                           "username=" + username + ";" +
                           "Password=" + password + ";" +
                           "Database=" + database + ";";
                
                sqlConn.Open();
                MySqlCommand sqlCmd = new MySqlCommand("DELETE FROM company.company WHERE memid = @memid", sqlConn);
                sqlCmd.Parameters.AddWithValue("@memid", txtMemberID.Text);
                sqlCmd.ExecuteNonQuery();
                sqlConn.Close();
                btnReset_Click(sender, e);
                foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConn.Open();
                sqlQuery = "Select * from company.company where memid = {id} ";
                sqlQuery = sqlQuery.Replace("{id}", txtSearch.Text);
                sqlCmd = new MySqlCommand(sqlQuery, sqlConn);
                sqlDr = sqlCmd.ExecuteReader();
                if (sqlDr.Read())
                {
                    txtMemberID.Text = sqlDr["memid"].ToString();
                    txtFirstname.Text = sqlDr["firsrname"].ToString();
                    txtSurname.Text = sqlDr["surname"].ToString();
                    txtAddress.Text = sqlDr["address"].ToString();
                    txtPostCode.Text = sqlDr["postcode"].ToString();
                    cbGender.Text = sqlDr["dender"].ToString();
                    txtMobile.Text = sqlDr["mobile"].ToString();
                    txtEmail.Text = sqlDr["email"].ToString();
                    cbType.Text = sqlDr["type"].ToString();
                }
                else
                {
                    MessageBox.Show("No record found");
                }
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
