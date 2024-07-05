using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Login1
{
    public partial class CartSummaryWindow : Window
    {
        public ObservableCollection<CartItem> CartItems { get; set; }
        private int customerId;

        public CartSummaryWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            CartItems = CartManager.Instance.GetCartItems();
            CartListView.ItemsSource = CartItems;
        }

        private void CompletePurchase_Click(object sender, RoutedEventArgs e)
        {
            var paymentDialog = new PaymentMethodDialog();
            if (paymentDialog.ShowDialog() == true)
            {
                if (paymentDialog.SelectedPaymentMethod == "Cash")
                {
                    if (CompleteOrder("Cash"))
                    {
                        MessageBox.Show("Order completed.");
                        CartManager.Instance.ClearCart();  // Clear the cart after purchase
                    }
                }
                else if (paymentDialog.SelectedPaymentMethod == "Credit Card")
                {
                    if (CompleteOrder("Credit Card"))
                    {
                        if (SendOrderConfirmationEmail(CartItems))
                        {
                            MessageBox.Show("Order completed and email sent.");
                        }
                        else
                        {
                            MessageBox.Show("Order completed, but failed to send email.");
                        }
                        CartManager.Instance.ClearCart();  // Clear the cart after purchase
                    }
                }
            }
        }

        private bool CompleteOrder(string paymentMethod)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in CartItems)
                            {
                                // Validate that the customer ID exists in the Users table
                                if (!IsCustomerIdValid(this.customerId, connection, transaction))
                                {
                                    MessageBox.Show($"Customer ID {this.customerId} does not exist.");
                                    transaction.Rollback();
                                    return false;
                                }

                                // First, retrieve the price for the menu item
                                decimal price = 0;
                                string priceQuery = "SELECT price FROM Menus WHERE menu_id = @MenuItemId";
                                using (SqlCommand priceCommand = new SqlCommand(priceQuery, connection, transaction))
                                {
                                    priceCommand.Parameters.AddWithValue("@MenuItemId", item.MenuId);
                                    price = (decimal)priceCommand.ExecuteScalar();
                                }

                                // Calculate the total amount for this item
                                decimal totalAmount = price * item.Quantity;

                                // Insert the order into the Orders table
                                string orderQuery = @"
                                    INSERT INTO Orders (customer_id, restaurant_id, order_date, total_amount, status, payment_method)
                                    VALUES (@CustomerId, @RestaurantId, @OrderDate, @TotalAmount, @Status, @PaymentMethod);
                                ";
                                using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection, transaction))
                                {
                                    orderCommand.Parameters.AddWithValue("@CustomerId", this.customerId);
                                    orderCommand.Parameters.AddWithValue("@RestaurantId", item.RestaurantId);
                                    orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                                    orderCommand.Parameters.AddWithValue("@TotalAmount", totalAmount);
                                    orderCommand.Parameters.AddWithValue("@Status", "Completed");
                                    orderCommand.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                                    orderCommand.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Error completing order: {ex.Message}\n{ex.StackTrace}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        private bool IsCustomerIdValid(int customerId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE user_id = @CustomerId";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private bool SendOrderConfirmationEmail(ObservableCollection<CartItem> cartItems)
        {
            try
            {
                string email = GetUserEmail(cartItems[0].CustomerId); // Assume all items have the same customer ID
                if (string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("User email is not found or invalid.");
                    return false;
                }

                string emailBody = "Your order has been placed successfully. Here are the details:\n\n";

                foreach (var item in cartItems)
                {
                    emailBody += $"Item: {item.Name}\nQuantity: {item.Quantity}\nPrice: {item.Price}\n\n";
                }

                emailBody += "Thank you for your order!";

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ap.project.402.osati.riazi@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Order Confirmation";
                mail.Body = emailBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("ap.project.402.osati.riazi@gmail.com", "hjgquxvdhnvxphce");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show("SMTP error: " + smtpEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("General error: " + ex.Message);
                return false;
            }
        }

        private string GetUserEmail(int customerId)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            string email = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT email FROM Users WHERE user_id = @CustomerId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        email = (string)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user email: " + ex.Message);
            }

            return email;
        }
    }

    public class CartItem
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string RestaurantName { get; set; }
        public decimal Total => Price * Quantity;
    }

    public class CartManager
    {
        private static readonly CartManager _instance = new CartManager();
        public static CartManager Instance => _instance;

        private ObservableCollection<CartItem> _cartItems;

        private CartManager()
        {
            _cartItems = new ObservableCollection<CartItem>();
        }

        public void AddToCart(CartItem item)
        {
            _cartItems.Add(item);
        }

        public ObservableCollection<CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}
