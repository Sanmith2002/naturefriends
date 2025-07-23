using System;
using System.Data;
using System.Web.UI.WebControls;
using Npgsql;
using System.Configuration;
using System.Web.UI;

namespace naturefriends
{
    public partial class manageSkin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSkinStock();
                lblMessage.Text = "";
            }
        }

        private void LoadSkinStock()
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT productid, name, cutting_stock, skin_stock FROM products ORDER BY productid";
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gridSkinStock.DataSource = dt;
                    gridSkinStock.DataBind();
                }
            }
        }

        protected void gridSkinStock_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    IncreaseSkinStock(productId, adjustment);
                }
                else
                {
                    DecreaseSkinStock(productId, adjustment);
                }
            }
        }

        private void IncreaseSkinStock(int productId, int amount)
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // Check if cutting stock is enough
                string checkSql = "SELECT cutting_stock FROM products WHERE productid = @id";
                using (var checkCmd = new NpgsqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("id", productId);
                    int cuttingStock = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (cuttingStock < amount)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Not enough cutting stock to increase skin stock.";
                        return;
                    }
                }

                // Perform update: decrease cutting, increase skin
                string updateSql = @"
                    UPDATE products
                    SET cutting_stock = cutting_stock - @amount,
                        skin_stock = skin_stock + @amount
                    WHERE productid = @id";

                using (var updateCmd = new NpgsqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("amount", amount);
                    updateCmd.Parameters.AddWithValue("id", productId);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Skin stock increased and cutting stock decreased successfully.";
                        LoadSkinStock();
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Update failed. Product not found.";
                    }
                }
            }
        }

        private void DecreaseSkinStock(int productId, int amount)
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // Check if skin stock is enough to decrease
                string checkSql = "SELECT skin_stock FROM products WHERE productid = @id";
                using (var checkCmd = new NpgsqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("id", productId);
                    int skinStock = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (skinStock < amount)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Not enough skin stock to decrease.";
                        return;
                    }
                }

                // Perform update: increase cutting, decrease skin
                string updateSql = @"
                    UPDATE products
                    SET cutting_stock = cutting_stock + @amount,
                        skin_stock = skin_stock - @amount
                    WHERE productid = @id";

                using (var updateCmd = new NpgsqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("amount", amount);
                    updateCmd.Parameters.AddWithValue("id", productId);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Skin stock decreased and cutting stock increased successfully.";
                        LoadSkinStock();
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
