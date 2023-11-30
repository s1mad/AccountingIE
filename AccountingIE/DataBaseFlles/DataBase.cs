using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Windows.Xps;

namespace AccountingIE;

public static class DataBase
{
    // Абсолютный путь к базе данных
    private static string connectionString = $"Data Source=D:\\Files\\Projects\\AccountingIE\\AccountingIE\\DataBaseFlles\\sqlite.db;Version=3;";

    public static class Users
    {
        public static int GetUserID(string login)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT user_id FROM users WHERE login = @login";
                    command.Parameters.AddWithValue("@login", login);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    return -1;
                }
            }
        } // Возвращает user_id
        public static bool IsLoginExists(string login)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM users WHERE login = @login";
                    command.Parameters.AddWithValue("@login", login);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        } // Возвращает true, если login существует
        public static bool IsLoginAndPasswordCorrect(string login, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM users WHERE login = @login AND password = @password";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        
        } // Возвращает true, если пара login, password существует
        public static void CreateUser(string login, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO users (login, password) VALUES (@login, @password)";
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    command.ExecuteNonQuery();
                }
            }
        } // Создаёт user в базе данных 
        public static void DeleteUser(string login)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM users WHERE login = @login";
                    command.Parameters.AddWithValue("@login", login);

                    command.ExecuteNonQuery();
                }
            }
        } // Удаляет user из базы данных 
        
    } // Класс для работы с таблицей users 
    
    public static class Accounts
    {
        public static void CreateAccount(int userId, string name, decimal balance)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO accounts (user_id, name, balance) VALUES (@userId, @name, @balance)";
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@balance", balance);

                    command.ExecuteNonQuery();
                }
            }
        } // Создание счёта
        
        public static List<Account> GetUserAccounts(int userId)
        {
            List<Account> accounts = new List<Account>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM accounts WHERE user_id = @userId";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                AccountId = Convert.ToInt32(reader["account_id"]),
                                UserId = Convert.ToInt32(reader["user_id"]),
                                Name = reader["name"].ToString(),
                                Balance = Convert.ToDecimal(reader["balance"])
                            };

                            accounts.Add(account);
                        }
                    }
                }
            }

            return accounts;
        }
        
        public static void DeleteAccount(int accountId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM accounts WHERE account_id = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateAccountDetails(int accountId, string newName, decimal newBalance)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE accounts SET name = @newName, balance = @newBalance WHERE account_id = @accountId";
                    command.Parameters.AddWithValue("@newName", newName);
                    command.Parameters.AddWithValue("@newBalance", newBalance);
                    command.Parameters.AddWithValue("@accountId", accountId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateBalance(int accountId, decimal amount)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Получаем изначальный баланс счета
                decimal initialBalance = GetBalance(accountId);

                // Рассчитываем новый баланс
                decimal newBalance = initialBalance + amount;

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE accounts SET balance = @newBalance WHERE account_id = @accountId";
                    command.Parameters.AddWithValue("@newBalance", newBalance);
                    command.Parameters.AddWithValue("@accountId", accountId);

                    command.ExecuteNonQuery();
                }
            }
        }
        private static decimal GetBalance(int accountId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT balance FROM accounts WHERE account_id = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }

                    throw new Exception("Balance not found");
                    return 0; 
                }
            }
        }
    } // Класс дял работы с таблицей accounts

    public static class Transactions
    {
        public static void CreateTransaction(int accountId, decimal amount, DateTime date, string category)
        {
            DateTime currentDate = DateTime.Now;
            DateTime fullDate = new DateTime(date.Year, date.Month, date.Day, currentDate.Hour, currentDate.Minute, currentDate.Second);
            
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO transactions (account_id, amount, date, category) VALUES (@accountId, @amount, @date, @category)";
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@date", fullDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@category", category);

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public static void DeleteTransaction(int transactionId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM transactions WHERE transaction_id = @transactionId";
                    command.Parameters.AddWithValue("@transactionId", transactionId);

                    command.ExecuteNonQuery();
                }
            }
        }
        
        public static void EditTransaction(int transactionId, int newAccountId, decimal newAmount, DateTime newDate, string newCategory)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE transactions SET account_id = @newAccountId, amount = @newAmount, date = @newDate, category = @newCategory WHERE transaction_id = @transactionId";
                    command.Parameters.AddWithValue("@newAccountId", newAccountId);
                    command.Parameters.AddWithValue("@newAmount", newAmount);
                    command.Parameters.AddWithValue("@newDate", newDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@newCategory", newCategory);
                    command.Parameters.AddWithValue("@transactionId", transactionId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Transaction> GetUserTransactions(int accountId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM transactions WHERE account_id = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["transaction_id"]),
                                AccountId = Convert.ToInt32(reader["account_id"]),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                // Используйте правильный формат даты для парсинга
                                Date = DateTime.Parse(reader["date"].ToString()), // Используем стандартный парсинг
                                Category = reader["category"].ToString()
                            };

                            transactions.Add(transaction);
                        }
                    }
                }
            }

            return transactions;
        }
        public static List<Transaction> GetAllUserTransactions(int userId)
        {
            List<Transaction> allTransactions = new List<Transaction>();
            
            List<Account> userAccounts = Accounts.GetUserAccounts(userId);
            
            foreach (var account in userAccounts)
            {
                List<Transaction> accountTransactions = GetUserTransactions(account.AccountId);
                allTransactions.AddRange(accountTransactions);
            }

            return allTransactions;
        }
        
    } // Класс дял работы с таблицей transactions
}