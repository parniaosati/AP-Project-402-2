using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

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
                    string query = @"
                SELECT R.restaurant_id, R.name, R.city, R.type, 
                    COALESCE((
                        SELECT AVG(CAST(Rv.rating AS FLOAT)) 
                        FROM Reviews Rv 
                        WHERE Rv.restaurant_id = R.restaurant_id AND Rv.rating IS NOT NULL
                    ), 0) AS OrderRating,
                    COALESCE((
                        SELECT AVG(CAST(Mr.rating AS FLOAT)) 
                        FROM Reviews Mr 
                        JOIN Orders O ON Mr.order_id = O.order_id
                        WHERE O.restaurant_id = R.restaurant_id AND Mr.rating IS NOT NULL
                    ), 0) AS MenuRating,
                    (
                        SELECT COUNT(*)
                        FROM Reviews Rv
                        WHERE Rv.restaurant_id = R.restaurant_id AND Rv.rating IS NOT NULL
                    ) AS OrderRatingCount,
                    (
                        SELECT COUNT(*)
                        FROM Reviews Mr
                        JOIN Orders O ON Mr.order_id = O.order_id
                        WHERE O.restaurant_id = R.restaurant_id AND Mr.rating IS NOT NULL
                    ) AS MenuRatingCount
                FROM Restaurants R";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        List<Restaurant> restaurants = new List<Restaurant>();
                        while (reader.Read())
                        {
                            double orderRating = reader.GetDouble(reader.GetOrdinal("OrderRating"));
                            double menuRating = reader.GetDouble(reader.GetOrdinal("MenuRating"));
                            int orderRatingCount = reader.GetInt32(reader.GetOrdinal("OrderRatingCount"));
                            int menuRatingCount = reader.GetInt32(reader.GetOrdinal("MenuRatingCount"));

                            double overallRating = (orderRating * orderRatingCount + menuRating * menuRatingCount) / (orderRatingCount + menuRatingCount);

                            restaurants.Add(new Restaurant
                            {
                                RestaurantId = (int)reader["restaurant_id"],
                                Name = reader["name"].ToString(),
                                City = reader["city"].ToString(),
                                Type = reader["type"].ToString(),
                                Rating = overallRating.ToString("F1")
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

        private void RestaurantList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantList.SelectedItem is Restaurant selectedRestaurant)
            {
                var restaurantMenuWindow = new RestaurantMenuWindow(selectedRestaurant.RestaurantId, customerId);
                restaurantMenuWindow.Show();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string city = CityTextBox.Text;
            string type = TypeTextBox.Text;
            string ratingText = SearchRatingTextBox.Text;
            double searchRating = string.IsNullOrEmpty(ratingText) ? 0 : Convert.ToDouble(ratingText);

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT R.restaurant_id, R.name, R.city, R.type, 
                            (
                                SELECT COALESCE(AVG(CAST(Rv.rating AS FLOAT)), 0)
                                FROM Reviews Rv
                                WHERE Rv.restaurant_id = R.restaurant_id AND Rv.rating IS NOT NULL
                            ) AS OrderRating,
                            (
                                SELECT COALESCE(AVG(CAST(Mr.rating AS FLOAT)), 0)
                                FROM Reviews Mr
                                WHERE Mr.menu_id IN (SELECT M.menu_id FROM Menus M WHERE M.restaurant_id = R.restaurant_id) AND Mr.rating IS NOT NULL
                            ) AS MenuRating,
                            (
                                SELECT COUNT(*)
                                FROM Reviews Rv
                                WHERE Rv.restaurant_id = R.restaurant_id AND Rv.rating IS NOT NULL
                            ) AS OrderRatingCount,
                            (
                                SELECT COUNT(*)
                                FROM Reviews Mr
                                WHERE Mr.menu_id IN (SELECT M.menu_id FROM Menus M WHERE M.restaurant_id = R.restaurant_id) AND Mr.rating IS NOT NULL
                            ) AS MenuRatingCount
                        FROM Restaurants R
                        WHERE (@City IS NULL OR R.city LIKE '%' + @City + '%') 
                          AND (@Type IS NULL OR R.type LIKE '%' + @Type + '%')";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@City", string.IsNullOrEmpty(city) ? (object)DBNull.Value : city);
                        command.Parameters.AddWithValue("@Type", string.IsNullOrEmpty(type) ? (object)DBNull.Value : type);

                        SqlDataReader reader = command.ExecuteReader();
                        List<Restaurant> restaurants = new List<Restaurant>();

                        while (reader.Read())
                        {
                            double orderRating = reader.GetDouble(reader.GetOrdinal("OrderRating"));
                            double menuRating = reader.GetDouble(reader.GetOrdinal("MenuRating"));
                            int orderRatingCount = reader.GetInt32(reader.GetOrdinal("OrderRatingCount"));
                            int menuRatingCount = reader.GetInt32(reader.GetOrdinal("MenuRatingCount"));

                            double overallRating = (orderRating * orderRatingCount + menuRating * menuRatingCount) / (orderRatingCount + menuRatingCount);

                            if (overallRating >= searchRating)
                            {
                                restaurants.Add(new Restaurant
                                {
                                    RestaurantId = (int)reader["restaurant_id"],
                                    Name = reader["name"].ToString(),
                                    City = reader["city"].ToString(),
                                    Type = reader["type"].ToString(),
                                    Rating = overallRating.ToString("F1")
                                });
                            }
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
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Rating { get; set; }
    }
}
