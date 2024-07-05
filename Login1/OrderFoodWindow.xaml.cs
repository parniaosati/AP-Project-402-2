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
    public partial class OrderFoodWindow : Window
    {
        private int customerId;
        private int selectedRestaurantId;
        private int selectedMenuItemId;

        public OrderFoodWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            List<RestaurantInfo> restaurants = new List<RestaurantInfo>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT restaurant_id, name FROM Restaurants";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            restaurants.Add(new RestaurantInfo
                            {
                                RestaurantId = (int)reader["restaurant_id"],
                                Name = reader["name"].ToString()
                            });
                        }
                    }
                }

                RestaurantComboBox.ItemsSource = restaurants;
                RestaurantComboBox.DisplayMemberPath = "Name";
                RestaurantComboBox.SelectedValuePath = "RestaurantId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading restaurants: " + ex.Message);
            }
        }

        private void RestaurantComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantComboBox.SelectedItem != null)
            {
                selectedRestaurantId = (int)RestaurantComboBox.SelectedValue;
                LoadMenuItems(selectedRestaurantId);
            }
        }

        private void LoadMenuItems(int restaurantId)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT menu_id, name, description, price, available_quantity FROM Menus WHERE restaurant_id=@RestaurantId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            menuItems.Add(new MenuItem
                            {
                                MenuId = (int)reader["menu_id"],
                                Name = reader["name"].ToString(),
                                Description = reader["description"].ToString(),
                                Price = (decimal)reader["price"],
                                AvailableQuantity = (int)reader["available_quantity"]
                            });
                        }
                    }
                }

                MenuItemsListView.ItemsSource = menuItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading menu items: " + ex.Message);
            }
        }

        private void MenuItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuItemsListView.SelectedItem != null)
            {
                selectedMenuItemId = ((MenuItem)MenuItemsListView.SelectedItem).MenuId;
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
            {
                PlaceOrder(selectedMenuItemId, quantity);
            }
            else
            {
                MessageBox.Show("Please enter a valid quantity.");
            }
        }

        private void PlaceOrder(int menuItemId, int quantity)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        DECLARE @Price DECIMAL(10,2);
                        SELECT @Price = price FROM Menus WHERE menu_id=@MenuItemId;

                        INSERT INTO Orders (customer_id, restaurant_id, order_date, total_amount, status)
                        VALUES (@CustomerId, @RestaurantId, GETDATE(), @Price * @Quantity, 'pending');

                        DECLARE @OrderId INT = SCOPE_IDENTITY();

                      
                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@RestaurantId", selectedRestaurantId);
                        command.Parameters.AddWithValue("@MenuItemId", menuItemId);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Order placed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message);
            }
        }
    }

    public class RestaurantInfo
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
    }

    public class MenuItem
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
    }
}

