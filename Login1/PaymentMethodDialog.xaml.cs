using System.Windows;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace Login1
{
    public partial class PaymentMethodDialog : Window
    {
        public string SelectedPaymentMethod { get; private set; }

        public PaymentMethodDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)PaymentMethodPanel.Children[0]).IsChecked == true)
            {
                SelectedPaymentMethod = "Cash";
            }
            else if (((RadioButton)PaymentMethodPanel.Children[1]).IsChecked == true)
            {
                SelectedPaymentMethod = "Credit Card";
            }
            else
            {
                MessageBox.Show("Please select a payment method.");
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
