using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;

public class AttemptsFirebase : MonoBehaviour
{
    private DatabaseReference _Attempts;
    private DatabaseReference _Users;

    private List<Attempt> attemptList;
    private List<User> userList;
    public List<HighscoreEntry> highscoreEntryList;
    private Highscores highscores;

    public string levelId = "1";
    public int worldId = 5;

    public Transform entryContainer;
    public Transform entryTemplate;
    public List<Transform> highscoreEntryTransformList = new List<Transform>(10);
    void Awake()
    {
        // Set up editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://marvellous-kittens.firebaseio.com/");
        //CreateDbReference();

        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
    }

    private async void Start()
    {
        await GetAttemptDataFirebase(); //initialised attemptList
        await GetUserData(); //initialised userList
        Debug.Log("Attempt count: " + attemptList.Count);

        CreateHighscoreEntryList();

        DisplayHighscoreInTable(highscoreEntryList);

    }

    private void CreateDbReference()
    {
        //get the root reference location of the database
        DatabaseReference refDB = FirebaseDatabase.DefaultInstance.RootReference;
        _Attempts = refDB.Child("Attempts");
        _Users = refDB.Child("Users");
    }

    //my purpose is to just read the data and display it on the leaderboard, no writing, just reading

    public async Task GetAttemptDataFirebase()
    {
        //basically i only need the score and user id of that particular level
        await FirebaseDatabase.DefaultInstance.GetReference("Attempts").OrderByChild("levelId").EqualTo(levelId)
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
                        if (attempt1.worldId == worldId) {
                            attemptList.Add(attempt1);
                        }
                        
                    }


                }
            });
    }

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
                    userList = new List<User>();
                    //im in an attempt id in each loop
                    while (childEnumerator.MoveNext())
                    {
                        DataSnapshot child = childEnumerator.Current;

                        json = child.GetRawJsonValue();
                        User user1 = JsonUtility.FromJson<User>(json);
                        userList.Add(user1);
                    }

                }
            });
    }

    public void CreateHighscoreEntryList()
    {
        string name;
        int score;
        highscoreEntryList = new List<HighscoreEntry>();
        foreach (Attempt atmpt in attemptList)
        {
            for (int j = 0; j < userList.Count; j++)
            {
                if (atmpt.userId == userList[j].userId)
                {
                    name = userList[j].usr;
                    score = atmpt.score;
                    HighscoreEntry entry = new HighscoreEntry(name, score);
                    highscoreEntryList.Add(entry);
                }
            }
        }
    }

    //====================================Display Functions===============================================================
    public void DisplayHighscoreInTable(List<HighscoreEntry> highscoreEntryList)
    {
        //display objects on UI using template and container
        highscores = new Highscores(highscoreEntryList);
        SortHighscores(highscores);
        int k = 0;
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {

            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            k++;
            if (k > 9) break;
        }
    }

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

        int score = highscoreEntry.score;

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
            case 1:
                entryTransform.Find("trophy").GetComponent<Image>().color = CodeMonkey.Utils.UtilsClass.GetColorFromString("FFD200");
                break;
            case 2:
                entryTransform.Find("trophy").GetComponent<Image>().color = CodeMonkey.Utils.UtilsClass.GetColorFromString("C6C6C6");
                break;
            case 3:
                entryTransform.Find("trophy").GetComponent<Image>().color = CodeMonkey.Utils.UtilsClass.GetColorFromString("B76F56");
                break;

        }

        transformList.Add(entryTransform);
    }

    public void SortHighscores(Highscores highscores)
    {
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }


    }
}