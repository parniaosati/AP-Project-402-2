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
using System.Data.SqlClient;


namespace Login1
{
    public partial class ProfileWindow : Window
    {
        private int customerId;

        public ProfileWindow(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadProfile();
        }

        private void LoadProfile()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT fullname, phonenumber, email, username FROM Users WHERE user_id=@UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", customerId);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            FullNameTextBox.Text = reader["fullname"].ToString();
                            PhoneNumberTextBox.Text = reader["phonenumber"].ToString();
                            EmailTextBox.Text = reader["email"].ToString();
                            UsernameTextBox.Text = reader["username"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message);
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string email = EmailTextBox.Text;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Users SET fullname=@FullName, phonenumber=@PhoneNumber, email=@Email WHERE user_id=@UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@UserId", customerId);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Profile updated successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message);
            }
        }
    }
}

