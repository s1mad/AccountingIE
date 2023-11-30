using System.Windows;
using System.Windows.Controls;

namespace AccountingIE;

public partial class RegistrationPage : Page
{
    public RegistrationPage()
    {
        InitializeComponent();
    }
    
    private void RegistrationButton_Click(object sender, RoutedEventArgs e)
    {
        if (LoginBox.Text.Length < 3)
            MessageBox.Show("Login must be at least 3 characters long. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        
        else if (PasswordBox.Password.Length < 3)
            MessageBox.Show("Password must be at least 3 characters long. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        
        else if (!isPasswordsMatch(PasswordBox.Password, RepeatPasswordBox.Password))
            MessageBox.Show("Passwords don't match. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
        else if (DataBase.Users.IsLoginExists(LoginBox.Text))
            MessageBox.Show("Login's busy. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        
        else {
            DataBase.Users.CreateUser(LoginBox.Text, PasswordBox.Password);
            MessageBox.Show("You have successfully registered.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            Session.ActivateUser(LoginBox.Text);
            
            NavigationService.Navigate(new AccountsPage());
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
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
    
    private bool isPasswordsMatch(string password, string password2) => password == password2;
}