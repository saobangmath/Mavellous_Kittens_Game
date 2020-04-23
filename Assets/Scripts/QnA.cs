using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class stores the details of each question (e.g. question text, index of correct answer).
/// </summary>
[System.Serializable]
public class QnA
{
    private String _questionText;
    public String[] ansChoice;
    private int _correctAns;
    public string questionId;
    
    public string QuestionText
    {
        get => _questionText;
        set => _questionText = value;
    }
    

    public int CorrectAns
    {
        get => _correctAns;
        set => _correctAns = value;
    }
    
}
