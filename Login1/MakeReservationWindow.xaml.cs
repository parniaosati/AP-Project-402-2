using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Login1
{
    public partial class MakeReservationWindow : Window
    {
        private int userId;
        private int restaurantId;

        public MakeReservationWindow(int userId, int restaurantId)
        {
            InitializeComponent();
            this.userId = userId;
            this.restaurantId = restaurantId;
            LoadUserServices();
            LoadTimeSlots();
        }

        private void LoadUserServices()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT US.user_service_id, SS.name
                        FROM UserServices US
                        JOIN SpecialServices SS ON US.service_id = SS.service_id
                        WHERE US.user_id = @UserId AND US.expiration_date > GETDATE()";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        SqlDataReader reader = command.ExecuteReader();
                        List<UserService> services = new List<UserService>();
                        while (reader.Read())
                        {
                            services.Add(new UserService
                            {
                                UserServiceId = (int)reader["user_service_id"],
                                Name = reader["name"].ToString()
                            });
                        }

                        ServiceComboBox.ItemsSource = services;
                        ServiceComboBox.DisplayMemberPath = "Name";
                        ServiceComboBox.SelectedValuePath = "UserServiceId";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading services: {ex.Message}");
            }
        }

        private void LoadTimeSlots()
        {
            var timeSlots = new List<string>
            {
                "08:00 AM", "09:00 AM", "10:00 AM", "11:00 AM",
                "12:00 PM", "01:00 PM", "02:00 PM", "03:00 PM",
                "04:00 PM", "05:00 PM", "06:00 PM", "07:00 PM",
                "08:00 PM", "09:00 PM", "10:00 PM"
            };

            ReservationTimeComboBox.ItemsSource = timeSlots;
        }

        private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceComboBox.SelectedItem is UserService selectedService &&
                ReservationDatePicker.SelectedDate.HasValue &&
                ReservationTimeComboBox.SelectedItem is string selectedTime)
            {
                DateTime reservationDate = ReservationDatePicker.SelectedDate.Value;
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"
                            SELECT 
                                COUNT(*) 
                            FROM Reservations 
                            WHERE customer_id = @UserId AND status = 'pending' AND 
                                  MONTH(reservation_date) = MONTH(@ReservationDate) AND 
                                  YEAR(reservation_date) = YEAR(@ReservationDate)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@ReservationDate", reservationDate);
                            int reservationCount = (int)command.ExecuteScalar();

                            if (reservationCount >= GetMaxReservations(selectedService.UserServiceId))
                            {
                                MessageBox.Show("You have reached the maximum number of reservations for this month.");
                                return;
                            }
                        }

                        string insertQuery = @"
                            INSERT INTO Reservations (customer_id, restaurant_id, reservation_date, reservation_time, status, service_type)
                            VALUES (@UserId, @RestaurantId, @ReservationDate, @ReservationTime, 'pending', @ServiceType)";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@UserId", userId);
                            insertCommand.Parameters.AddWithValue("@RestaurantId", restaurantId);
                            insertCommand.Parameters.AddWithValue("@ReservationDate", reservationDate);
                            insertCommand.Parameters.AddWithValue("@ReservationTime", GetMinutesFromTime(selectedTime));
                            insertCommand.Parameters.AddWithValue("@ServiceType", selectedService.Name);
                            insertCommand.ExecuteNonQuery();

                            MessageBox.Show("Reservation made successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error making reservation: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a service, date, and time for the reservation.");
            }
        }

        private int GetMaxReservations(int userServiceId)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SS.reservations_per_month FROM UserServices US JOIN SpecialServices SS ON US.service_id = SS.service_id WHERE US.user_service_id = @UserServiceId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserServiceId", userServiceId);
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting max reservations: {ex.Message}");
                return 0;
            }
        }

        private int GetMinutesFromTime(string time)
        {
            DateTime dateTime = DateTime.ParseExact(time, "hh:mm tt", null);
            return dateTime.Hour * 60 + dateTime.Minute;
        }
    }

    public class UserService
    {
        public int UserServiceId { get; set; }
        public string Name { get; set; }
    }
}
