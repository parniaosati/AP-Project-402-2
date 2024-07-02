using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Login1
{
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = fullname.Text;
            string phoneNumber = phonenumber.Text;
            string username = Username.Text;
            string email = Email.Text;
            string verificationCode = GenerateVerificationCode();

            if (IsUserDataUnique(username, phoneNumber))
            {
                if (InsertUserData(fullName, phoneNumber, username, email, verificationCode))
                {
                    if (SendVerificationEmail(email, verificationCode))
                    {
                        MessageBox.Show("Registration successful! A verification code has been sent to your email.");
                        CheckVerification c = new CheckVerification();
                        c.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to send verification email.");
                    }
                }
                else
                {
                    MessageBox.Show("Registration failed.");
                }
            }
            else
            {
                MessageBox.Show("Username or phone number is already taken.");
            }
        }

        private bool IsUserDataUnique(string username, string phoneNumber)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT COUNT(*) FROM Users WHERE username=@Username OR phonenumber=@PhoneNumber";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                        int count = (int)command.ExecuteScalar();
                        return count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
                return false;
            }
        }

        private bool InsertUserData(string fullName, string phoneNumber, string username, string email, string verificationCode)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO Users (fullname, phonenumber, username, email,  usertype, created_at, updated_at, verificationcode) VALUES (@FullName, @PhoneNumber, @Username, @Email, @UserType, GETDATE(), GETDATE(), @VerificationCode)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@UserType", "Customer");
                        command.Parameters.AddWithValue("@VerificationCode", verificationCode);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
                return false;
            }
        }

        private bool SendVerificationEmail(string email, string verificationCode)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ap.project.402.osati.riazi@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Verification Code";
                mail.Body = "Your verification code is: " + verificationCode;

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

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
