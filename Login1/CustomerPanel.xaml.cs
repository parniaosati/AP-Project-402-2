using System.Windows;

namespace Login1
{
    public partial class CustomerPanel : Window
    {
        private int customerId;

        public CustomerPanel(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            lblPanelTitle.Content = $"Customer Panel";
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(customerId);
            profileWindow.Show();
        }

        private void SearchRestaurantsButton_Click(object sender, RoutedEventArgs e)
        {
            SearchRestaurantsWindow searchRestaurantsWindow = new SearchRestaurantsWindow(customerId);
            searchRestaurantsWindow.Show();
        }

        private void OrderFoodButton_Click(object sender, RoutedEventArgs e)
        {
            OrderFoodWindow orderFoodWindow = new OrderFoodWindow(customerId);
            orderFoodWindow.Show();
        }

        private void OrderHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            OrderHistoryWindow orderHistoryWindow = new OrderHistoryWindow(customerId);
            orderHistoryWindow.Show();
        }

        private void RegisterComplaintsButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterComplaintsWindow registerComplaintsWindow = new RegisterComplaintsWindow(customerId);
            registerComplaintsWindow.Show();
        }

        private void SpecialServicesButton_Click(object sender, RoutedEventArgs e)
        {
            SpecialServicesWindow specialServicesWindow = new SpecialServicesWindow(customerId);
            specialServicesWindow.Show();
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}
