using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Login1
{
    public partial class RPanel : Window
    {
        private string restaurantName;
        private int restaurantId;

        public RPanel(string restaurantName, int restaurantId)
        {
            InitializeComponent();
            this.restaurantName = restaurantName;
            this.restaurantId = restaurantId;
            lblPanelTitle.Content = $"{restaurantName} Panel";
        }

        private void ShowMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMenuWindow showMenuWindow = new ShowMenuWindow(restaurantId);
            showMenuWindow.Show();
            this.Close();
        }

        private void ChangeMenuButton_Clicked(object sender, RoutedEventArgs e)
        {
            category categoryWindow = new category(restaurantName, restaurantId);
            categoryWindow.Show();
            this.Close();
        }

        private void ChangeAvailableFoodButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAvailableFoodWindow changeAvailableFoodWindow = new ChangeAvailableFoodWindow(restaurantId);
            changeAvailableFoodWindow.Show();
            this.Close();
        }

        private void ChangeReservationServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ReservationSystemWindow reservationSystemWindow = new ReservationSystemWindow(restaurantId);
            reservationSystemWindow.Show();
            this.Close();
        }

        private void OrdersAndReserveHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersAndReservationsWindow ordersAndReservationsWindow = new OrdersAndReservationsWindow(restaurantId);
            ordersAndReservationsWindow.Show();
            this.Close();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            changepassword changePasswordWindow = new changepassword();
            changePasswordWindow.Show();
            this.Close();
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
