﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(TurnController))]
public class QuestionController : MonoBehaviour
{
    private TurnController _turnController;
    private DataController _dataController;
    private FirebaseScript _firebaseScript;
    private List<QnA> qData=new List<QnA>();
    [SerializeField] private TextMeshProUGUI questionTMP;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private TextMeshProUGUI timeRemainingDisplay;
    [SerializeField] private GameObject levelFinishPrompt;
    [SerializeField] private TextMeshProUGUI levelFinishTxt;
    [SerializeField] private Sprite baseButtonImage;
    [SerializeField] private Sprite correctButtonImage;
    [SerializeField] private Sprite wrongButtonImage;
    private int currQidx = 0;
    private int score = 0;
    private bool isRoundActive = false;
    private float timeRemaining;
    private RoundData currentRoundData;
    
    // Start is called before the first frame update
    private void Start()
    {
        _dataController = FindObjectOfType<DataController>();
        _firebaseScript = FindObjectOfType<FirebaseScript>();
        _turnController = GetComponent<TurnController>();

        
        if (_dataController != null)
        {
            //Different format of level id between custom level and default level
            if (_dataController.getCustom())
            {
                currentRoundData = _dataController.GetCurrentRoundData(_dataController.getLvlID());
            }
            else
            {
                currentRoundData= _dataController.GetCurrentRoundData(_dataController.getCurrLevel());
            }
            Debug.Log("qController Boss: "+currentRoundData.boss);
            for (int i = 0; i < currentRoundData.questions.Count; ++i)
            {
                qData.Add(currentRoundData.questions[i]);
            }            
        }
        timeRemaining = 30f;
        UpdateQuestion(currQidx);
        isRoundActive = true;
    }

    private void Update()
    {
        if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;                                                // If the round is active, subtract the time since Update() was last called from timeRemaining
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)                                                        // If timeRemaining is 0 or less, the round ends
            {
                EndRound();
            }
        }
    }

    private void UpdateQuestion(int index)
    {
        for (int i = 0; i < choiceButtons.Length; ++i)
        {
            choiceButtons[i].GetComponent<Image>().sprite = baseButtonImage;
        }
        questionTMP.text = qData[index].QuestionText;    //Changes the text of the question 
        for (int i = 0; i < currentRoundData.questions[index].ansChoice.Length; ++i)
        {
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = qData[index].ansChoice[i];     //Changes the text on answer buttons
        }
    }
    
    
    public void CheckAns(int choice)
    {
        isRoundActive = false;
        //Locks the user's choice by making all buttons not clickable
        for (int i = 0; i < currentRoundData.questions[currQidx].ansChoice.Length; ++i)
        {
            choiceButtons[i].interactable = false;
        }
        
        if (choice != qData[currQidx].CorrectAns)        //Checks if the chosen answer is correct
        {
            //When player chooses the wrong answer, shows the correct answer and marks player's answer as wrong and
            //enemy atacks
            choiceButtons[choice - 1].GetComponent<Image>().sprite = wrongButtonImage;
            for (int i = 0; i < choiceButtons.Length; ++i)
            {
                if (i == qData[currQidx].CorrectAns - 1)
                {
                    choiceButtons[i].GetComponent<Image>().sprite = correctButtonImage;
                }
            }
            _turnController.EnemyAttack();
        }
        else
        {
            //When player chooses the correct answer, increase user's score and player attacks
            score += (int) timeRemaining;
            choiceButtons[choice - 1].GetComponent<Image>().sprite = correctButtonImage;
            _turnController.PlayerAttack();
        }
    }
    
    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Mathf.Round(timeRemaining).ToString();
    }
    
    public void EndRound()
    {
        isRoundActive = false;
        timeRemaining = 30f;
        currQidx++;

        //If there are still questions in the level and the player is still alive, go to next question
        if (currQidx < currentRoundData.questions.Count && _turnController.GetPlayerHP()>0)

        {
            nextButton.SetActive(true);
        }
        else
        {
            LevelFinished();
        }
    }

    public void StartNewRound()
    {
        nextButton.SetActive(false);
        isRoundActive = true;
        UpdateQuestion(currQidx);
        
        //Make all the button clickable again
        for (int i = 0; i < currentRoundData.questions[currQidx].ansChoice.Length; ++i)
        {
            choiceButtons[i].interactable = true;
        }
    }

    private void LevelFinished()
    {
        PostScore();
        levelFinishPrompt.SetActive(true);
        //If player loses, don't increase llv
        if (_turnController.GetPlayerHP() == 0)
        {
            levelFinishTxt.text = "You Lose!";
        }
        else
        {
            _dataController.IncUserLLv();
        }
        levelFinishPrompt.GetComponentInChildren<TextMeshProUGUI>().text = "Score: "+score.ToString();
    }

    private void PostScore()
    {
        //If current user is the debug user
        if (_dataController.currentUser.usr != "siaii")
        {
            Attempt currAttempt;
            //Different lvl ID format of custom level and default level
            if (_dataController.getCustom())
            {
                currAttempt=new Attempt(_dataController.getCurrWorld(), _dataController.getLvlID(), score, FirebaseAuth.DefaultInstance.CurrentUser.UserId);
            }
            else
            {
                currAttempt=new Attempt(_dataController.getCurrWorld(), _dataController.getCurrLevel(), score, FirebaseAuth.DefaultInstance.CurrentUser.UserId);

            }
            _firebaseScript.PostUserAttempt(currAttempt);            
        }
    }
}
