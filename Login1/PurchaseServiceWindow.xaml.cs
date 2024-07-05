using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Login1
{
    public partial class PurchaseServiceWindow : Window
    {
        private int userId;

        public PurchaseServiceWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadServices();
        }

        private void LoadServices()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT service_id, name, price, reservations_per_month, reservation_time_limit, cancellation_threshold FROM SpecialServices";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        List<SpecialServiceModel> services = new List<SpecialServiceModel>();
                        while (reader.Read())
                        {
                            services.Add(new SpecialServiceModel
                            {
                                ServiceId = (int)reader["service_id"],
                                Name = reader["name"].ToString(),
                                Price = (decimal)reader["price"],
                                ReservationsPerMonth = (int)reader["reservations_per_month"],
                                ReservationTimeLimit = (int)reader["reservation_time_limit"],
                                CancellationThreshold = (int)reader["cancellation_threshold"]
                            });
                        }

                        ServiceListView.ItemsSource = services;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading services: {ex.Message}");
            }
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceListView.SelectedItem is SpecialServiceModel selectedService)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"
                            INSERT INTO UserServices (user_id, service_id, purchase_date, expiration_date)
                            VALUES (@UserId, @ServiceId, GETDATE(), DATEADD(month, 1, GETDATE()))";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@ServiceId", selectedService.ServiceId);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Service purchased successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error purchasing service: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a service to purchase.");
            }
        }
    }

    public class SpecialServiceModel
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ReservationsPerMonth { get; set; }
        public int ReservationTimeLimit { get; set; }
        public int CancellationThreshold { get; set; }
    }
}
