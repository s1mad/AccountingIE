using System.Windows;

namespace AccountingIE;

public partial class CreateAccountWindow : Window
{
    public CreateAccountWindow()
    {
        InitializeComponent();
    }

    private void Create_Click(object sender, RoutedEventArgs e)
    {
        string accountName = accountNameTextBox.Text;
        
        if (string.IsNullOrEmpty(accountName))
        {
            MessageBox.Show("Please enter an account name.");
            return;
        }

        if (!decimal.TryParse(initialBalanceTextBox.Text, out decimal initialBalance))
        {
            MessageBox.Show("Please enter a valid initial balance.");
            return;
        }
        
        DataBase.Accounts.CreateAccount(Session.CurrentUser, accountName, initialBalance);
        
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}