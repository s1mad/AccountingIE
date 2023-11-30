using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AccountingIE;

public partial class TransactionPage : Page
{
    private Transaction selectedTransaction;
    public TransactionPage()
    {
        InitializeComponent();

        LoadTransaction();
    }

    private void AddTransactionButton_Click(object sender, RoutedEventArgs e)
    {
        new CreateTransactionWindow().ShowDialog();
        LoadTransaction();
    }

    private void AllAccounts_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AccountsPage());
        
        if (NavigationService.CanGoBack)
        {
            NavigationService.RemoveBackEntry();
        }
    }
    
    private void LoadTransaction()
    {
        List<Transaction> allUserTransactions = DataBase.Transactions.GetAllUserTransactions(Session.CurrentUser)
            .OrderByDescending(transaction => transaction.Date)
            .ToList();
        
        transactionsListBox.ItemsSource = allUserTransactions;
    }

    private void EditMenuItem_Click(object sender, RoutedEventArgs e)
    {
        selectedTransaction = (Transaction)transactionsListBox.SelectedItem;
        
        if (selectedTransaction != null)
        {
            UpdateTransactionWindow editWindow = new UpdateTransactionWindow(selectedTransaction);

            editWindow.ShowDialog();
            
            LoadTransaction();
        }
    }

    private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
        selectedTransaction = (Transaction)transactionsListBox.SelectedItem;
        
        if (selectedTransaction != null)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this transaction?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DataBase.Accounts.UpdateBalance(selectedTransaction.AccountId, -selectedTransaction.Amount);
                DataBase.Transactions.DeleteTransaction(selectedTransaction.TransactionId);

                LoadTransaction();
            }
        }
    }
}