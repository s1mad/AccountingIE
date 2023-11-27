using System.Windows;
using System.Windows.Controls;

namespace AccountingIE;

public partial class RegistrationPage : Page
{
    public RegistrationPage()
    {
        InitializeComponent();
    }
    
    private bool IsValidUserData(string email, string username, string password, string password2)
    {
        if (password == password2)
        {
            return true;
        }

        return false;
    }
    private void RegistrationButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsValidUserData(EmailTextBox.Text, UsernameTextBox.Text, PasswordBox.Password, RepeatPasswordBox.Password))
        {
            NavigationService.Navigate(new AccountsPage());
        }
        else
        {
            MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void HaveAccountButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new LoginPage());
        if (NavigationService.CanGoBack)
        {
            NavigationService.RemoveBackEntry();
        }
        
    }
}