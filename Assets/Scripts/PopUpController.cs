using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    private DataController dataController;
    private RoundData currentRoundData;

    public Text levelName;
    public Text description;
    public Text difficulty;
    public Text highScore;

    // Start is called before the first frame update
    void Start()
    {
      dataController = FindObjectOfType<DataController>();                                // Store a reference to the DataController so we can request the data we need for this round

      // print level details
      if (levelName != null) {
        levelName.text = dataController.GetLevelName();
        description.text = dataController.GetLevelDescription();
        difficulty.text = "Difficulty: " + dataController.GetLevelDifficulty().ToString();
        highScore.text = "High Score: " + dataController.GetHighestPlayerScore().ToString();
      }

    }
}
