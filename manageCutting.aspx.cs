using System;
using System.Data;
using System.Web.UI.WebControls;
using Npgsql;
using System.Configuration;
using System.Web.UI;

namespace naturefriends
{
    public partial class manageCutting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCuttingStock();
                lblMessage.Text = "";
            }
        }

        private void LoadCuttingStock()
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT productid, name, cutting_stock FROM products ORDER BY productid";
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gridCuttingStock.DataSource = dt;
                    gridCuttingStock.DataBind();
                }
            }
        }

        protected void gridCuttingStock_RowCommand(object sender, GridViewCommandEventArgs e)
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

                UpdateCuttingStock(productId, adjustment, e.CommandName == "Increase");
            }
        }

        private void UpdateCuttingStock(int productId, int amount, bool increase)
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                // For Decrease, ensure we do not go below zero
                if (!increase)
                {
                    // Get current cutting stock first
                    string getStockSql = "SELECT cutting_stock FROM products WHERE productid = @id";
                    using (var getCmd = new NpgsqlCommand(getStockSql, conn))
                    {
                        getCmd.Parameters.AddWithValue("id", productId);
                        int currentStock = Convert.ToInt32(getCmd.ExecuteScalar());
                        if (currentStock < amount)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Cannot decrease. Cutting stock would become negative.";
                            return;
                        }
                    }
                }

                string sql = increase
                    ? "UPDATE products SET cutting_stock = cutting_stock + @amount WHERE productid = @id"
                    : "UPDATE products SET cutting_stock = cutting_stock - @amount WHERE productid = @id";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("amount", amount);
                    cmd.Parameters.AddWithValue("id", productId);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = $"Cutting stock {(increase ? "increased" : "decreased")} successfully.";
                        LoadCuttingStock();
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
