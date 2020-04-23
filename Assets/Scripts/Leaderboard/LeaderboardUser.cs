using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The class UserList represent the user list model which comprise of the list of many leaderboard user
/// </summary>
public class UserList
{
    List<LeaderboardUser> userList;
}
/// <summary>
/// the class LeaderboardUser represent the leaderboard user object
/// </summary>
[System.Serializable]
public class LeaderboardUser
{
    public string chr;
    public string llv;
    public string usr;
    public string userId;
}
