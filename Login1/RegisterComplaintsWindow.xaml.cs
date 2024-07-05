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
    public partial class RegisterComplaintsWindow : Window
    {
        private int customerId;

        public RegisterComplaintsWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            List<RestaurantDetails> restaurants = new List<RestaurantDetails>();
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
                            restaurants.Add(new RestaurantDetails
                            {
                                RestaurantId = Convert.ToInt32(reader["restaurant_id"]),
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

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (RestaurantComboBox.SelectedItem != null && !string.IsNullOrEmpty(TitleTextBox.Text) && !string.IsNullOrEmpty(DescriptionTextBox.Text))
            {
                int restaurantId = (int)RestaurantComboBox.SelectedValue;
                string title = TitleTextBox.Text;
                string description = DescriptionTextBox.Text;
                RegisterComplaint(restaurantId, title, description);
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private void RegisterComplaint(int restaurantId, string title, string description)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Complaints (user_id, restaurant_id, title, description, status, created_at, updated_at) VALUES (@UserId, @RestaurantId, @Title, @Description, 'pending', GETDATE(), GETDATE())";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", customerId);
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Description", description);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Complaint registered successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering complaint: " + ex.Message);
            }
        }
    }

    public class RestaurantDetails
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
    }
}
