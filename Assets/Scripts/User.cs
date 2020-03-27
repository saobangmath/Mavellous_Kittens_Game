using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserList
{
    List<User> userList;
}

[System.Serializable]
public class User
{
    public string chr;
    public string llv;
    public string usr;
    public int userId;
}
