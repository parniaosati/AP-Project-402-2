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
            ChangeReservationServiceWindow changeReservationServiceWindow = new ChangeReservationServiceWindow(restaurantId);
            changeReservationServiceWindow.Show();
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
