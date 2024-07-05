using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Login1
{
    public partial class SpecialServicesWindow : Window
    {
        private int customerId;

        public SpecialServicesWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadSpecialServices();
        }

        private void LoadSpecialServices()
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
                        List<SpecialService> services = new List<SpecialService>();
                        while (reader.Read())
                        {
                            services.Add(new SpecialService
                            {
                                ServiceId = (int)reader["service_id"],
                                Name = reader["name"].ToString(),
                                Price = (decimal)reader["price"],
                                ReservationsPerMonth = (int)reader["reservations_per_month"],
                                ReservationTimeLimit = (int)reader["reservation_time_limit"],
                                CancellationThreshold = (int)reader["cancellation_threshold"]
                            });
                        }

                        SpecialServiceListView.ItemsSource = services;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading special services: {ex.Message}");
            }
        }
    }

    public class SpecialService
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ReservationsPerMonth { get; set; }
        public int ReservationTimeLimit { get; set; }
        public int CancellationThreshold { get; set; }
    }
}
