using System;
using System.Web.UI;
using Npgsql;
using System.Configuration;

namespace naturefriends
{
    public partial class AddProduct : Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Product Name is required.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "INSERT INTO products (name, description) VALUES (@name, @description)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("description", (object)description ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = $"Product '{name}' added successfully!";

            txtName.Text = "";
            txtDescription.Text = "";
        }
    }
}
