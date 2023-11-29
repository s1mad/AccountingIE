using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AccountingIE;

public partial class AccountsPage : Page
{
    public ObservableCollection<Account> accounts;
    public AccountsPage()
    {
        InitializeComponent();
        
        LoadAccounts();

        UpdateTotalBalance();
    }

    private void CreateAccount_Click(object sender, RoutedEventArgs e)
    {
        new CreateAccountWindow().ShowDialog();
        LoadAccounts();
        UpdateTotalBalance();
    }
    
    private void DeleteAccount_Click(object sender, RoutedEventArgs e)
    {
        new DeleteAccountWindow(accounts).ShowDialog();
        LoadAccounts();
        UpdateTotalBalance();
    }
    private void LoadAccounts()
    {
        accounts = new ObservableCollection<Account>(DataBase.Accounts.GetUserAccounts(Session.CurrentUser));

        accountsListBox.ItemsSource = accounts;
    }
    
    private void UpdateTotalBalance()
    {
        decimal totalBalance = accounts.Sum(account => account.Balance);
        
        totalBalanceText.Text = $"Total Balance: {totalBalance:C}";
    }
}