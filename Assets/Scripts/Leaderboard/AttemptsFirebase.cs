using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// the AttemptsFirebase class responsible for all calling to the Firebase database server 
/// </summary>
public class AttemptsFirebase : MonoBehaviour
{
    private DatabaseReference _Attempts;
    private DatabaseReference _Users;

    private List<Attempt> attemptList;
    private List<LeaderboardUser> userList;
    public List<HighscoreEntry> highscoreEntryList;
    private Highscores highscores;

    private DataController _dataController;

    public string levelId;
    public int worldId = 5;

    public Transform entryContainer;
    public Transform entryTemplate;
    public List<Transform> highscoreEntryTransformList = new List<Transform>(10);
    /// <summary>
    /// the Awake() function triggered to set up the url connection to the Firebase database server and set up other required variables for interaction with the cloud database
    /// </summary>
    void Awake()
    {
        // Set up editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://marvellous-kittens.firebaseio.com/");
        //CreateDbReference();

        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
        
        _dataController = FindObjectOfType<DataController>();
    }
    /// <summary>
    /// the Start() function is trigger to display the list of available top 10 players for a specific level and world id
    /// </summary>
    private async void Start()
    {
        if (_dataController.getCustom())
        {
            levelId = _dataController.getLvlID();
        }
        else
        {
            levelId = _dataController.getCurrLevel().ToString();            
        }
        worldId = _dataController.getCurrWorld();
        
        await GetAttemptDataFirebase(); //initialised attemptList
        await GetUserData(); //initialised userList
        Debug.Log("Attempt count: " + attemptList.Count);
        
        CreateHighscoreEntryList();

        DisplayHighscoreInTable(highscoreEntryList);

        
    }
    /// <summary>
    /// the CreateDbReference() to initialize the suitable database section for each variables 
    /// e.g. _Attempts should be corresponding to the 
    /// </summary>
    private void CreateDbReference()
    {
        //get the root reference location of the database
        DatabaseReference refDB = FirebaseDatabase.DefaultInstance.RootReference;
        _Attempts = refDB.Child("Attempts");
        _Users = refDB.Child("Users");
    }

    /// <summary>
    ///  GetAttemptDataFirebase()  purpose is just read the data and display it on the leaderboard, no writing, just reading
    /// </summary>
    public async Task GetAttemptDataFirebase()
    {
        //basically i only need the score and user id of that particular level
        await FirebaseDatabase.DefaultInstance.GetReference("Attempts").OrderByChild("levelId").EqualTo(levelId.ToString())
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("task faulted");
                }
                else if (task.IsCompleted)
                {
                    //it will return datasnapshot as a list
                    DataSnapshot test = task.Result;
                    Debug.Log(test.GetRawJsonValue());
                    Debug.Log(test.ChildrenCount);
                    IEnumerator<DataSnapshot> childEnumerator = test.Children.GetEnumerator();
                    string json;
                    attemptList = new List<Attempt>();
                    //im in an attempt id in each loop
                    while (childEnumerator.MoveNext())
                    {
                        DataSnapshot child = childEnumerator.Current;

                        json = child.GetRawJsonValue();
                        Attempt attempt1 = JsonUtility.FromJson<Attempt>(json);
                        if (attempt1.worldId == worldId.ToString()) {
                            attemptList.Add(attempt1);
                        }
                    }
                }
            });
    }
    /// <summary>
    /// the function to get the user data to use for displaying in the leaderboard later
    /// </summary>
    /// <returns></returns>
    public async Task GetUserData()
    {
        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("task faulted");
                }
                else if (task.IsCompleted)
                {
                    //it will return datasnapshot as a list
                    DataSnapshot test = task.Result;
                    Debug.Log(test.ChildrenCount);
                    IEnumerator<DataSnapshot> childEnumerator = test.Children.GetEnumerator();
                    string json;
                    userList = new List<LeaderboardUser>();
                    //im in an attempt id in each loop
                    while (childEnumerator.MoveNext())
                    {
                        DataSnapshot child = childEnumerator.Current;
                        Debug.Log(child.Key);
                        json = child.GetRawJsonValue();
                        LeaderboardUser user1 = JsonUtility.FromJson<LeaderboardUser>(json);
                        user1.userId = child.Key.ToString();
                        userList.Add(user1);
                    }

                }
            });
    }
    /// <summary>
    /// the CreateHighscoreEntryList to create a list of highscore entry
    /// </summary>
    public void CreateHighscoreEntryList()
    {
        string name;
        string score;
        highscoreEntryList = new List<HighscoreEntry>();
        foreach (Attempt atmpt in attemptList)
        {
            for (int j = 0; j < userList.Count; j++)
            {
                if (atmpt.userId == userList[j].userId)
                {
                    name = userList[j].usr;
                    score = atmpt.score;
                    HighscoreEntry entry = new HighscoreEntry(name, score, userList[j].userId);
                    highscoreEntryList.Add(entry);
                }
            }
        }
    }


    /// <summary>
    /// DisplayHighscoreInTable(highscoreEntryList) triggered to display the top 10 highscore in the leaderboard
    /// </summary>
    /// <param name="highscoreEntryList">list of highscore entry</param>
    public void DisplayHighscoreInTable(List<HighscoreEntry> highscoreEntryList)
    {
        //Initialise variable to keep track of already added users
        List<string> addedUsers = new List<string>();

        //display objects on UI using template and container
        highscores = new Highscores(highscoreEntryList);
        SortHighscores(highscores);
        int k = 0;
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            if (addedUsers.Contains(highscoreEntry.userId) || Int32.Parse(highscoreEntry.score) > 150)
                continue;
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            addedUsers.Add(highscoreEntry.userId);
            k++;
            if (k > 9) break;
        }
    }
    /// <summary>
    /// The CreateHighscoreEntryTransform(highscoreEntry, container, transformList) is triggered to create a high score entry transformation
    /// </summary>
    /// <param name="highscoreEntry">the high score entry </param>
    /// <param name="container">the transform container object</param>
    /// <param name="transformList">the transform list of Transformation object</param>
    public void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 31f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        entryTransform.gameObject.tag = "Respawn";

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        string score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // Highlight First
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank)
        {
            default:
                entryTransform.Find("trophy").gameObject.SetActive(false);
                break;

        }

        transformList.Add(entryTransform);
    }
    /// <summary>
    /// this function is triggered to sort the list of the highscores
    /// </summary>
    /// <param name="highscores">List of highscore for many players</param>
    public void SortHighscores(Highscores highscores)
    {
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (int.Parse(highscores.highscoreEntryList[j].score) > int.Parse(highscores.highscoreEntryList[i].score))
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
    }
    /// <summary>
    /// BackToWorldButton() is triggered to return to the main menu scence   
    /// </summary>
    public void BackToWorldButton()
    {
        SceneManager.LoadSceneAsync("MenuScreen");
    }
}