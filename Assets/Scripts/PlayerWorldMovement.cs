using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The class in charge of animating the player moving to each level.
/// </summary>
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


    /// <summary>
    /// Get the waypoints of each level button from the LevelScript.
    /// </summary>
    public void setWaypoints()
    {
        _levelScript = levelBaseGameObject.GetComponentInChildren<LevelScript>();
        waypoints = _levelScript.getWaypoints();
    }

    /// <summary>
    /// Returns the player back to the starting position, i.e. Level 1
    /// </summary>
    public void resetPlayer()
    {
        //Resets player position to default
        currWaypointIdx = 0;
        currLoc = 0;
        isMoving = false;
        transform.position = waypoints[0].position;
    }

    /// <summary>
    /// Moves the player to the specified Level number
    /// </summary>
    /// <param name="wayIdx">The level number, ranges from 0 to 5</param>
    public void movePlayer(int wayIdx)
    {
        //Checks if the player is currently moving
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(moveToWaypoint(wayIdx));            
        }

    }

    /// <summary>
    /// The function in charge of animating the player's UI component to the chosen Level button's waypoint.
    /// </summary>
    /// <param name="wayIdx">The level number, ranges from 0 to 5</param>
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
