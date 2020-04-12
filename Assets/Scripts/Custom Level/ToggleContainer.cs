using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ToggleContainer
{
    private static List<GameObject> _toggles = new List<GameObject>();

    public static void AddToggle(GameObject newToggle)
    {
        _toggles.Add(newToggle);
    }

    //Gets toggle index that are on (chosen)
    public static List<int> GetOnToggleIdx()
    {
        List<int> resList=new List<int>();
        for (int i = 0; i < _toggles.Count; ++i)
        {
            if (_toggles[i].GetComponentInChildren<Toggle>().isOn)
            {
                resList.Add(i);
            }
        }

        return resList;
    }

    public static void DeactivateOffToggle()
    {
        foreach (var t in _toggles)
        {
            if (!t.GetComponentInChildren<Toggle>().isOn)
            {
                t.SetActive(false);
            }
        }
    }

    public static void ActivateAllToggle()
    {
        foreach (var t in _toggles)
        {
            t.SetActive(true);
        }
    }
}
