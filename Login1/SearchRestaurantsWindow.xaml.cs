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
    public partial class SearchRestaurantsWindow : Window
    {
        private int customerId;

        public SearchRestaurantsWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
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
                    string query = "SELECT name, city, type, average_rating FROM Restaurants";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        List<Restaurant> restaurants = new List<Restaurant>();
                        while (reader.Read())
                        {
                            restaurants.Add(new Restaurant
                            {
                                Name = reader["name"].ToString(),
                                City = reader["city"].ToString(),
                                Type = reader["type"].ToString(),
                                Rating = reader["average_rating"] != DBNull.Value ? reader["average_rating"].ToString() : "No rating"
                            });
                        }

                        RestaurantList.ItemsSource = restaurants;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading restaurants: " + ex.Message);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string city = CityTextBox.Text;
            string type = TypeTextBox.Text;
            string ratingText = RatingTextBox.Text;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT name, city, type, average_rating FROM Restaurants WHERE city LIKE @City AND type LIKE @Type AND average_rating >= @Rating";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@City", $"%{city}%");
                        command.Parameters.AddWithValue("@Type", $"%{type}%");
                        command.Parameters.AddWithValue("@Rating", string.IsNullOrEmpty(ratingText) ? 0 : Convert.ToDouble(ratingText));

                        SqlDataReader reader = command.ExecuteReader();
                        List<Restaurant> restaurants = new List<Restaurant>();
                        while (reader.Read())
                        {
                            restaurants.Add(new Restaurant
                            {
                                Name = reader["name"].ToString(),
                                City = reader["city"].ToString(),
                                Type = reader["type"].ToString(),
                                Rating = reader["average_rating"] != DBNull.Value ? reader["average_rating"].ToString() : "No rating"
                            });
                        }

                        RestaurantList.ItemsSource = restaurants;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading restaurants: " + ex.Message);
            }
        }
    }

    public class Restaurant
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Rating { get; set; }
    }
}
