using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TurnController))]
public class QuestionController : MonoBehaviour
{
    private TurnController _turnController;
    private List<QnA> qData=new List<QnA>();
    [SerializeField] private TextMeshProUGUI questionTMP;
    [SerializeField] private TextMeshProUGUI[] choiceTMP;
    private int idx = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _turnController = GetComponent<TurnController>();
        QnA testQ=new QnA();
        testQ.Question = "1+1=?";
        testQ.AnswerChoice = new string[] {"a. 1", "b. 2", "c. 3", "d. 4"};
        testQ.CorrectAnswer = 2;
        qData.Add(testQ);
        UpdateQuestion(idx);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateQuestion(int index)
    {
        questionTMP.text = qData[index].Question;
        for (int i = 0; i < 4; ++i)
        {
            choiceTMP[i].text = qData[index].AnswerChoice[i];
        }
    }
    
    
    public void CheckAns(int choice)
    {
        if (choice != qData[idx].CorrectAnswer)
        {
            _turnController.EnemyAttack();
        }
        else
        {
            _turnController.PlayerAttack();
        }
    }
}
