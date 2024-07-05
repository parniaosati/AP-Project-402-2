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
        /// <summary>
        /// Interaction logic for CategoryWindow.xaml
        /// </summary>
        public partial class category : Window
        {
            private string restaurantName;
            private int restaurantId;
            private List<string> categories;

            public category(string restaurantName, int restaurantId)
            {
                InitializeComponent();
                this.restaurantName = restaurantName;
                this.restaurantId = restaurantId;
                lblPanelTitle.Text = $"{restaurantName} Categories";
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
                    CategoryListBox.ItemsSource = categories;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message);
                }
            }

            private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
            {
                InputDialog inputDialog = new InputDialog();
                if (inputDialog.ShowDialog() == true)
                {
                    string newCategory = inputDialog.ResponseText;
                    if (!string.IsNullOrEmpty(newCategory) && !categories.Contains(newCategory))
                    {
                        categories.Add(newCategory);
                        CategoryListBox.ItemsSource = null;
                        CategoryListBox.ItemsSource = categories;
                        MessageBox.Show("Category added successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Category name is invalid or already exists.");
                    }
                }
            }

            private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
            {
                if (CategoryListBox.SelectedItem != null)
                {
                    string selectedCategory = CategoryListBox.SelectedItem.ToString();
                    categories.Remove(selectedCategory);
                    CategoryListBox.ItemsSource = null;
                    CategoryListBox.ItemsSource = categories;
                    MessageBox.Show("Category removed successfully.");
                }
                else
                {
                    MessageBox.Show("Please select a category to remove.");
                }
            }

            private void AddMenuItemButton_Click(object sender, RoutedEventArgs e)
            {
                AddMenuItemWindow addMenuItemWindow = new AddMenuItemWindow(restaurantId, categories);
                addMenuItemWindow.ShowDialog();
                LoadCategories(); // Reload categories to reflect any new items added
            }
        
        private void BackToPanelButton_Click(object sender, RoutedEventArgs e)
        {
            RPanel rPanel = new RPanel(restaurantName, restaurantId);
            rPanel.Show();
            this.Close();
        }

    }
}
