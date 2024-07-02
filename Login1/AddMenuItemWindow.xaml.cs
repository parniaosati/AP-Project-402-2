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
using Microsoft.Win32;


namespace Login1
{
    public partial class AddMenuItemWindow : Window
    {
        private int restaurantId;
        private List<string> categories;

        public AddMenuItemWindow(int restaurantId, List<string> categories)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            this.categories = categories;

            CategoryComboBox.ItemsSource = categories;
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string category = CategoryComboBox.Text;
            string name = NameTextBox.Text;
            string description = DescriptionTextBox.Text;
            decimal price = decimal.Parse(PriceTextBox.Text);
            int availableQuantity = int.Parse(QuantityTextBox.Text);
            string imageUrl = ImageUrlTextBox.Text;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Menus (restaurant_id, category, name, description, price, available_quantity, image_url) " +
                                   "VALUES (@RestaurantId, @Category, @Name, @Description, @Price, @AvailableQuantity, @ImageUrl)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@AvailableQuantity", availableQuantity);
                        command.Parameters.AddWithValue("@ImageUrl", imageUrl);

                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Menu item added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding menu item: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ImageUrlTextBox.Text = openFileDialog.FileName;
                ImagePreview.Text = "Image selected: " + openFileDialog.FileName;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    ImageUrlTextBox.Text = files[0];
                    ImagePreview.Text = "Image selected: " + files[0];
                }
            }
        }
    }
}
