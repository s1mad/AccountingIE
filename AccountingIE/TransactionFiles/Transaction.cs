using System;

namespace AccountingIE;

public class Transaction
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
}