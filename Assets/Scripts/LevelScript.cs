using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Transform[] getWaypoints()
    {
        return waypointsArr;
    }

    public void ButtonPasser(int wayidx)
    {
        _playerWorldMovement.movePlayer(wayidx);
    }

    //Only activate buttons for levels that are playable for the player according to llv
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
