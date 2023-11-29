using System;

namespace AccountingIE;

public static class Session
{
    public static int CurrentUser { get; private set; }
    public static void ActivateUser(string login)
    {
        if (!DataBase.Users.IsLoginExists(login))
        {
            throw new Exception("The user with this login does not exist");
        }
    
        CurrentUser = DataBase.Users.GetUserID(login);
    }
}