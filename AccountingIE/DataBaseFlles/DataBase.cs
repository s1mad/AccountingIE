using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    }
    
}