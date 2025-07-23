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
                string sql = @"
                    SELECT name, description, cutting_stock, skin_stock, finishing_stock,
                           manufacturing_cost, selling_price
                    FROM products WHERE productid = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hfProductId.Value = productId.ToString();
                            txtName.Text = reader["name"].ToString();
                            txtDescription.Text = reader["description"] == DBNull.Value ? "" : reader["description"].ToString();
                            txtCuttingStock.Text = reader["cutting_stock"].ToString();
                            txtSkinStock.Text = reader["skin_stock"].ToString();
                            txtFinishingStock.Text = reader["finishing_stock"].ToString();
                            txtManufacturingCost.Text = reader["manufacturing_cost"].ToString();
                            txtSellingPrice.Text = reader["selling_price"].ToString();
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
                ShowAlert("Invalid Product ID.");
                return;
            }

            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ShowAlert("Product Name is required.");
                return;
            }

            int cuttingStock = ParseInt(txtCuttingStock.Text);
            int skinStock = ParseInt(txtSkinStock.Text);
            int finishingStock = ParseInt(txtFinishingStock.Text);
            decimal manufacturingCost = ParseDecimal(txtManufacturingCost.Text);
            decimal sellingPrice = ParseDecimal(txtSellingPrice.Text);

            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                    UPDATE products SET 
                        name = @name,
                        description = @description,
                        cutting_stock = @cutting_stock,
                        skin_stock = @skin_stock,
                        finishing_stock = @finishing_stock,
                        manufacturing_cost = @manufacturing_cost,
                        selling_price = @selling_price
                    WHERE productid = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("description", string.IsNullOrEmpty(description) ? DBNull.Value : (object)description);
                    cmd.Parameters.AddWithValue("cutting_stock", cuttingStock);
                    cmd.Parameters.AddWithValue("skin_stock", skinStock);
                    cmd.Parameters.AddWithValue("finishing_stock", finishingStock);
                    cmd.Parameters.AddWithValue("manufacturing_cost", manufacturingCost);
                    cmd.Parameters.AddWithValue("selling_price", sellingPrice);
                    cmd.Parameters.AddWithValue("id", productId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Show alert and redirect
            string script = "alert('Product updated successfully!'); window.location='ProductList.aspx';";
            ClientScript.RegisterStartupScript(this.GetType(), "AlertRedirect", script, true);
        }

        private void ShowAlert(string message)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", script, true);
        }

        private int ParseInt(string input) => int.TryParse(input, out int val) ? val : 0;

        private decimal ParseDecimal(string input) => decimal.TryParse(input, out decimal val) ? val : 0m;
    }
}
