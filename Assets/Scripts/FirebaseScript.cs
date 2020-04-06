using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;

public class FirebaseScript : MonoBehaviour
{

    private string[] presetWorldList=new string[]{"300003", "300004", "300005", "300006", "300007"};
    
    private DatabaseReference _reference;
    private DatabaseReference _worldReference;
    private DatabaseReference _levelReference;
    private DatabaseReference _questionReference;
    private DatabaseReference _userReference;
    private DatabaseReference _attemptReference;

    private List<QnA> _questionList;
    private List<RoundData> _levelList;
    
    private worldData[] worldDataResult;
    private User currentUser;
    public bool isLoading = true;
    
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://marvellous-kittens.firebaseio.com/");
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
        _worldReference = _reference.Child("Worlds");
        _levelReference = _reference.Child("Levels");
        _questionReference = _reference.Child("Questions");
        _userReference = _reference.Child("Users");
        _attemptReference = _reference.Child("Attempts");
    }

    private async void Start()
    {
        worldDataResult= new worldData[presetWorldList.Length];
        _questionList=new List<QnA>();
        _levelList=new List<RoundData>();
        
        for (int i = 0; i < worldDataResult.Length; ++i)
        {
            if (worldDataResult[i] == null)
            {
                worldDataResult[i]=new worldData();
            }
        }


        await LoadGameData();
        
        isLoading = false;

        Debug.Log("Data Finished Loading");
        /*for (int i = 0; i < worldDataResult.Length; ++i)
        {
            Debug.Log("world: "+worldDataResult[i].WorldName);
            for (int j = 0; j < worldDataResult[i].LvlData.Length; ++j)
            {
                Debug.Log("level: "+worldDataResult[i].LvlData[j].name);
            }
        }*/
    }



    public worldData[] GetWorldData()
    {
        return worldDataResult;
    }

    public User GetUserData()
    {
        return currentUser;
    }
    

    public async Task LoadGameData()
    {
        await LoadQuestionData();
        await LoadLevelData();
        
        
        //Loads DataSnapshot that is referenced by _worldReference
        await _worldReference.OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error loading world!");
            }
            else if (task.IsCompleted)
            {
                //Get enumerator for _worldReference's children node (list of worlds)
                IEnumerator<DataSnapshot> worldEnumerator = task.Result.Children.GetEnumerator();
                
                //Var to track current world being loaded
                int wldIdx = 0;
                
                //Iterates through all child
                while (worldEnumerator.MoveNext())
                {
                    worldData worldTemp = new worldData();
                    worldTemp.LvlData=new RoundData[6];
                    RoundData[] lvlCol=new RoundData[6];
                    for (int i = 0; i < lvlCol.Length; ++i)
                    {
                        worldTemp.LvlData[i]=new RoundData();
                        lvlCol[i]=new RoundData();
                    }
                    
                    DataSnapshot world = worldEnumerator.Current;
                    
                    //Var to track current level being loaded
                    int lvlIdx = 0;

                    //Get enumerator for the chosen world's child (list of levels)
                    IEnumerator<DataSnapshot> childEnumerator = world.Children.GetEnumerator();
                    while (childEnumerator.MoveNext())
                    {
                        DataSnapshot lvl = childEnumerator.Current;

                        //Special case for "wld", the world name
                        if (lvl.Key == "wld")
                        {
                            worldTemp.WorldName = lvl.Value.ToString();
                        }
                        else
                        {
                            RoundData lvlTemp=new RoundData();
                            for (int i = 0; i < _levelList.Count; ++i)
                            {
                                if (_levelList[i].lvlId == lvl.Value.ToString())
                                {
                                    lvlTemp = _levelList[i];
                                }
                            }
                            lvlCol[lvlIdx].name = lvlTemp.name;
                            lvlCol[lvlIdx].boss = lvlTemp.boss;
                            lvlCol[lvlIdx].questions = lvlTemp.questions;
                            ++lvlIdx;
                        }
                    }

                    for (int i = 0; i < lvlCol.Length; ++i)
                    {
                        worldTemp.LvlData[i].name = lvlCol[i].name;
                        worldTemp.LvlData[i].boss = lvlCol[i].boss;
                        worldTemp.LvlData[i].questions = lvlCol[i].questions;
                    }

                    worldDataResult[wldIdx] = worldTemp;
                    Debug.Log("World #" + wldIdx + " loaded");
                    wldIdx++;
                }
            }
        });
        
        currentUser = await LoadUserData();
    }


    private async Task LoadLevelData()
    {
        await _levelReference.OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error loading levels!");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot allLvl = task.Result;
                IEnumerator<DataSnapshot> lvlEnumerator = allLvl.Children.GetEnumerator();
                while (lvlEnumerator.MoveNext())
                {
                    RoundData lvlResult=new RoundData();
                    lvlResult.questions=new List<QnA>();
                    IEnumerator<DataSnapshot> childEnumerator = lvlEnumerator.Current.Children.GetEnumerator();
                    //Iterates through all questions
                    while (childEnumerator.MoveNext())
                    {
                        DataSnapshot question = childEnumerator.Current;
                    
                        //Special cases for "boss", the enemy image sprite and "lv", level name
                        if (question.Key == "boss")
                        {
                            lvlResult.boss = question.Value.ToString();
                        }else if (question.Key == "lv")
                        {
                            lvlResult.name = question.Value.ToString();
                        }
                        else
                        {
                            for (int i = 0; i < _questionList.Count; ++i)
                            {
                                if (_questionList[i].questionId == question.Value.ToString())
                                {
                                    lvlResult.questions.Add(_questionList[i]);
                                    break;
                                }
                            }
                        }
                    }

                    lvlResult.lvlId = lvlEnumerator.Current.Key;
                    _levelList.Add(lvlResult);
                }
                
            }
        });
    }

    private async Task LoadQuestionData()
    {

        await _questionReference.OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error loading questions!");
            }
            else if (task.IsCompleted)
            {
                //DataSnapshot question = task.Result.Child(questionName);
                IEnumerator<DataSnapshot> questionEnumerator = task.Result.Children.GetEnumerator();
                //IEnumerator<DataSnapshot> childEnumerator = question.Children.GetEnumerator();

                //Iterates through all question components
                while (questionEnumerator.MoveNext())
                {
                    QnA questionResult=new QnA();
                    questionResult.ansChoice = new string[4];
                    int ansIdx = 0;
                    IEnumerator<DataSnapshot> childEnumerator = questionEnumerator.Current.Children.GetEnumerator();
                    Debug.Log(questionEnumerator.Current.Key);
                    while (childEnumerator.MoveNext())
                    {
                        DataSnapshot questionComponent = childEnumerator.Current;
                    
                        //Special cases for "qn", the question itself and the correct answer
                        if (questionComponent.Key == "qn")
                        {
                            questionResult.QuestionText=questionComponent.Value.ToString();
                        }
                        else if (questionComponent.Key == "correct")
                        {
                            questionResult.CorrectAns = int.Parse(questionComponent.Value.ToString());
                        }
                        else if (questionComponent.Key.ToLower() == "difficulty"){}
                        else
                        {
                            questionResult.ansChoice[ansIdx] = questionComponent.Value.ToString();
                            ++ansIdx;
                        }
                    }

                    questionResult.questionId = questionEnumerator.Current.Key;
                    _questionList.Add(questionResult);
                }
            }
        });
    }
    public async Task<User> LoadUserData()
    {
        User userResult = new User();
        await _userReference.OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Get User faulted");
            }
            else if(task.IsCompleted)
            {
                if (FirebaseAuth.DefaultInstance.CurrentUser != null)
                {
                    var userNode = task.Result.Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId);

                    userResult.usr = userNode.Child("usr").Value.ToString();
                    userResult.llv = userNode.Child("llv").Value.ToString();
                    userResult.chr = userNode.Child("chr").Value.ToString();  
                }
                else
                {
                    //Fallback user incase the user is found (should not be the case)
                    userResult.usr = "siaii";
                    userResult.llv = "1";
                    userResult.chr = "pipo-nekonin004";
                }
            }
        });
        return userResult;
    }

    public async void PostUserAttempt(Attempt currAttempt)
    {
        //Generates new entry with unique key in the database and saves the reference
        DatabaseReference newAttemptReference = _attemptReference.Push();
        
        //Sets the value of the new entry
        await newAttemptReference.SetRawJsonValueAsync(JsonUtility.ToJson(currAttempt));
    }

    public async void UpdateUserChar(string name)
    {
        await _userReference.Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("chr").SetValueAsync(name);
    }
    
}
