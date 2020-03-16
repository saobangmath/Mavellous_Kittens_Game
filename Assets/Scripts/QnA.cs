using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QnA
{
    public String questionText;
    public String[] ansChoice;
    public int correctAns;

    //The question
    /*public string Question
    {
        get { return questionText; }
        set { questionText = value; }
    }

    //Array of answer choices
    public int CorrectAnswer
    {
        get { return correctAns;}
        set { correctAns = value; }
    }

    //The correct choice (1 for a, 2 for b, 3 for c, 4 for d)
    public String[] AnswerChoice
    {
        get { return ansChoice; }
        set { ansChoice = value; }
    }*/
}
