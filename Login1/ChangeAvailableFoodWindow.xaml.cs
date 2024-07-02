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
    public partial class ChangeAvailableFoodWindow : Window
    {
        private int restaurantId;
        private Dictionary<string, int> items;

        public ChangeAvailableFoodWindow(int restaurantId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            LoadCategories();
        }

        private void LoadCategories()
        {
            List<string> categories = new List<string>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT category FROM Menus WHERE restaurant_id=@RestaurantId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            categories.Add(reader["category"].ToString());
                        }
                    }
                }
                CategoryComboBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem != null)
            {
                string selectedCategory = CategoryComboBox.SelectedItem.ToString();
                LoadItems(selectedCategory);
            }
        }

        private void LoadItems(string category)
        {
            items = new Dictionary<string, int>();
            ItemComboBox.ItemsSource = null;
            ItemComboBox.Items.Clear();

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT menu_id, name FROM Menus WHERE restaurant_id=@RestaurantId AND category=@Category";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("@Category", category);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            items.Add(reader["name"].ToString(), (int)reader["menu_id"]);
                        }
                    }
                }
                ItemComboBox.ItemsSource = items.Keys;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading items: " + ex.Message);
            }
        }

        private void ItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: You can load the current quantity here if needed.
        }

        private void UpdateQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemComboBox.SelectedItem != null && int.TryParse(QuantityTextBox.Text, out int newQuantity))
            {
                int menuId = items[ItemComboBox.SelectedItem.ToString()];
                UpdateItemQuantity(menuId, newQuantity);
            }
            else
            {
                MessageBox.Show("Please select an item and enter a valid quantity.");
            }
        }

        private void UpdateItemQuantity(int menuId, int newQuantity)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Menus SET available_quantity=@NewQuantity WHERE menu_id=@MenuId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                        command.Parameters.AddWithValue("@MenuId", menuId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Quantity updated successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating quantity: " + ex.Message);
            }
        }
    }
}
