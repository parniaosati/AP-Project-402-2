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
    public partial class ChangeReservationServiceWindow : Window
    {
        private int restaurantId;

        public ChangeReservationServiceWindow(int restaurantId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            LoadReservationStatus();
        }

        private void LoadReservationStatus()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT is_reservation_active FROM Restaurants WHERE restaurant_id=@RestaurantId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            ActivateReservationsCheckBox.IsChecked = Convert.ToBoolean(reader["is_reservation_active"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reservation status: " + ex.Message);
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            bool isActive = ActivateReservationsCheckBox.IsChecked == true;
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Restaurants SET is_reservation_active=@IsActive WHERE restaurant_id=@RestaurantId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsActive", isActive);
                        command.Parameters.AddWithValue("@RestaurantId", restaurantId);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Reservation status updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating reservation status: " + ex.Message);
            }
        }
    }
}
