using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using Firebase;

// The System.IO namespace contains functions related to loading and saving files
[RequireComponent(typeof(FirebaseScript))]

public class DataController : MonoBehaviour
{
    private worldData[] allWorldData;
    
    private worldData allRoundData;
    private PlayerProgress playerProgress;

    private FirebaseScript _firebaseScript;

    private string gameDataFileName = "data.json";
    private int currLevel = 1;
    private int currWorld = 1;

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

        SceneManager.LoadScene("MenuScreen");
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
        if (allRoundData == null)
        {
            Debug.Log("Was here");
            LoadWorldData(currWorld);
        }
        return allRoundData.LvlData[currLvl-1].name;
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    } 
    
    public void LoadGameData()
    {
        /*
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }*/

        allWorldData = _firebaseScript.GetWorldData();
        LoadWorldData(currWorld);
    }

    void LoadWorldData(int wldIdx)
    {
        if (allWorldData == null)
        {
            LoadGameData();
        }
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
