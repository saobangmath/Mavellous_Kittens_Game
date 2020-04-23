using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// the class Highscores represent the list of all HighscoreEntry objects 
/// </summary>
public class Highscores
{
    public List<HighscoreEntry> highscoreEntryList;
    /// <summary>
    /// constructor for the Highscores objects
    /// </summary>
    /// <param name="list">the list of all highscoreEntry objects</param>
    public Highscores(List<HighscoreEntry> list)
    {
        this.highscoreEntryList = list;
    }
}
/// <summary>
/// the class HighscoreEntry represent the high score object 
/// </summary>
[System.Serializable]
public class HighscoreEntry
{
    public string name;
    public string score;
    public string userId;
    /// <summary>
    /// constructor for the HighscoreEntry object
    /// </summary>
    /// <param name="name">the name of the player</param>
    /// <param name="score">the score achieve</param>
    /// <param name="userId">the user id </param>
    public HighscoreEntry(string name, string score, string userId)
    {
        this.name = name;
        this.score = score;
        this.userId = userId;
    }
}
