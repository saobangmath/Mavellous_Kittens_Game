using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MenuScreenController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI lvlName;
	[SerializeField] private TextMeshProUGUI lvlDifficulty;
	[SerializeField] private TextMeshProUGUI lvlHighScore;
	private DataController dataController;
	private RoundData currentRoundData;

	void Start() {
		dataController = FindObjectOfType<DataController>();                                // Store a reference to the DataController so we can request the data we need for this round
		changeLvl();
		
		//GameObject.FindWithTag("LevelButton").GetComponentInChildren<Text>().text = dataController.GetLevelName();

		//popUpWindow = GameObject.Find("PopUpWindow");
		//popUpWindow.gameObject.SetActive(false);
	}

	public void SelectLevel() {
		//popUpWindow.gameObject.SetActive(true);
	}

	public void changeLvl()
	{
		lvlName.text = dataController.GetLevelName();
		lvlDifficulty.text = "Difficulty: " + dataController.GetLevelDifficulty().ToString();
		lvlHighScore.text = "High Score: " + dataController.GetHighestPlayerScore().ToString();
	}

	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Turn-Based");
	}
}
