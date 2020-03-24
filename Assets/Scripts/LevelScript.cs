using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypointsArr;

    public Transform[] getWaypoints()
    {
        return waypointsArr;
    }
}
