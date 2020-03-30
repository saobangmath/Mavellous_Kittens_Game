using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypointsArr;

    private Canvas _staticCanvas;
    private PlayerWorldMovement _playerWorldMovement;


    private void Start()
    {
        _staticCanvas = GetComponentInChildren<Canvas>();
        _playerWorldMovement = FindObjectOfType<PlayerWorldMovement>();
        
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
    
    
}
