using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldData
{
    private string worldName;
    private RoundData[] lvlData;

    public string WorldName
    {
        get => worldName;
        set => worldName = value;
    }

    public RoundData[] LvlData
    {
        get => lvlData;
        set => lvlData = value;
    }
}
