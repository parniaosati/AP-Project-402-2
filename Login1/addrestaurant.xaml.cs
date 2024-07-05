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
        public partial class addrestaurant : Window
        {
            public addrestaurant()
            {
                InitializeComponent();
            }

            private void btnRegister_Click(object sender, RoutedEventArgs e)
            {
                string username = Username.Text;
                string password = Password.Password;
                string restaurantName = RestaurantName.Text; // Updated name
                string city = City.Text;

                if (IsUsernameUnique(username))
                {
                    if (IsRestaurantNameUnique(restaurantName))
                    {
                        if (AddRestaurantUser(username, password, restaurantName, city))
                        {
                            MessageBox.Show("Restaurant added successfully.");
                            APanel a = new APanel();
                            a.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add restaurant.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Restaurant name already taken.");
                    }
                }
                else
                {
                    MessageBox.Show("Username already taken.");
                }
            }

            private bool IsUsernameUnique(string username)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
                string query = "SELECT COUNT(*) FROM Users WHERE username=@Username";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);

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

            private bool IsRestaurantNameUnique(string restaurantName)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
                string query = "SELECT COUNT(*) FROM Restaurants WHERE name=@RestaurantName";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RestaurantName", restaurantName);

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

            private bool AddRestaurantUser(string username, string password, string restaurantName, string city)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            // Insert user into Users table
                            string userQuery = "INSERT INTO Users (username, password, usertype, created_at, updated_at) VALUES (@Username, @Password, @UserType, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY();";
                            SqlCommand userCmd = new SqlCommand(userQuery, connection, transaction);
                            userCmd.Parameters.AddWithValue("@Username", username);
                            userCmd.Parameters.AddWithValue("@Password", password); // Ensure password is hashed before storing
                            userCmd.Parameters.AddWithValue("@UserType", "restaurant");

                            int userId = Convert.ToInt32(userCmd.ExecuteScalar());

                            // Insert restaurant into Restaurants table
                            string restaurantQuery = "INSERT INTO Restaurants (user_id, name, city, created_at, updated_at) VALUES (@UserId, @Name, @City, GETDATE(), GETDATE())";
                            SqlCommand restaurantCmd = new SqlCommand(restaurantQuery, connection, transaction);
                            restaurantCmd.Parameters.AddWithValue("@UserId", userId);
                            restaurantCmd.Parameters.AddWithValue("@Name", restaurantName);
                            restaurantCmd.Parameters.AddWithValue("@City", city);

                            restaurantCmd.ExecuteNonQuery();

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Database error: " + ex.Message);
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                    return false;
                }
            }
        }
    }
