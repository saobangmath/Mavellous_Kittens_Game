using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class in charge of handling the Level Buttons from 1-6 for each World. It also records the waypoints for each button
/// to faciliate the movement animation of the player character in the PlayerWorldMovement class.
/// </summary>
public class LevelScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypointsArr;
    [SerializeField] private Button[] buttonsArr;

    private Canvas _staticCanvas;
    private PlayerWorldMovement _playerWorldMovement;
    private DataController _dataController;

    private void Start()
    {
        _staticCanvas = GetComponentInChildren<Canvas>();
        _playerWorldMovement = FindObjectOfType<PlayerWorldMovement>();
        _dataController = FindObjectOfType<DataController>();
        
        ActivateLevelButtons();
        
        _staticCanvas.worldCamera=Camera.main;
    }

    /// <summary>
    /// Returns the buttons waypoints
    /// </summary>
    public Transform[] getWaypoints()
    {
        return waypointsArr;
    }

    /// <summary>
    /// The trigger function for when the player taps on a desired Level button. It takes the Level ID as a paramter and moves the player to the
    /// waypoint of that Level button.
    /// </summary>
    /// <param name="wayidx">The Level Number to travel to. Ranges from 0 to 5</param>
    public void ButtonPasser(int wayidx)
    {
        _playerWorldMovement.movePlayer(wayidx);
    }

    /// <summary>
    /// The function used to activate the level buttons that the player has access to.
    /// </summary>
    /// <example>
    /// If the player has only cleared World 1 Level 1, all Level buttons from Level 3 to 6 will not be interactable
    /// </example>
    private void ActivateLevelButtons()
    {
        for (int i = 0; i < buttonsArr.Length; ++i)
        {
            if(int.Parse(_dataController.GetUserLLv())>=(_dataController.getCurrWorld()-1)*6+i)
            {
                buttonsArr[i].interactable = true;
            }
        }
    }
    
}
