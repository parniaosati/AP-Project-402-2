using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace Login1
    {
        /// <summary>
        /// Interaction logic for CheckVerification.xaml
        /// </summary>
        public partial class CheckVerification : Window
        {
            public CheckVerification()
            {
                InitializeComponent();
            }

            private void btnSubmit_Click(object sender, RoutedEventArgs e)
            {
                string enteredCode = VerificationCode.Text; // Assuming there's a TextBox for the verification code

                SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30");
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    String query = "SELECT verificationcode FROM Users WHERE verificationcode=@VerificationCode";
                    SqlCommand sqlCmd = new SqlCommand(query, connection);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@VerificationCode", enteredCode);

                    object result = sqlCmd.ExecuteScalar();

                    if (result != null && result.ToString() == enteredCode)
                    {
                        // If the verification code matches, open the Password window
                        PasswordWindow passwordWindow = new PasswordWindow(enteredCode);
                        passwordWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wrong verification code.");
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
        }
    }
