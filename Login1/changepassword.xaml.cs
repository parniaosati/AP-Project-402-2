using System;
using System.Data.SqlClient;
using System.Windows;

namespace Login1
{
    /// <summary>
    /// Interaction logic for changepassword.xaml
    /// </summary>
    public partial class changepassword : Window
    {
        public changepassword()
        {
            InitializeComponent();
        }

            private void btnSave_Click(object sender, RoutedEventArgs e)
            {
                string username = Username.Text;
                string currentPassword = cpass.Password;
                string newPassword = npass.Password;
                string confirmNewPassword = cnpass.Password;

                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the username and current password match
                        string query = "SELECT COUNT(*) FROM Users WHERE username=@Username AND password=@Password";
                        SqlCommand sqlCmd = new SqlCommand(query, connection);
                        sqlCmd.Parameters.AddWithValue("@Username", username);
                        sqlCmd.Parameters.AddWithValue("@Password", currentPassword);

                        int count = (int)sqlCmd.ExecuteScalar();

                        if (count == 1)
                        {
                            // Username and current password match
                            if (newPassword == confirmNewPassword)
                            {
                                // New password and confirm new password match, update the password
                                query = "UPDATE Users SET password=@NewPassword WHERE username=@Username";
                                SqlCommand updateCmd = new SqlCommand(query, connection);
                                updateCmd.Parameters.AddWithValue("@NewPassword", newPassword); // Make sure to hash the password before storing it
                                updateCmd.Parameters.AddWithValue("@Username", username);

                                int rowsAffected = updateCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Password changed successfully.");
                                    Login login = new Login();
                                    login.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to change password.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("New passwords do not match.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Username or current password is incorrect.");
                            APanel aPanel = new APanel();
                            aPanel.Show();
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }
    }
