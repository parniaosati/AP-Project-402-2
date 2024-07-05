using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Login1
{
    public partial class ItemWindow : Window
    {
        private MenuItemm menuItem;
        private int customerId;
        public ObservableCollection<Comment> Comments { get; set; }
        public Comment SelectedComment { get; set; }
        public bool IsRatingEnabled { get; set; }

        public ItemWindow(MenuItemm menuItem, int customerId)
        {
            InitializeComponent();
            this.menuItem = menuItem;
            this.customerId = customerId;
            DataContext = this;
            LoadItemDetails();
            LoadComments();
            CheckIfUserHasRated();
        }

        private void OpenCartSummaryWindow()
        {
            var cartSummaryWindow = new CartSummaryWindow(customerId);
            cartSummaryWindow.Show();
        }

        private void LoadItemDetails()
        {
            ItemName.Text = menuItem.Name;
            ItemDescription.Text = menuItem.Description;
            ItemPrice.Text = $"Price: ${menuItem.Price}";
            ItemQuantity.Text = $"Available Quantity: {menuItem.AvailableQuantity}";

            if (!string.IsNullOrEmpty(menuItem.ImageUrl))
            {
                ItemImage.Source = new BitmapImage(new Uri(menuItem.ImageUrl, UriKind.RelativeOrAbsolute));
            }

            CalculateAverageRating();
        }

        private void CalculateAverageRating()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT AVG(CAST(rating AS FLOAT)) AS AverageRating FROM Reviews WHERE menu_id = @MenuId AND is_rating = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MenuId", menuItem.MenuId);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            double averageRating = reader.GetDouble(0);
                            AverageRating.Text = $"Average Rating: {averageRating:F1}";
                        }
                        else
                        {
                            AverageRating.Text = "Average Rating: N/A";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating average rating: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void LoadComments()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            R.review_id,
                            R.user_id,
                            R.comment, 
                            R.created_at, 
                            R.is_edited,
                            U.username, 
                            (SELECT TOP 1 rating FROM Reviews WHERE user_id = R.user_id AND menu_id = @MenuId AND is_rating = 1 ORDER BY created_at DESC) AS user_rating
                        FROM 
                            Reviews R
                        JOIN 
                            Users U ON R.user_id = U.user_id
                        WHERE 
                            R.menu_id = @MenuId AND R.comment IS NOT NULL
                        ORDER BY 
                            R.created_at DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MenuId", menuItem.MenuId);
                        SqlDataReader reader = command.ExecuteReader();
                        List<Comment> comments = new List<Comment>();
                        while (reader.Read())
                        {
                            int reviewId = reader.GetInt32(reader.GetOrdinal("review_id"));
                            int userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                            string userName = reader.GetString(reader.GetOrdinal("username"));
                            string commentText = reader.IsDBNull(reader.GetOrdinal("comment")) ? null : reader.GetString(reader.GetOrdinal("comment"));
                            int? rating = reader.IsDBNull(reader.GetOrdinal("user_rating")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("user_rating"));
                            DateTime commentTime = reader.GetDateTime(reader.GetOrdinal("created_at"));
                            bool isEdited = reader.IsDBNull(reader.GetOrdinal("is_edited")) ? false : reader.GetBoolean(reader.GetOrdinal("is_edited"));

                            comments.Add(new Comment
                            {
                                ReviewId = reviewId,
                                UserId = userId,
                                UserName = userName,
                                CommentText = commentText,
                                Rating = rating,
                                CommentTime = commentTime,
                                IsEdited = isEdited,
                                CanEditOrDelete = userId == customerId
                            });
                        }
                        Comments = new ObservableCollection<Comment>(comments);
                        CommentsList.ItemsSource = Comments;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading comments: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void CheckIfUserHasRated()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Reviews WHERE user_id = @UserId AND menu_id = @MenuId AND is_rating = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", customerId);
                        command.Parameters.AddWithValue("@MenuId", menuItem.MenuId);
                        int count = (int)command.ExecuteScalar();
                        IsRatingEnabled = count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking user rating status: {ex.Message}\n{ex.StackTrace}");
            }
            DataContext = null;
            DataContext = this;
        }

        private void SubmitRating_Click(object sender, RoutedEventArgs e)
        {
            if (RatingComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a rating.");
                return;
            }

            int rating = int.Parse((RatingComboBox.SelectedItem as ComboBoxItem).Content.ToString());

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Reviews (user_id, restaurant_id, menu_id, rating, comment, created_at, updated_at, is_rating) " +
                                   "VALUES (@UserId, @RestaurantId, @MenuId, @Rating, NULL, @CreatedAt, @UpdatedAt, @IsRating)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", customerId);
                        command.Parameters.AddWithValue("@RestaurantId", menuItem.RestaurantId);
                        command.Parameters.AddWithValue("@MenuId", menuItem.MenuId);
                        command.Parameters.AddWithValue("@Rating", rating);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@IsRating", 1);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Rating submitted successfully.");
                IsRatingEnabled = false;
                DataContext = null;
                DataContext = this;
                CalculateAverageRating();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting rating: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void SubmitComment_Click(object sender, RoutedEventArgs e)
        {
            string commentText = CommentTextBox.Text;

            if (string.IsNullOrEmpty(commentText))
            {
                MessageBox.Show("Please enter a comment.");
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Reviews (user_id, restaurant_id, menu_id, rating, comment, created_at, updated_at, is_rating, is_edited) " +
                                   "VALUES (@UserId, @RestaurantId, @MenuId, NULL, @Comment, @CreatedAt, @UpdatedAt, 0, 0)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", customerId);
                        command.Parameters.AddWithValue("@RestaurantId", menuItem.RestaurantId);
                        command.Parameters.AddWithValue("@MenuId", menuItem.MenuId);
                        command.Parameters.AddWithValue("@Comment", commentText);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Comment submitted successfully.");
                LoadComments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting comment: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void EditComment_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedComment == null)
            {
                MessageBox.Show("Please select a comment to edit.");
                return;
            }

            if (SelectedComment.UserId != customerId)
            {
                MessageBox.Show("You can only edit your own comments.");
                return;
            }

            string newCommentText = CommentTextBox.Text;

            if (string.IsNullOrEmpty(newCommentText))
            {
                MessageBox.Show("Please enter a new comment.");
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Reviews SET comment = @Comment, updated_at = @UpdatedAt, is_edited = 1 WHERE review_id = @ReviewId AND user_id = @UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Comment", newCommentText);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@ReviewId", SelectedComment.ReviewId);
                        command.Parameters.AddWithValue("@UserId", customerId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Comment updated successfully.");
                LoadComments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating comment: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedComment == null)
            {
                MessageBox.Show("Please select a comment to delete.");
                return;
            }

            if (SelectedComment.UserId != customerId)
            {
                MessageBox.Show("You can only delete your own comments.");
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Documents\ap.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Reviews WHERE review_id = @ReviewId AND user_id = @UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReviewId", SelectedComment.ReviewId);
                        command.Parameters.AddWithValue("@UserId", customerId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Comment deleted successfully.");
                LoadComments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting comment: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputtDialog($"Enter quantity to add to cart (Available: {menuItem.AvailableQuantity}):");
            if (inputDialog.ShowDialog() == true)
            {
                int quantity;
                if (!int.TryParse(inputDialog.ResponseText, out quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                if (quantity > menuItem.AvailableQuantity)
                {
                    MessageBox.Show("The quantity you entered exceeds the available quantity.");
                    return;
                }

                CartManager.Instance.AddToCart(new CartItem
                {
                    CustomerId = customerId, // Ensure customerId is set correctly
                    MenuId = menuItem.MenuId,
                    Name = menuItem.Name,
                    Price = menuItem.Price,
                    Quantity = quantity,
                    RestaurantId = menuItem.RestaurantId,
                    RestaurantName = "Restaurant Name" // Fetch or pass the actual restaurant name
                });

                MessageBox.Show("Item added to cart.");
                OpenCartSummaryWindow();
            }
        }
    }

    public class Comment
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public int? Rating { get; set; }
        public DateTime CommentTime { get; set; }
        public bool IsEdited { get; set; }
        public bool CanEditOrDelete { get; set; }
    }
}
