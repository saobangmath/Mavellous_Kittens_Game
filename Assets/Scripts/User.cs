using UnityEngine;

public class User
{
    // Character sprite chosen by the user
    public string chr;
    // Last level finished by the user
    public string llv;
    // Username
    public string usr;

    public User()
    {
        llv = "0";
    }

    // Convert User object data to json format
    public string SaveToJsonString()
    {
        return JsonUtility.ToJson(this);
    }
}
