using System;
using System.Data.SQLite;
using System.IO;

namespace AccountingIE;

public static class DataBase
{
    private static string connectionString = $"Data Source=D:\\Files\\Projects\\AccountingIE\\AccountingIE\\sqlite.db;Version=3;"; // Абсолютный путь к базе данных
    
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
}