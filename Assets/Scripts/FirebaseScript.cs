using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class FirebaseScript : MonoBehaviour
{
    private string[] presetWorldList=new string[]{"world1", "world2", "world3", "world4", "world5"};


    private DatabaseReference _reference;
    private DatabaseReference _worldReference;
    private DatabaseReference _levelReference;
    private DatabaseReference _questionReference;


    
    private worldData[] worldDataResult;
    
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://marvellous-kittens.firebaseio.com/");
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
        _worldReference = _reference.Child("Worlds");
        _levelReference = _reference.Child("Levels");
        _questionReference = _reference.Child("Questions");
        
        

    }

    private async void Start()
    {
        worldDataResult= new worldData[presetWorldList.Length];
        
        for (int i = 0; i < worldDataResult.Length; ++i)
        {
            if (worldDataResult[i] == null)
            {
                worldDataResult[i]=new worldData();
            }
        }
        
        await LoadGameData();
        
        Debug.Log("Data Finished Loading");
    }

    public worldData[] GetWorldData()
    {
        if (worldDataResult[0] == null)
        {
            Debug.Log("getWorldData0 null");
        }
        else if (worldDataResult[0].WorldName == null)
        {
            Debug.Log("worldDataResult name null");
        }
        else if (worldDataResult[0].LvlData[0].name == null)
        {
            Debug.Log("worldDataResult.LvlData.name null");
        }
        return worldDataResult;
    }
    
    

    public async Task LoadGameData()
    {
        worldData worldTemp = new worldData();
        worldTemp.LvlData=new RoundData[6];
        RoundData[] lvlCol=new RoundData[6];
        for (int i = 0; i < lvlCol.Length; ++i)
        {
            worldTemp.LvlData[i]=new RoundData();
            lvlCol[i]=new RoundData();
        }
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
                
                //Iterates through all child
                while (worldEnumerator.MoveNext())
                {
                    DataSnapshot world = worldEnumerator.Current;
                    
                    //Process only preset world's data (custom level is handled by other function)
                    if (presetWorldList.Contains(world.Key))
                    {

                        //Var to track current level being loaded
                        int wldIdx = 0;
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
                                RoundData lvlTemp = LoadLevelData(lvl.Value.ToString()).Result;
                                Debug.Log(lvlTemp.name);
                                Debug.Log(lvlTemp.questions.Count.ToString());
                                lvlCol[lvlIdx].name = lvlTemp.name;
                                lvlCol[lvlIdx].questions = lvlTemp.questions;
                                ++lvlIdx;
                            }
                        }
                        Debug.Log("lvlcol name: "+lvlCol[0].name);
                        for (int i = 0; i < lvlCol.Length; ++i)
                        {
                            worldTemp.LvlData[i].name = lvlCol[i].name;
                            worldTemp.LvlData[i].questions = lvlCol[i].questions;
                        }
                        Debug.Log("worldTemp lvl name:"+worldTemp.LvlData[0].name);
                        worldDataResult[wldIdx] = worldTemp;
                        Debug.Log("worldResult lvl name: "+ worldDataResult[wldIdx].LvlData[0].name);
                        ++wldIdx;
                    }
                }
            }
        });
    }


    private async Task<RoundData> LoadLevelData(string lvlName)
    {
        RoundData lvlResult=new RoundData();
        lvlResult.questions=new List<QnA>();
        await _levelReference.OrderByKey().EqualTo(lvlName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error loading levels!");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot lvl = task.Result.Child(lvlName);
                //Debug.Log(lvlName + " : " + lvl.ChildrenCount.ToString());
                IEnumerator<DataSnapshot> childEnumerator = lvl.Children.GetEnumerator();

                //int qIdx = 0;
                
                //Iterates through all questions
                while (childEnumerator.MoveNext())
                {
                    DataSnapshot question = childEnumerator.Current;
                    
                    //Special cases for "boss", the enemy image sprite and "lv", level name
                    if (question.Key == "boss")
                    {
                        continue;    //TODO Feature not implemented yet, load enemy sprite based on here
                    }else if (question.Key == "lv")
                    {
                        lvlResult.name = question.Value.ToString();
                        Debug.Log("in: "+lvlResult.name);
                    }
                    else
                    {
                        lvlResult.questions.Add(LoadQuestionData(question.Value.ToString()).Result);
                        //++qIdx;
                    }
                }
            }
        });
        Debug.Log("out: "+lvlResult.name);
        Debug.Log("q: "+lvlResult.questions[0].QuestionText);
        return lvlResult;
    }

    private async Task<QnA> LoadQuestionData(string questionName)
    {
        QnA questionResult=new QnA();
        questionResult.ansChoice = new string[4];
        await _questionReference.OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Error loading questions!");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot question = task.Result.Child(questionName);

                IEnumerator<DataSnapshot> childEnumerator = question.Children.GetEnumerator();

                int ansIdx = 0;
                Debug.Log(questionName);
                //Iterates through all question components
                while (childEnumerator.MoveNext())
                {
                    DataSnapshot questionComponent = childEnumerator.Current;
                    
                    //Special cases for "qn", the question itself and the correct answer
                    if (questionComponent.Key == "qn")
                    {
                        Debug.Log("questiontxt");
                        questionResult.QuestionText=questionComponent.Value.ToString();
                    }
                    else if (questionComponent.Key == "correct")
                    {
                        Debug.Log("ansnum");
                        questionResult.CorrectAns = int.Parse(questionComponent.Value.ToString());
                    }
                    else if (questionComponent.Key == "difficulty")
                    {
                        
                    }
                    else
                    {
                        Debug.Log("questionchoice");
                        questionResult.ansChoice[ansIdx] = questionComponent.Value.ToString();
                        ++ansIdx;
                    }
                }
            }
        });
        return questionResult;
    }
}
