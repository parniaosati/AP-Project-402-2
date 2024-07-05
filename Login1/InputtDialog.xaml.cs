using System.Windows;

namespace Login1
{
    public partial class InputtDialog : Window
    {
        public string ResponseText { get; private set; }

        public InputtDialog(string prompt)
        {
            InitializeComponent();
            PromptText.Text = prompt;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ResponseText = InputTextBox.Text;
            DialogResult = true;
            Close();
        }
    }
}
