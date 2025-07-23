using System;
using System.Data;
using System.Web.UI.WebControls;
using Npgsql;
using System.Configuration;

namespace naturefriends
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
                LoadTotalStocks();
            }
        }

        private void LoadProducts()
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                    SELECT productid, name, description, cutting_stock, skin_stock, finishing_stock,
                           manufacturing_cost, selling_price
                    FROM products
                    ORDER BY productid";
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gridProducts.DataSource = dt;
                    gridProducts.DataBind();
                }
            }
        }

        private void LoadTotalStocks()
        {
            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        COALESCE(SUM(cutting_stock), 0) AS total_cutting,
                        COALESCE(SUM(skin_stock), 0) AS total_skin,
                        COALESCE(SUM(finishing_stock), 0) AS total_finishing
                    FROM products";
                using (var cmd = new NpgsqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalCuttingStock.Text = reader["total_cutting"].ToString();
                        lblTotalSkinStock.Text = reader["total_skin"].ToString();
                        lblTotalFinishingStock.Text = reader["total_finishing"].ToString();
                    }
                }
            }
        }

        protected void gridProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = Convert.ToInt32(gridProducts.DataKeys[e.RowIndex].Value);

            string connStr = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM products WHERE productid = @id";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("id", productId);
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Product deleted successfully!";
            LoadProducts();
            LoadTotalStocks();
        }
    }
}
