using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
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
    
    private worldData allRoundData;
    public User currentUser;

    private FirebaseScript _firebaseScript;

    private int currLevel = 1;
    private int currWorld = 1;
    private bool loadingFinished = false;

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
        LoadWorldData(currWorld);
    }

    public void setCurrLevel(int lvl)
    {
        currLevel = lvl;
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
    

    public RoundData GetCurrentRoundData(int currLvl)
    {
        return allRoundData.LvlData[currLvl-1];
    }

    public string GetLevelName(int currLvl) {
        return allRoundData.LvlData[currLvl-1].name;
    }

    public void LoadGameData()
    {
        allWorldData = _firebaseScript.GetWorldData();
        LoadWorldData(currWorld);
        
    }

    void LoadWorldData(int wldIdx)
    {
        allRoundData = allWorldData[wldIdx-1];
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
    
}
