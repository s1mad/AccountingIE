using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AccountingIE;

public partial class AccountsPage : Page
{
    public List<Account> accounts;
    
    private Account selectedAccount;
    
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

    private void LoadAccounts()
    {
        accounts = new List<Account>(DataBase.Accounts.GetUserAccounts(Session.CurrentUser));

        accountsListBox.ItemsSource = accounts;
    }
    
    private void UpdateTotalBalance()
    {
        decimal totalBalance = accounts.Sum(account => account.Balance);
        
        totalBalanceText.Text = $"Total Balance: {totalBalance:C}";
    }

    private void AllTransaction_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new TransactionPage());
        
        if (NavigationService.CanGoBack)
        {
            NavigationService.RemoveBackEntry();
        }
    }

    private void EditMenuItem_Click(object sender, RoutedEventArgs e)
    {
        // Получите выбранный аккаунт
        selectedAccount = (Account)accountsListBox.SelectedItem;

        // Проверка на null важна, так как контекстное меню может вызываться не только через клик мыши
        if (selectedAccount != null)
        {
            UpdateAccountWindow editWindow = new UpdateAccountWindow(selectedAccount);
            if (editWindow.ShowDialog() == true)
            {
                // Если пользователь подтвердил редактирование, обновите список и баланс
                LoadAccounts();
                UpdateTotalBalance();
            }
        }
    }
    
    private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        selectedAccount = (Account)accountsListBox.SelectedItem;
        
        if (selectedAccount != null)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete \"{selectedAccount.Name}\" account?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DataBase.Accounts.DeleteAccount(selectedAccount.AccountId);

                LoadAccounts();
                UpdateTotalBalance();
            }
        }
    }
}