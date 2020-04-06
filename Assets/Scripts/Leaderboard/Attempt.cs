using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptList
{
    List<Attempt> attemptList;
}

[System.Serializable]
public class Attempt
{
    public string levelId;
    public string score;
    public string time=0.ToString();
    public string userId;
    public string worldId;
    public Attempt(int world, int lvl, int score, string id)
    {
        this.levelId = lvl.ToString();
        this.worldId = world.ToString();
        this.score = score.ToString();
        this.userId = id;
    }
}
