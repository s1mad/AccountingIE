using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AccountingIE;

namespace AccountingIE;

public partial class TransactionPage : Page
{
    public TransactionPage()
    {
        InitializeComponent();

        LoadTransaction();
    }

    private void FilterButton_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
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

        // Установите операции в ListBox
        transactionsListBox.ItemsSource = allUserTransactions;
    }
}