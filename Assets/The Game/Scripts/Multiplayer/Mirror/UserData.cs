using System;
using Shadow_Dominion.Main;

public class UserData : Singleton<UserData>
{
    public string Nickname { get; set; }
    public string IP { get; set; }

    public UserData()
    {
        if (Instance == null)
            Instance = this;
        else
            throw new Exception("Second instance.");
    }
}