using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Login1
{
    public partial class RestaurantMenuWindow : Window
    {
        private int restaurantId;
        private int customerId;
        public ObservableCollection<string> Categories { get; set; }

        public RestaurantMenuWindow(int restaurantId, int customerId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            this.customerId = customerId;
            DataContext = this;
            LoadCategories();
        }

        private void LoadCategories()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT category FROM Menus WHERE restaurant_id = @RestaurantId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reader = command.ExecuteReader();
                        List<string> categories = new List<string>();
                        while (reader.Read())
                        {
                            categories.Add(reader["category"].ToString());
                        }
                        Categories = new ObservableCollection<string>(categories);
                        CategoryItemsControl.ItemsSource = Categories;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void LoadMenuItems(string category)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Menus WHERE restaurant_id = @RestaurantId AND category = @Category";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("@Category", category);
                        SqlDataReader reader = command.ExecuteReader();
                        MenuItemsPanel.Children.Clear();
                        while (reader.Read())
                        {
                            var menuItem = new MenuItemm
                            {
                                MenuId = (int)reader["menu_id"],
                                RestaurantId = (int)reader["restaurant_id"], // Ensure restaurant_id is set
                                Name = reader["name"].ToString(),
                                Category = reader["category"].ToString(),
                                Description = reader["description"].ToString(),
                                Price = (decimal)reader["price"],
                                AvailableQuantity = (int)reader["available_quantity"],
                                ImageUrl = reader["image_url"].ToString()
                            };
                            AddMenuItemToPanel(menuItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading menu items: " + ex.Message);
            }
        }


        private void AddMenuItemToPanel(MenuItemm menuItem)
        {
            var stackPanel = new StackPanel
            {
                Width = 200,
                Margin = new Thickness(10)
            };

            var image = new Image
            {
                Height = 150,
                Margin = new Thickness(0, 0, 0, 10)
            };
            if (!string.IsNullOrEmpty(menuItem.ImageUrl))
            {
                image.Source = new BitmapImage(new Uri(menuItem.ImageUrl, UriKind.RelativeOrAbsolute));
            }

            var nameTextBlock = new TextBlock
            {
                Text = menuItem.Name,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var descriptionTextBlock = new TextBlock
            {
                Text = menuItem.Description,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var priceTextBlock = new TextBlock
            {
                Text = $"${menuItem.Price}",
                Margin = new Thickness(0, 0, 0, 10)
            };

            var itemButton = new Button
            {
                Content = "View Item",
                Margin = new Thickness(0, 0, 0, 10),
                CommandParameter = menuItem
            };
            itemButton.Click += ItemButton_Click;

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(nameTextBlock);
            stackPanel.Children.Add(descriptionTextBlock);
            stackPanel.Children.Add(priceTextBlock);
            stackPanel.Children.Add(itemButton);

            MenuItemsPanel.Children.Add(stackPanel);
        }

        private void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var menuItem = button.CommandParameter as MenuItemm;
            var itemWindow = new ItemWindow(menuItem, customerId);
            itemWindow.Show();
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedCategory = (sender as Button).CommandParameter.ToString();
            LoadMenuItems(selectedCategory);
        }
    }
}
