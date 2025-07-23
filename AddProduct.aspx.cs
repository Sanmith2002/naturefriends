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
                    INSERT INTO products 
                    (name, description, cutting_stock, skin_stock, finishing_stock, manufacturing_cost, selling_price)
                    VALUES 
                    (@name, @description, @cutting_stock, @skin_stock, @finishing_stock, @manufacturing_cost, @selling_price)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("description", string.IsNullOrEmpty(description) ? DBNull.Value : (object)description);
                    cmd.Parameters.AddWithValue("cutting_stock", cuttingStock);
                    cmd.Parameters.AddWithValue("skin_stock", skinStock);
                    cmd.Parameters.AddWithValue("finishing_stock", finishingStock);
                    cmd.Parameters.AddWithValue("manufacturing_cost", manufacturingCost);
                    cmd.Parameters.AddWithValue("selling_price", sellingPrice);
                    cmd.ExecuteNonQuery();
                }
            }

            // Show success alert and redirect to product list
            string script = $"alert('Product \"{name.Replace("'", "\\'")}\" added successfully!'); window.location='ProductList.aspx';";
            ClientScript.RegisterStartupScript(this.GetType(), "AlertRedirect", script, true);

            // Optional: Clear form fields if you want (won't be seen because of redirect)
            txtName.Text = "";
            txtDescription.Text = "";
            txtCuttingStock.Text = "";
            txtSkinStock.Text = "";
            txtFinishingStock.Text = "";
            txtManufacturingCost.Text = "";
            txtSellingPrice.Text = "";
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
