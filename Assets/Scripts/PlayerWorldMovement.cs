using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldMovement : MonoBehaviour
{
    [SerializeField] private GameObject levelBaseGameObject;
    [SerializeField] private MenuScreenController menuScreenController;
    private LevelScript _levelScript;
    
    private Transform[] waypoints;
    private int currWaypointIdx;
    private int currLoc;
    private bool isMoving = false;
    
    private Transform _playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        _levelScript = levelBaseGameObject.GetComponentInChildren<LevelScript>();
        _playerTransform = this.GetComponent<Transform>();
        waypoints = _levelScript.getWaypoints();
        transform.position = waypoints[(MenuScreenLoadParam.currentLevel-1)*3].position;
        currWaypointIdx = (MenuScreenLoadParam.currentLevel-1)*3;
        currLoc = MenuScreenLoadParam.currentLevel;
    }


    public void setWaypoints()
    {
        _levelScript = levelBaseGameObject.GetComponentInChildren<LevelScript>();
        waypoints = _levelScript.getWaypoints();
    }

    public void resetPlayer()
    {
        //Resets player position to default
        currWaypointIdx = 0;
        currLoc = 0;
        isMoving = false;
        transform.position = waypoints[0].position;
    }

    public void movePlayer(int wayIdx)
    {
        //Checks if the player is currently moving
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(moveToWaypoint(wayIdx));            
        }

    }

    IEnumerator moveToWaypoint(int wayIdx)
    {
        Vector3 currPos = _playerTransform.position;
        Vector3 origPos;
        float t;
        
        //While the character haven't arrived to the intended destination (3 waypoints inbetween levels)
        while (currWaypointIdx!=(wayIdx-1)*3)
        {
            origPos = _playerTransform.position;
            t = 0;
            
            if (currLoc < wayIdx)
            {
                //If going to level larger than the current
                ++currWaypointIdx;
            }
            else if (currLoc > wayIdx)
            {
                //If going to level smaller than the current
                --currWaypointIdx;
            }
            
            while (currPos != waypoints[currWaypointIdx].position)
            {
                //Moves the player from one waypoint to the next one smoothly
                currPos = Vector3.Lerp(origPos, waypoints[currWaypointIdx].position, t);
                _playerTransform.position = currPos;
                t += 3f * Time.deltaTime;
                yield return new WaitForSeconds(0.0001f);
            }

        }

        isMoving = false;
        currLoc = wayIdx;
        menuScreenController.showLevelPopup(currLoc);
    }
}
