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
using System.Data;
using System.Data.SqlClient;





namespace Login1
    {
        /// <summary>
        /// Interaction logic for PasswordWindow.xaml
        /// </summary>
        public partial class PasswordWindow : Window
        {
            private string verificationCode;

            public PasswordWindow(string verificationCode)
            {
                InitializeComponent();
                this.verificationCode = verificationCode;
            }

            private void btnSubmit_Click(object sender, RoutedEventArgs e)
            {
                string password1 = pass1.Password;
                string password2 = pass2.Password;

                if (password1 == password2)
                {
                    SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30");
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();

                        // Update the password for the user with the matching verification code
                        String query = "UPDATE Users SET password=@Password WHERE verificationcode=@VerificationCode";
                        SqlCommand sqlCmd = new SqlCommand(query, connection);
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("@Password", password1); // Make sure to hash the password before storing
                        sqlCmd.Parameters.AddWithValue("@VerificationCode", verificationCode);

                        int rowsAffected = sqlCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password set successfully.");
                            Login login = new Login();
                            login.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to set password.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Passwords do not match.");
                }
            }
        }
    }
