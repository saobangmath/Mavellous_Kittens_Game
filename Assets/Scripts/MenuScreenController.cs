using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuScreenController : MonoBehaviour
{
	public GameObject popUpWindow;

	private DataController dataController;
	private RoundData currentRoundData;

	void Start() {
		dataController = FindObjectOfType<DataController>();                                // Store a reference to the DataController so we can request the data we need for this round

		GameObject.Find("LevelButton").GetComponentInChildren<Text>().text = dataController.GetLevelName();

		popUpWindow = GameObject.Find("PopUpWindow");
		popUpWindow.gameObject.SetActive(false);
	}

	public void SelectLevel() {
		popUpWindow.gameObject.SetActive(true);
	}

	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
	}
}
