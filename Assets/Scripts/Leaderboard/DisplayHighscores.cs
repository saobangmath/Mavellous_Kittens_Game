using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    public List<Transform> highscoreEntryTransformList = new List<Transform>(10);

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
    }

    public void DisplayHighscoreInTable(Highscores highscores)
    {
        //display objects on UI using template and container

        int k = 0;
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {

            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            k++;
            if (k > 9) break;
        }
    }

    public void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 31f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        entryTransform.gameObject.tag = "Respawn";

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        string score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // Highlight First
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank)
        {
            default:
                entryTransform.Find("trophy").gameObject.SetActive(false);
                break;
            case 1:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(1f, 0.82f, 0f, 1);
                break;
            case 2:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(0.78f, 0.78f, 0.78f, 1);
                break;
            case 3:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(0.717f, 0.435f, 0.337f, 1);
                break;

        }

        transformList.Add(entryTransform);
    }
}   
