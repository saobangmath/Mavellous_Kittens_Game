using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// the list of Attempt objects
/// </summary>
public class AttemptList
{
    List<Attempt> attemptList;
}

/// <summary>
/// the Attempt class model
/// </summary>
[System.Serializable]
public class Attempt
{
    public string levelId;
    public string score;
    public string time=0.ToString();
    public string userId;
    public string worldId;/// <summary>
    /// the first constructor for Attempt instance 
    /// </summary>
    /// <param name="world">world ID</param>
    /// <param name="lvl">level ID</param>
    /// <param name="score">score achieved by the user</param>
    /// <param name="id">the user ID</param>
    public Attempt(int world, int lvl, int score, string id)
    {
        this.levelId = lvl.ToString();
        this.worldId = world.ToString();
        this.score = score.ToString();
        this.userId = id;
    }
    /// <summary>
    /// the second constructor for the Attempt instance
    /// </summary>
    /// <param name="world">world ID</param>
    /// <param name="lvl">level ID</param>
    /// <param name="score">score achieved by the user</param>
    /// <param name="id">the user ID</param>
    public Attempt(int world, string lvl, int score, string id)
    {
        this.levelId = lvl;
        this.worldId = world.ToString();
        this.score = score.ToString();
        this.userId = id;
    }
}
