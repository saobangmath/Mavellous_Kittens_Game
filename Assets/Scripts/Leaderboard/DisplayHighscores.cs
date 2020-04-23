using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// the class DisplayHighscores responsible for displaying the highscores
/// </summary>
public class DisplayHighscores : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    public List<Transform> highscoreEntryTransformList = new List<Transform>(10);
    /// <summary>
    /// the Awake() function is triggered set up required variables for displaying highscores
    /// </summary>
    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
    }
}   
