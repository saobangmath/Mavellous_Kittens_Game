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
    private PlayerProgress playerProgress; 
    public User currentUser;

    private FirebaseScript _firebaseScript;

    private string gameDataFileName = "data.json";
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

        LoadPlayerProgress();
        
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
        // If we wanted to return different rounds, we could do that here
        // We could store an int representing the current round index in PlayerProgress
        
        return allRoundData.LvlData[currLvl-1];
    }
    
    public void SubmitNewPlayerScore(int newScore)
    {
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if(newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public string GetLevelName(int currLvl) {
        return allRoundData.LvlData[currLvl-1].name;
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
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

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void LoadPlayerProgress()
    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if(PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }
}
