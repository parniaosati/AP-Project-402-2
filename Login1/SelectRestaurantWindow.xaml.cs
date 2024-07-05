using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Login1
{
    public partial class SelectRestaurantWindow : Window
    {
        private int userId;

        public SelectRestaurantWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT restaurant_id, name, city, type FROM Restaurants";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        List<RestaurantModel> restaurants = new List<RestaurantModel>();
                        while (reader.Read())
                        {
                            restaurants.Add(new RestaurantModel
                            {
                                RestaurantId = (int)reader["restaurant_id"],
                                Name = reader["name"].ToString(),
                                City = reader["city"].ToString(),
                                Type = reader["type"].ToString()
                            });
                        }

                        RestaurantListView.ItemsSource = restaurants;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading restaurants: {ex.Message}");
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RestaurantListView.SelectedItem is RestaurantModel selectedRestaurant)
            {
                MakeReservationWindow makeReservationWindow = new MakeReservationWindow(userId, selectedRestaurant.RestaurantId);
                makeReservationWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a restaurant.");
            }
        }
    }

    public class RestaurantModel
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
    }
}
