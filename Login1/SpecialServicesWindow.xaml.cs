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
    public partial class SpecialServicesWindow : Window
    {
        private int customerId;
        private int selectedRestaurantId;
        private int selectedServiceId;

        public SpecialServicesWindow(int customerId)
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

        private void RestaurantComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantComboBox.SelectedItem != null)
            {
                selectedRestaurantId = (int)RestaurantComboBox.SelectedValue;
                LoadSpecialServices(selectedRestaurantId);
            }
        }

        private void LoadSpecialServices(int restaurantId)
        {
            List<SpecialService> specialServices = new List<SpecialService>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT service_id, name, description, price FROM SpecialServices WHERE restaurant_id=@RestaurantId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            specialServices.Add(new SpecialService
                            {
                                ServiceId = (int)reader["service_id"],
                                Name = reader["name"].ToString(),
                                Description = reader["description"].ToString(),
                                Price = (decimal)reader["price"]
                            });
                        }
                    }
                }

                SpecialServicesListView.ItemsSource = specialServices;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading special services: " + ex.Message);
            }
        }

        private void SpecialServicesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpecialServicesListView.SelectedItem != null)
            {
                selectedServiceId = ((SpecialService)SpecialServicesListView.SelectedItem).ServiceId;
            }
        }

        private void ReserveServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedServiceId != 0)
            {
                ReserveService(selectedServiceId);
            }
            else
            {
                MessageBox.Show("Please select a service to reserve.");
            }
        }

        private void ReserveService(int serviceId)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Reservations (customer_id, restaurant_id, service_id, reservation_date, status)
                        VALUES (@CustomerId, @RestaurantId, @ServiceId, GETDATE(), 'pending')";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@RestaurantId", selectedRestaurantId);
                        command.Parameters.AddWithValue("@ServiceId", serviceId);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Service reserved successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reserving service: " + ex.Message);
            }
        }
    }

    public class Restaurantjoz
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
    }

    public class SpecialService
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
