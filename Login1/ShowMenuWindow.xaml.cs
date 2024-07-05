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
    public partial class ShowMenuWindow : Window
    {
        private int restaurantId;
        private List<string> categories;

        public ShowMenuWindow(int restaurantId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            LoadCategories();
        }

        private void LoadCategories()
        {
            categories = new List<string>();
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
                LoadMenuItems(selectedCategory);
            }
        }

        private void LoadMenuItems(string category)
        {
            MenuGrid.Children.Clear();
            MenuGrid.RowDefinitions.Clear();
            MenuGrid.ColumnDefinitions.Clear();

            MenuGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            MenuGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            MenuGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            MenuGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            MenuGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

            // Add header row
            MenuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            AddMenuGridHeader("Image", 0);
            AddMenuGridHeader("Name", 1);
            AddMenuGridHeader("Description", 2);
            AddMenuGridHeader("Price", 3);
            AddMenuGridHeader("Available Quantity", 4);

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT name, description, price, available_quantity, image_url FROM Menus WHERE restaurant_id=@RestaurantId AND category=@Category";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("@Category", category);
                        SqlDataReader reader = command.ExecuteReader();

                        int row = 1;
                        while (reader.Read())
                        {
                            string name = reader["name"].ToString();
                            string description = reader["description"].ToString();
                            decimal price = (decimal)reader["price"];
                            int availableQuantity = (int)reader["available_quantity"];
                            string imageUrl = reader["image_url"].ToString();

                            MenuGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            AddMenuGridImage(imageUrl, row, 0);
                            AddMenuGridText(name, row, 1);
                            AddMenuGridText(description, row, 2);
                            AddMenuGridText(price.ToString("C"), row, 3);
                            AddMenuGridText(availableQuantity.ToString(), row, 4);

                            row++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading menu items: " + ex.Message);
            }
        }

        private void AddMenuGridHeader(string text, int column)
        {
            TextBlock header = new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(5)
            };
            Grid.SetRow(header, 0);
            Grid.SetColumn(header, column);
            MenuGrid.Children.Add(header);
        }

        private void AddMenuGridText(string text, int row, int column)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                Margin = new Thickness(5)
            };
            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            MenuGrid.Children.Add(textBlock);
        }

        private void AddMenuGridImage(string imageUrl, int row, int column)
        {
            Image image = new Image();
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    image.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                }
                catch (Exception)
                {
                    image.Source = null;
                }
            }

            image.Width = 50;
            image.Height = 50;
            image.Margin = new Thickness(5);
            Grid.SetRow(image, row);
            Grid.SetColumn(image, column);
            MenuGrid.Children.Add(image);
        }
    }
}
