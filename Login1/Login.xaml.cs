using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace Login1
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnSubmit_clicked(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30");
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                String query = @"
                    SELECT u.user_id, u.usertype, u.phonenumber, u.fullname, r.name as restaurantName, r.restaurant_id
                    FROM Users u
                    LEFT JOIN Restaurants r ON u.user_id = r.user_id
                    WHERE u.username = @Username AND u.password = @Password";

                SqlCommand sqlCmd = new SqlCommand(query, connection);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                SqlDataReader reader = sqlCmd.ExecuteReader();
                if (reader.Read())
                {
                    int userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                    string userType = reader["usertype"].ToString();
                    string phonenumber = reader["phonenumber"] != DBNull.Value ? reader["phonenumber"].ToString() : string.Empty;
                    string fullname = reader["fullname"] != DBNull.Value ? reader["fullname"].ToString() : string.Empty;
                    string restaurantName = reader["restaurantName"] != DBNull.Value ? reader["restaurantName"].ToString() : string.Empty;

                    if (userType.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                    {
                        CustomerPanel customerPanel = new CustomerPanel(userId);
                        customerPanel.Show();
                        this.Close();
                    }
                    else if (userType.Equals("restaurant", StringComparison.OrdinalIgnoreCase))
                    {
                        RPanel restaurantPanel = new RPanel(restaurantName, reader.GetInt32(reader.GetOrdinal("restaurant_id")));
                        restaurantPanel.Show();
                        this.Close();
                    }
                    else if (userType.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        APanel adminPanel = new APanel();
                        adminPanel.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unknown user type.");
                    }
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect.");
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

        private void register_clicked(object sender, RoutedEventArgs e)
        {
            Register r = new Register();
            r.Show();
            this.Close();
        }
    }
}
