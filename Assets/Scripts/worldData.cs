using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldData
{
    private string worldName;
    private RoundData[] lvlData;

    /// <summary>
    /// Getter and Setter for the World object's name.
    /// </summary>
    public string WorldName
    {
        get => worldName;
        set => worldName = value;
    }

    /// <summary>
    /// Getter and Setter for the World object's level data.
    /// </summary>
    public RoundData[] LvlData
    {
        get => lvlData;
        set => lvlData = value;
    }
}
