using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;

namespace AccountingIE;

public partial class DeleteAccountWindow : Window
{
    public DeleteAccountWindow(List<Account> accounts)
    {
        InitializeComponent();
        
        accountComboBox.ItemsSource = accounts;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        Account selectedAccount = accountComboBox.SelectedItem as Account;

        if (selectedAccount != null)
        {
            DataBase.Accounts.DeleteAccount(selectedAccount.AccountId);
            
            Close();
        }
        else
        {
            MessageBox.Show("Select the account to be deleted.");
        }
    }
}