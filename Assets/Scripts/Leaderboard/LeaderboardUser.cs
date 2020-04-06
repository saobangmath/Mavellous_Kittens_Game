using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserList
{
    List<LeaderboardUser> userList;
}

[System.Serializable]
public class LeaderboardUser
{
    public string chr;
    public string llv;
    public string usr;
    public string userId;
}
