using System;
using System.Data;
using System.Web.UI.WebControls;
using Npgsql;
using System.Configuration;
using System.Web.UI;

namespace naturefriends
{
    public partial class manageFinishing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFinishingStock();
                lblMessage.Text = "";
            }
        }

        private void LoadFinishingStock()
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT productid, name, skin_stock, finishing_stock FROM products ORDER BY productid";
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gridFinishingStock.DataSource = dt;
                    gridFinishingStock.DataBind();
                }
            }
        }

        protected void gridFinishingStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Increase" || e.CommandName == "Decrease")
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                TextBox txtAdjust = (TextBox)row.FindControl("txtAdjust");

                if (!int.TryParse(txtAdjust.Text.Trim(), out int adjustment) || adjustment <= 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Please enter a valid positive number for adjustment.";
                    return;
                }

                if (e.CommandName == "Increase")
                {
                    IncreaseFinishingStock(productId, adjustment);
                }
                else
                {
                    DecreaseFinishingStock(productId, adjustment);
                }
            }
        }

        private void IncreaseFinishingStock(int productId, int amount)
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // Check if skin stock is enough
                string checkSql = "SELECT skin_stock FROM products WHERE productid = @id";
                using (var checkCmd = new NpgsqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("id", productId);
                    int skinStock = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (skinStock < amount)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Not enough skin stock to increase finishing stock.";
                        return;
                    }
                }

                // Update stocks
                string updateSql = @"
                    UPDATE products
                    SET skin_stock = skin_stock - @amount,
                        finishing_stock = finishing_stock + @amount
                    WHERE productid = @id";

                using (var updateCmd = new NpgsqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("amount", amount);
                    updateCmd.Parameters.AddWithValue("id", productId);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Finishing stock increased and skin stock decreased successfully.";
                        LoadFinishingStock();
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Update failed. Product not found.";
                    }
                }
            }
        }

        private void DecreaseFinishingStock(int productId, int amount)
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // Check if finishing stock is enough
                string checkSql = "SELECT finishing_stock FROM products WHERE productid = @id";
                using (var checkCmd = new NpgsqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("id", productId);
                    int finishingStock = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (finishingStock < amount)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Not enough finishing stock to decrease.";
                        return;
                    }
                }

                // Update stocks
                string updateSql = @"
                    UPDATE products
                    SET skin_stock = skin_stock + @amount,
                        finishing_stock = finishing_stock - @amount
                    WHERE productid = @id";

                using (var updateCmd = new NpgsqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("amount", amount);
                    updateCmd.Parameters.AddWithValue("id", productId);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Finishing stock decreased and skin stock increased successfully.";
                        LoadFinishingStock();
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Update failed. Product not found.";
                    }
                }
            }
        }
    }
}
