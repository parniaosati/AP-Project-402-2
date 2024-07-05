using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Login1
{
    public partial class OrdersAndReservationsWindow : Window
    {
        private int restaurantId;

        public OrdersAndReservationsWindow(int restaurantId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            LoadOrdersAndReservations();
        }

        private void LoadOrdersAndReservations()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string orderQuery = "SELECT order_id, order_date, total_amount, status FROM Orders WHERE restaurant_id = @RestaurantId";
                    string reservationQuery = "SELECT reservation_id, reservation_date, reservation_time, status FROM Reservations WHERE restaurant_id = @RestaurantId";

                    using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection))
                    {
                        orderCommand.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader orderReader = orderCommand.ExecuteReader();
                        List<OrderDetail> orders = new List<OrderDetail>();
                        while (orderReader.Read())
                        {
                            orders.Add(new OrderDetail
                            {
                                OrderId = (int)orderReader["order_id"],
                                OrderDate = (DateTime)orderReader["order_date"],
                                TotalAmount = (decimal)orderReader["total_amount"],
                                Status = orderReader["status"].ToString()
                            });
                        }
                        orderReader.Close();
                        OrdersListView.ItemsSource = orders;
                    }

                    using (SqlCommand reservationCommand = new SqlCommand(reservationQuery, connection))
                    {
                        reservationCommand.Parameters.AddWithValue("@RestaurantId", restaurantId);
                        SqlDataReader reservationReader = reservationCommand.ExecuteReader();
                        List<ReservationDetail> reservations = new List<ReservationDetail>();
                        while (reservationReader.Read())
                        {
                            reservations.Add(new ReservationDetail
                            {
                                ReservationId = (int)reservationReader["reservation_id"],
                                ReservationDate = (DateTime)reservationReader["reservation_date"],
                                ReservationTime = (int)reservationReader["reservation_time"],
                                Status = reservationReader["status"].ToString()
                            });
                        }
                        reservationReader.Close();
                        ReservationsListView.ItemsSource = reservations;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders and reservations: " + ex.Message);
            }
        }
    }

    public class OrderDetail
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }

    public class ReservationDetail
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ReservationTime { get; set; }
        public string Status { get; set; }
    }
}
