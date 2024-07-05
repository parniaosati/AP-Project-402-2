using System;
using System.Data.SqlClient;
using System.Windows;

namespace Login1
{
    public partial class ReservationSystemWindow : Window
    {
        private int restaurantId;

        public ReservationSystemWindow(int restaurantId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            LoadRestaurantRating();
        }

        private void LoadRestaurantRating()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            COALESCE((
                                SELECT AVG(CAST(Rv.rating AS FLOAT)) 
                                FROM Reviews Rv 
                                WHERE Rv.restaurant_id = @RestaurantId AND Rv.rating IS NOT NULL
                            ), 0) AS OrderRating,
                            COALESCE((
                                SELECT AVG(CAST(Mr.rating AS FLOAT)) 
                                FROM Reviews Mr 
                                JOIN Orders O ON Mr.order_id = O.order_id
                                WHERE O.restaurant_id = @RestaurantId AND Mr.rating IS NOT NULL
                            ), 0) AS MenuRating,
                            (
                                SELECT COUNT(*)
                                FROM Reviews Rv
                                WHERE Rv.restaurant_id = @RestaurantId AND Rv.rating IS NOT NULL
                            ) AS OrderRatingCount,
                            (
                                SELECT COUNT(*)
                                FROM Reviews Mr
                                JOIN Orders O ON Mr.order_id = O.order_id
                                WHERE O.restaurant_id = @RestaurantId AND Mr.rating IS NOT NULL
                            ) AS MenuRatingCount
                        FROM Restaurants R
                        WHERE R.restaurant_id = @RestaurantId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            double orderRating = reader.GetDouble(reader.GetOrdinal("OrderRating"));
                            double menuRating = reader.GetDouble(reader.GetOrdinal("MenuRating"));
                            int orderRatingCount = reader.GetInt32(reader.GetOrdinal("OrderRatingCount"));
                            int menuRatingCount = reader.GetInt32(reader.GetOrdinal("MenuRatingCount"));

                            double overallRating = (orderRating * orderRatingCount + menuRating * menuRatingCount) / (orderRatingCount + menuRatingCount);

                            RatingTextBlock.Text = $"Your Rating: {overallRating:F1}";

                            if (overallRating >= 4.5)
                            {
                                EnableReservationButton.IsEnabled = true;
                            }
                            else
                            {
                                EnableReservationButton.IsEnabled = false;
                                MessageBox.Show("Your rating is too low to enable the reservation system. You need a rating of 4.5 or higher.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating rating: {ex.Message}");
            }
        }

        private void EnableReservationButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Restaurants SET reservation_system = 1 WHERE restaurant_id = @RestaurantId";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Reservation system enabled successfully.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enabling reservation system: {ex.Message}");
            }
        }
    }
}
