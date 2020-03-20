using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(TurnController))]
public class QuestionController : MonoBehaviour
{
    private TurnController _turnController;
    private DataController _dataController;
    private List<QnA> qData=new List<QnA>();
    [SerializeField] private TextMeshProUGUI questionTMP;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private TextMeshProUGUI timeRemainingDisplay;
    [SerializeField] private GameObject levelFinishPrompt;
    [SerializeField] private Sprite baseButtonImage;
    [SerializeField] private Sprite correctButtonImage;
    [SerializeField] private Sprite wrongButtonImage;
    private int currQidx = 0;
    private int score = 0;
    private bool isRoundActive = false;
    private float timeRemaining;
    private RoundData currentRoundData;
    
    // Start is called before the first frame update
    void Start()
    {
        _dataController = FindObjectOfType<DataController>();
        _turnController = GetComponent<TurnController>();

        if (_dataController != null)
        {
            currentRoundData= _dataController.GetCurrentRoundData(_dataController.getCurrLevel());
            for (int i = 0; i < currentRoundData.questions.Length; ++i)
            {
                addQuestionData(i);
            }            
        }
        else
        {
            QnA testQ=new QnA();
            testQ.questionText = "1+1=?";        //Test question to remove later
            testQ.ansChoice = new string[] {"a. 1", "b. 2", "c. 3", "d. 4"};
            testQ.correctAns = 2;
            qData.Add(testQ);
        }
        timeRemaining = 20f;
        UpdateQuestion(currQidx);
        isRoundActive = true;
    }

    void Update()
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

    void UpdateQuestion(int index)
    {
        for (int i = 0; i < choiceButtons.Length; ++i)
        {
            choiceButtons[i].GetComponent<Image>().sprite = baseButtonImage;
        }
        questionTMP.text = qData[index].questionText;    //Changes the text of the question 
        for (int i = 0; i < currentRoundData.questions[currQidx].ansChoice.Length; ++i)
        {
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = qData[index].ansChoice[i];     //Changes the text on answer buttons
        }
    }
    
    
    public void CheckAns(int choice)
    {
        for (int i = 0; i < currentRoundData.questions[currQidx].ansChoice.Length; ++i)
        {
            choiceButtons[i].interactable = false;
        }
        if (choice != qData[currQidx].correctAns)        //Checks if the chosen answer is correct
        {
            choiceButtons[choice - 1].GetComponent<Image>().sprite = wrongButtonImage;
            for (int i = 0; i < choiceButtons.Length; ++i)
            {
                if (i == qData[currQidx].correctAns - 1)
                {
                    choiceButtons[i].GetComponent<Image>().sprite = correctButtonImage;
                }
            }
            _turnController.EnemyAttack();
        }
        else
        {
            ++score;
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
        if (currQidx < currentRoundData.questions.Length)
        {
            nextButton.SetActive(true);
        }
        else
        {
            levelFinished();
        }
    }

    private void addQuestionData(int idx)
    {
        QnA currRoundData = _dataController.GetCurrentRoundData(_dataController.getCurrLevel()).questions[idx];
        qData.Add(currRoundData);
    }

    private void FinishLevel()
    {
        Debug.Log("level won");
        SceneManager.LoadScene("MenuScreen");
    }

    public void StartNewRound()
    {
        nextButton.SetActive(false);
        isRoundActive = true;
        UpdateQuestion(currQidx);
        for (int i = 0; i < currentRoundData.questions[currQidx].ansChoice.Length; ++i)
        {
            choiceButtons[i].interactable = true;
        }
    }

    private void levelFinished()
    {
        levelFinishPrompt.SetActive(true);
        levelFinishPrompt.GetComponentInChildren<TextMeshProUGUI>().text = "Score: "+score.ToString();

    }
}
