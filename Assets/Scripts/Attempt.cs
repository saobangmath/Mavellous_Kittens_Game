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
    public int levelId;
    public int score;
    public int time;
    public int userId;
    public int worldId;
    public Attempt(int id, int score)
    {
        this.levelId = id;
        this.score = score;
    }
}
