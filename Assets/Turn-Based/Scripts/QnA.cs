using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QnA
{
    private String question;
    private String[] ansChoice = new String[4];
    private int correctAns;

    //The Question
    public string Question
    {
        get { return question; }
        set { question = value; }
    }

    //Array of answer choices (4)
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
    }
}
