﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores
{
    public List<HighscoreEntry> highscoreEntryList;

    public Highscores(List<HighscoreEntry> list)
    {
        this.highscoreEntryList = list;
    }
}

[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public string score;
    public string userId;
    public HighscoreEntry(string name, string score, string userId)
    {
        this.name = name;
        this.score = score;
        this.userId = userId;
    }
}
