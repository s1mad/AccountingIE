using System;
using System.Collections.Generic;
using System.Windows;

namespace AccountingIE;

public partial class CreateTransactionWindow : Window
{
    public List<Account> accounts;
    
    public CreateTransactionWindow()
    {
        InitializeComponent();
        
        accountComboBox.ItemsSource = new List<Account>(DataBase.Accounts.GetUserAccounts(Session.CurrentUser));;
    }

    private void Create_Click(object sender, RoutedEventArgs e)
    {
        string amountText = amountTextBox.Text;
        string category = categoryTextBox.Text;

        if (string.IsNullOrEmpty(amountText))
        {
            MessageBox.Show("Please enter an amount.");
            return;
        }

        if (!decimal.TryParse(amountText, out decimal amount))
        {
            MessageBox.Show("Please enter a valid amount.");
            return;
        }

        if (accountComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select an account.");
            return;
        }

        if (!DateTime.TryParse(datePicker.Text, out DateTime date))
        {
            MessageBox.Show("Please enter a valid date.");
            return;
        }
        
        Account selectedAccount = (Account)accountComboBox.SelectedItem;
       
        DataBase.Transactions.CreateTransaction(selectedAccount.AccountId, amount, date, category);
        
        DataBase.Accounts.UpdateBalance(selectedAccount.AccountId, amount);
        
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}