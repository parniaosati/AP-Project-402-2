using System;
using System.Windows;

namespace Login1
{
    /// <summary>
    /// Interaction logic for APanel.xaml
    /// </summary>
    public partial class APanel : Window
    {
        public APanel()
        {
            InitializeComponent();
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

        private void rr_clicked(object sender, RoutedEventArgs e)
        {
            addrestaurant addRestaurantWindow = new addrestaurant();
            addRestaurantWindow.Show();
            this.Close();
        }
    }
}
