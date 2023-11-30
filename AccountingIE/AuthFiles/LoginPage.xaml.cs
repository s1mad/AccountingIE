using System.Windows;
using System.Windows.Controls;

namespace AccountingIE;

public partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataBase.Users.IsLoginAndPasswordCorrect(LoginBox.Text, PasswordBox.Password))
        {
            Session.ActivateUser(LoginBox.Text);
            
            NavigationService.Navigate(new AccountsPage());
        }
        else
        {
            MessageBox.Show("Invalid login or password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void NoAccountButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RegistrationPage());
        
        if (NavigationService.CanGoBack)
        {
            NavigationService.RemoveBackEntry();
        }
    }
}