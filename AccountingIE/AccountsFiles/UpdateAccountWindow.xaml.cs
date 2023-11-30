using System.Windows;

namespace AccountingIE;

public partial class UpdateAccountWindow : Window
{
    private Account editedAccount;
    
    public UpdateAccountWindow(Account account)
    {
        InitializeComponent();
        editedAccount = account;
        DataContext = editedAccount;
    }
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            if (!decimal.TryParse(initialBalanceTextBox.Text, out decimal initialBalance))
            {
                MessageBox.Show("Please enter a valid initial balance.");
                return;
            }

            DataBase.Accounts.UpdateAccountDetails(editedAccount.AccountId, accountNameTextBox.Text, initialBalance);

            DialogResult = true;
        }
    }
    
    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}