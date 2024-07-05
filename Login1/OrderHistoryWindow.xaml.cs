using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Login1
{
    public partial class OrderHistoryWindow : Window
    {
        private int customerId;
        public ObservableCollection<Order> Orders { get; set; }
        public Order SelectedOrder { get; set; }

        public OrderHistoryWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            Orders = new ObservableCollection<Order>();
            OrderListView.ItemsSource = Orders;
            LoadOrders();
        }

        private void LoadOrders()
        {
            Orders.Clear(); // Clear the Orders list to avoid duplicates

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT O.order_id, R.name AS restaurant_name, O.order_date, O.total_amount, O.status, O.restaurant_id,
                               COALESCE(Rev.rating, NULL) AS rating, COALESCE(Rev.comment, '') AS comment
                        FROM Orders O
                        JOIN Restaurants R ON O.restaurant_id = R.restaurant_id
                        LEFT JOIN Reviews Rev ON O.order_id = Rev.order_id
                        WHERE O.customer_id = @CustomerId
                        ORDER BY O.order_date DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Orders.Add(new Order
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("order_id")),
                                RestaurantName = reader.GetString(reader.GetOrdinal("restaurant_name")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("total_amount")),
                                Status = reader.GetString(reader.GetOrdinal("status")),
                                RestaurantId = reader.GetInt32(reader.GetOrdinal("restaurant_id")),
                                Rating = reader.IsDBNull(reader.GetOrdinal("rating")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("rating")),
                                Comment = reader.GetString(reader.GetOrdinal("comment"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void OrderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderListView.SelectedItem != null)
            {
                SelectedOrder = (Order)OrderListView.SelectedItem;
                CommentTextBox.Text = SelectedOrder.Comment;
                if (SelectedOrder.Rating.HasValue)
                {
                    RatingComboBox.SelectedItem = RatingComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == SelectedOrder.Rating.Value.ToString());
                }
                else
                {
                    RatingComboBox.SelectedItem = null;
                }
            }
        }

        private void AddCommentRating_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Please select an order to comment or rate.");
                return;
            }

            string comment = CommentTextBox.Text;
            int? rating = RatingComboBox.SelectedItem != null ? int.Parse((RatingComboBox.SelectedItem as ComboBoxItem).Content.ToString()) : (int?)null;

            if (string.IsNullOrEmpty(comment) && rating == null)
            {
                MessageBox.Show("Please enter a comment or select a rating.");
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string queryCheck = "SELECT COUNT(*) FROM Reviews WHERE user_id = @UserId AND order_id = @OrderId";
                    using (SqlCommand commandCheck = new SqlCommand(queryCheck, connection))
                    {
                        commandCheck.Parameters.AddWithValue("@UserId", customerId);
                        commandCheck.Parameters.AddWithValue("@OrderId", SelectedOrder.OrderId);
                        int reviewCount = (int)commandCheck.ExecuteScalar();

                        string query;
                        if (reviewCount > 0)
                        {
                            query = "UPDATE Reviews SET rating = @Rating, comment = @Comment, updated_at = @UpdatedAt WHERE user_id = @UserId AND order_id = @OrderId";
                        }
                        else
                        {
                            query = "INSERT INTO Reviews (user_id, restaurant_id, order_id, rating, comment, created_at, updated_at) " +
                                    "VALUES (@UserId, @RestaurantId, @OrderId, @Rating, @Comment, @CreatedAt, @UpdatedAt)";
                        }

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", customerId);
                            command.Parameters.AddWithValue("@RestaurantId", SelectedOrder.RestaurantId);
                            command.Parameters.AddWithValue("@OrderId", SelectedOrder.OrderId);
                            command.Parameters.AddWithValue("@Rating", rating.HasValue ? (object)rating.Value : DBNull.Value);
                            command.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(comment) ? (object)DBNull.Value : comment);
                            command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Comment/Rating submitted successfully.");
                CommentTextBox.Clear();
                RatingComboBox.SelectedItem = null;
                LoadOrders(); // Reload orders to update the displayed data
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting comment/rating: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string RestaurantName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int RestaurantId { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
    }
}
