using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace Login1
{
    public partial class OrderHistoryWindow : Window
    {
        private int customerId;

        public OrderHistoryWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadOrderHistory();
        }

        private void LoadOrderHistory()
        {
            List<OrderHistoryItem> orderHistory = new List<OrderHistoryItem>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT o.order_id, r.name AS RestaurantName, o.order_date, o.total_amount, o.status
                        FROM Orders o
                        JOIN Restaurants r ON o.restaurant_id = r.restaurant_id
                        WHERE o.customer_id = @CustomerId
                        ORDER BY o.order_date DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            orderHistory.Add(new OrderHistoryItem
                            {
                                OrderId = (int)reader["order_id"],
                                RestaurantName = reader["RestaurantName"].ToString(),
                                OrderDate = (DateTime)reader["order_date"],
                                TotalAmount = (decimal)reader["total_amount"],
                                Status = reader["status"].ToString()
                            });
                        }
                    }
                }

                OrderHistoryListView.ItemsSource = orderHistory;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order history: " + ex.Message);
            }
        }
    }

    public class OrderHistoryItem
    {
        public int OrderId { get; set; }
        public string RestaurantName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}

