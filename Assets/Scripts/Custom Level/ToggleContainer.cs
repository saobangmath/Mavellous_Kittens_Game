using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// The class responsible for managing the state of the game objects.
/// </summary>
public class ToggleContainer : MonoBehaviour
{
    private List<GameObject> _toggles = new List<GameObject>();
    /// <summary>
    ///  add a new toogle to the list of current toggles.
    /// </summary>
    /// <param name="newToggle">a new game object</param>
    public void AddToggle(GameObject newToggle)
    {
        _toggles.Add(newToggle);
    }
    /// <summary>
    /// Gets the list of toggle index that are on (chosen)
    /// </summary>
    public List<int> GetOnToggleIdx()
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
    /// <summary>
    /// deactivate the state of all available game objects
    /// </summary>
    public void DeactivateOffToggle()
    {
        foreach (var t in _toggles)
        {
            if (!t.GetComponentInChildren<Toggle>().isOn)
            {
                t.SetActive(false);
            }
        }
    }
    /// <summary>
    /// activate the state of all available game objects
    /// </summary>
    public void ActivateAllToggle()
    {
        foreach (var t in _toggles)
        {
            t.SetActive(true);
        }
    }
    /// <summary>
    /// get the list of all game objects which put in the toggle list
    /// </summary>
    /// <returns>List of gameObjects in the toggle List</returns>
    public List<GameObject> getToggles()
    {
        return _toggles;
    }
}
