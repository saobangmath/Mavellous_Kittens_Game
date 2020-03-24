using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldMovement : MonoBehaviour
{
    [SerializeField] private GameObject levelBaseGameObject;
    [SerializeField] private MenuScreenController menuScreenController;
    private LevelScript _levelScript;
    
    private Transform[] waypoints;
    private int currWaypointIdx = 0;
    private int currLoc;
    private bool isMoving = false;
    
    private Transform _playerTransform;


    // Start is called before the first frame update
    void Start()
    {
        _levelScript = levelBaseGameObject.GetComponentInChildren<LevelScript>();
        _playerTransform = this.GetComponent<Transform>();
        waypoints = _levelScript.getWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public int getCurrLoc()
    {
        return currLoc;
    }

    public void movePlayer(int wayIdx)
    {
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
        while (currWaypointIdx!=(wayIdx-1)*3)
        {
            origPos = _playerTransform.position;
            t = 0;
            
            if (currLoc < wayIdx)
            {
                ++currWaypointIdx;
            }
            else if (currLoc > wayIdx)
            {
                --currWaypointIdx;
            }
            
            while (currPos != waypoints[currWaypointIdx].position)
            {
                currPos = Vector3.Lerp(origPos, waypoints[currWaypointIdx].position, t);
                _playerTransform.position = currPos;
                t += 0.05f;
                yield return new WaitForSeconds(0.02f);
            }

        }

        isMoving = false;
        currLoc = wayIdx;
        menuScreenController.showLevelPopup(currLoc);
    }
}
