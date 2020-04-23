using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase;
using TMPro;
using UnityEngine.Serialization;

// The System.IO namespace contains functions related to loading and saving files

/// <summary>
/// The Controller class for storing and managing the overall Game States and for initialising the Game Data.
/// This class allows for the passing of information between subsystems. 
/// </summary>
[RequireComponent(typeof(FirebaseScript))]
public class DataController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    
    private worldData[] allWorldData;

    private List<RoundData> allLvlData;
    
    private worldData allRoundData;
    public User currentUser;

    private FirebaseScript _firebaseScript;

    private int currLevel = 1;
    private int currWorld = 1;
    private bool loadingFinished = false;
    private bool isCustom = false;
    private string lvlID;

    /// <summary>
    /// Get the World that the game is currently on
    /// </summary>
    public int getCurrWorld()
    {
        return currWorld;
    }

    /// <summary>
    /// Get the Level that the game is currently on
    /// </summary>
    public int getCurrLevel()
    {
        return currLevel;
    }

    /// <summary>
    /// Sets the Game to be of a specific World using the parameter wrld.
    /// </summary>
    /// <param name="wrld">A World Number between 1-6</param>
    public void setCurrWorld(int wrld)
    {
        currWorld = wrld;
        if (currWorld <= allWorldData.Length)
        {
            LoadWorldData(currWorld);
        }
    }

    /// <summary>
    /// Sets the Game to be of a specific Level using the parameter lvl
    /// </summary>
    /// <param name="lvl">The Level Number between 1-6</param>
    public void setCurrLevel(int lvl)
    {
        currLevel = lvl;
    }

    /// <summary>
    /// Retrieve the isCustom variable, which tells the Game if it is starting a custom level.
    /// </summary>
    /// <returns></returns>
    public bool getCustom()
    {
        return isCustom;
    }

    /// <summary>
    /// Sets the isCustom variable.
    /// </summary>
    /// <param name="val">The setting boolean value</param>
    public void setCustom(bool val)
    {
        isCustom = val;
    }

    /// <summary>
    /// Sets the Game's current Level ID.
    /// </summary>
    /// <param name="id">The Level ID of the current level</param>
    public void setLvlID(string id)
    {
        lvlID = id;
    }

    /// <summary>
    /// Get the Game's current Level ID
    /// </summary>
    /// <returns></returns>
    public string getLvlID()
    {
        return lvlID;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        _firebaseScript = GetComponent<FirebaseScript>();
    }

    private void Update()
    {
        if (!loadingFinished)
        {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

            if (_firebaseScript.isLoading == false)
            {
                loadingFinished = true;
                Debug.Log("Getting User Data");
                currentUser = _firebaseScript.GetUserData();
                SceneManager.LoadSceneAsync("MenuScreen");
            }            
        }
    }
    
    //Overloading the function to serve both regular levels and custom levels
    /// <summary>
    /// Get all the data that is to be used for the gameplay.
    /// </summary>
    /// <param name="currLvl">The level number that is going to be played</param>
    /// <returns></returns>
    public RoundData GetCurrentRoundData(int currLvl)
    {
        return allRoundData.LvlData[currLvl-1];
    }

    /// <summary>
    /// Get all the data that is to be used for the gameplay.
    /// </summary>
    /// <param name="currLvl">The level ID that is going to be played</param>
    /// <returns></returns>
    public RoundData GetCurrentRoundData(string lvlID)
    {
        foreach (var lvl in allLvlData)
        {
            if (lvl.lvlId == lvlID)
            {
                return lvl;
            }
        }

        return null;
    }
    
    /// <summary>
    /// Get the level name
    /// </summary>
    /// <param name="currLvl">The level number</param>
    /// <returns></returns>
    public string GetLevelName(int currLvl) {
        return allRoundData.LvlData[currLvl-1].name;
    }

    /// <summary>
    /// Initialise the game's World and Level data from the Firebase database
    /// </summary>
    public void LoadGameData()
    {
        allWorldData = _firebaseScript.GetWorldData();
        allLvlData = _firebaseScript.GetLevelData();

        LoadWorldData(currWorld);
        
    }

    /// <summary>
    /// Initialise the world data.
    /// </summary>
    void LoadWorldData(int wldIdx)
    {
        //As custom level is wldIdx 6, allWorldData will be out of bounds if this check is not in place
        if (wldIdx <= allWorldData.Length)
        {
            allRoundData = allWorldData[wldIdx-1];
        }
        
    }

    /// <summary>
    /// Get the user's latest level progress
    /// </summary>
    /// <returns></returns>
    public string GetUserLLv()
    {
        return currentUser.llv;
    }

    /// <summary>
    /// Increase the user's latest level progress.
    /// </summary>
    public void IncUserLLv()
    {
        //If currentUser llv is smaller than the level completed, increase llv
        if (int.Parse(currentUser.llv) < currWorld * 6 + currLevel)
        {
            currentUser.llv = ((currWorld-1) * 6 + currLevel).ToString();
            //If not debug user
            if (currentUser.usr != "siaii" )
            {
                _firebaseScript.UpdateUserLLv(currentUser.llv);
            }
        }
    }

    /// <summary>
    /// Checks if a level with the entered level ID exists
    /// </summary>
    /// <param name="lvlID"></param>
    /// <returns></returns>
    public bool CheckLevelID(string lvlID)
    {
        foreach (var lvl in allLvlData)
        {
            if (lvl.lvlId == lvlID)
            {
                return true;
            }
        }
        
        return false;
    }
    
}
