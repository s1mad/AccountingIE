using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AccountingIE;

public partial class UpdateTransactionWindow : Window
{
    private Transaction editedTransaction;
    private ObservableCollection<Account> accountList;
    public UpdateTransactionWindow(Transaction transaction)
    {
        InitializeComponent();
        List<Account> userAccounts = DataBase.Accounts.GetUserAccounts(Session.CurrentUser);
        
        accountList = new ObservableCollection<Account>(userAccounts);
        
        accountComboBox.ItemsSource = accountList;
        
        int selectedIndex = accountList.IndexOf(accountList.FirstOrDefault(acc => acc.AccountId == transaction.AccountId));
        
        accountComboBox.SelectedIndex = selectedIndex;
        
        editedTransaction = transaction;
        DataContext = editedTransaction;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            if (!decimal.TryParse(amountTextBox.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }
            
            DataBase.Accounts.UpdateBalance(editedTransaction.AccountId,  -DataBase.Transactions.GetTransaction(editedTransaction.TransactionId).Amount  + amount);
            
            DataBase.Transactions.EditTransaction(editedTransaction.TransactionId, 
                editedTransaction.AccountId, 
                amount, 
                datePicker.SelectedDate ?? DateTime.Now, 
                categoryTextBox.Text);

            DialogResult = true;
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}