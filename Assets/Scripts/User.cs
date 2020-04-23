using UnityEngine;

public class User
{
    // Character sprite chosen by the user
    public string chr;
    // Last level finished by the user
    public string llv;
    // Username
    public string usr;

    /// <summary>
    /// Constructor for User object. Sets the last level attribute to 0.
    /// </summary>
    public User()
    {
        llv = "0";
    }
    
    /// <summary>
    /// Convert User object data to json format.
    /// </summary>
    /// <returns>Json String format of User object data</returns>
    public string SaveToJsonString()
    {
        return JsonUtility.ToJson(this);
    }
}
