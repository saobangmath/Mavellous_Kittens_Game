using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QnA
{
    private String questionText;
    public String[] ansChoice;
    private int correctAns;
    
    public string QuestionText
    {
        get => questionText;
        set => questionText = value;
    }
    

    public int CorrectAns
    {
        get => correctAns;
        set => correctAns = value;
    }
    
}
