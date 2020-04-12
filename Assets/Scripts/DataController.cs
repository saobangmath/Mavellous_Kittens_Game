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

    public int getCurrWorld()
    {
        return currWorld;
    }

    public int getCurrLevel()
    {
        return currLevel;
    }

    public void setCurrWorld(int wrld)
    {
        currWorld = wrld;
        if (currWorld <= allWorldData.Length)
        {
            LoadWorldData(currWorld);
        }
    }

    public void setCurrLevel(int lvl)
    {
        currLevel = lvl;
    }

    public bool getCustom()
    {
        return isCustom;
    }

    public void setCustom(bool val)
    {
        isCustom = val;
    }

    public void setLvlID(string id)
    {
        lvlID = id;
    }

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
    public RoundData GetCurrentRoundData(int currLvl)
    {
        return allRoundData.LvlData[currLvl-1];
    }
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
    

    public string GetLevelName(int currLvl) {
        return allRoundData.LvlData[currLvl-1].name;
    }

    public void LoadGameData()
    {
        allWorldData = _firebaseScript.GetWorldData();
        allLvlData = _firebaseScript.GetLevelData();

        LoadWorldData(currWorld);
        
    }

    void LoadWorldData(int wldIdx)
    {
        //As custom level is wldIdx 6, allWorldData will be out of bounds if this check is not in place
        if (wldIdx <= allWorldData.Length)
        {
            allRoundData = allWorldData[wldIdx-1];
        }
        
    }

    public string GetUserLLv()
    {
        return currentUser.llv;
    }

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

    //Checks if a level with the entered level ID exists
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
