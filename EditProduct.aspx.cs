using System;
using Npgsql;
using System.Configuration;

namespace naturefriends
{
    public partial class EditProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get product ID from query string
                if (int.TryParse(Request.QueryString["id"], out int productId))
                {
                    LoadProduct(productId);
                }
                else
                {
                    Response.Redirect("ProductList.aspx");
                }
            }
        }

        private void LoadProduct(int productId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT name, description FROM products WHERE productid = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hfProductId.Value = productId.ToString();
                            txtName.Text = reader["name"].ToString();
                            txtDescription.Text = reader["description"].ToString();
                        }
                        else
                        {
                            Response.Redirect("ProductList.aspx");
                        }
                    }
                }
            }
        }

        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfProductId.Value, out int productId))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid Product ID.";
                return;
            }

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
                string sql = "UPDATE products SET name = @name, description = @description WHERE productid = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("id", productId);
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Product updated successfully!";
        }
    }
}
