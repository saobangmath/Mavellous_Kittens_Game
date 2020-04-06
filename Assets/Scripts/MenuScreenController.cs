using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuScreenController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI lvlName;
	[SerializeField] private TextMeshProUGUI hiUser;
	[SerializeField] private GameObject worldBaseGameObject;
	[SerializeField] private GameObject levelBaseGameObject;
	[SerializeField] private GameObject levelPopup;
	[SerializeField] private GameObject worldImage;
	[SerializeField] private GameObject backButton;
	[SerializeField] private GameObject nextButton;
	[SerializeField] private Camera camera;
	[SerializeField] private PlayerWorldMovement playerWorldMovement;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private GameObject settingsPopup;
	[SerializeField] private Button musicButton;
	[SerializeField] private TextMeshProUGUI worldName;
	[SerializeField] private Sprite unmuteButtonSprite;
	[SerializeField] private Sprite muteButtonSprite;

	[SerializeField] private Sprite[] worldImageList;

	[SerializeField] private GameObject[] worldPrefabs;

	[SerializeField] private RuntimeAnimatorController[] animatorList;
	
	private GameObject activeWorld;
	private DataController _dataController;
	private RoundData currentRoundData;
	

	void Start()
	{
		_dataController = FindObjectOfType<DataController>();    // Store a reference to the DataController so we can request the data we need for this round
		
		_dataController.LoadGameData();
		
		//For debugging purposes, won't be executed in real build
		if (_dataController.currentUser == null)
		{
			_dataController.currentUser=new User();
			_dataController.currentUser.usr = "siaii";
			_dataController.currentUser.chr = "pipo-nekonin002";
		}
		
		hiUser.text = "Hi, " + _dataController.currentUser.usr;

		//Change the shown user character
		int chrIdx = int.Parse(_dataController.currentUser.chr.Substring(12, 3));
		playerAnimator.runtimeAnimatorController = animatorList[chrIdx - 1];
		
		updateWorldShown(_dataController.getCurrWorld());
		if (_dataController.getCurrWorld() > 1)
		{
			backButton.SetActive(true);
		}

		if (_dataController.getCurrWorld() < worldPrefabs.Length)
		{
			nextButton.SetActive(true);
		}

		//If loading from gameplay, show level select instead of world select
		if (MenuScreenLoadParam.MenuLoadFromGame)
		{
			SelectWorld();
			worldBaseGameObject.SetActive(false);
			levelBaseGameObject.SetActive(true);
			MenuScreenLoadParam.MenuLoadFromGame = false;
		}
		
		
	}
	
	public void SelectWorld()
	{		
		Vector3 camPos = camera.transform.position;
		camPos.y = camera.GetComponent<CameraControl>().getMinY();
		camera.transform.position = camPos;
		activeWorld = Instantiate(worldPrefabs[_dataController.getCurrWorld() - 1], levelBaseGameObject.transform, false);
		playerWorldMovement.setWaypoints();
		playerWorldMovement.resetPlayer();
		worldName.text = "World "+_dataController.getCurrWorld().ToString();
		worldBaseGameObject.SetActive(false);
		levelBaseGameObject.SetActive(true);

	}

	public void BackToWorld()
	{
		//Resets the current level so that the next world loaded the player is at level 1
		MenuScreenLoadParam.currentLevel = 1;
		levelPopup.SetActive(false);
		levelBaseGameObject.SetActive(false);
		worldBaseGameObject.SetActive(true);
		
		//Destroy the current world prefab so it doesn't clash with the next world loaded
		Destroy(activeWorld);
		
		//Resets the camera position to the bottom
		Vector3 camPos = camera.transform.position;
		camPos.y = camera.GetComponent<CameraControl>().getMinY();
		camera.transform.position = camPos;
	}

	public void changeLvl()
	{
		lvlName.text = _dataController.GetLevelName(_dataController.getCurrLevel());
	}

	public void StartGame()
	{
		//Saves current position before entering gameplay
		MenuScreenLoadParam.currentLevel = _dataController.getCurrLevel();
		SceneManager.LoadScene("Turn-Based");
	}

	public void showLevelPopup(int level)
	{
		_dataController.setCurrLevel(level);
		changeLvl();
		levelPopup.SetActive(true);
		StartCoroutine(PopupAnim());
	}
	
	IEnumerator PopupAnim()
	{
		float t = 0;
		//Makes the popup has animation
		while (t <= 1)
		{
			float scale=Mathf.Lerp(0, 1, t);
			Vector3 currScale=new Vector3(scale,scale,1);
			levelPopup.transform.localScale = currScale;
			t += 3f*Time.deltaTime;
			yield return new WaitForSeconds(0.001f);
		}
	}

	public void closeLevelPopup()
	{
		levelPopup.SetActive(false);
	}

	private void updateWorldShown(int world)
	{
		worldImage.GetComponentInChildren<TextMeshProUGUI>().text = "World " + world.ToString();
		//Gets all children of worldImage and iterates all until it finds the one with the name "Image"
		Component[] list = worldImage.GetComponentsInChildren<Image>();
		foreach (Image img in list)
		{
			//Changes the show image for the shown world
			if (img.name == "Image")
			{
				img.preserveAspect = true;
				img.sprite = worldImageList[world - 1];
			}
		}
	}
	public void nextLevelButton()
	{
		int currWorld = _dataController.getCurrWorld();
		_dataController.setCurrWorld(++currWorld);
		
		//If the shown world is the last one, deactivate, else activate
		if (currWorld == 2)
		{
			backButton.SetActive(true);
		}else if (currWorld == worldPrefabs.Length)
		{
			nextButton.SetActive(false);
		}
		updateWorldShown(currWorld);
		
	}

	public void backLevelButton()
	{
		int currWorld = _dataController.getCurrWorld();
		_dataController.setCurrWorld(--currWorld);
		
		//If the shown world is the first one, deactivate, else activate
		if (currWorld == 1)
		{
			backButton.SetActive(false);
		}else if (currWorld == worldPrefabs.Length - 1)
		{
			nextButton.SetActive(true);
		}
		updateWorldShown(currWorld);
	}

	public void LeaderboardButton()
	{
		//Saves the position of player before loading leaderboard
		MenuScreenLoadParam.MenuLoadFromGame = true;
		MenuScreenLoadParam.currentLevel = _dataController.getCurrLevel();
		SceneManager.LoadSceneAsync("Leaderboard");
	}

	public void OnSettingsPopupButton()
	{
		settingsPopup.SetActive(true);
	}

	public void ToggleMuteButton()
	{
		MusicObjectScript audioSourceScript = FindObjectOfType<MusicObjectScript>();
		if (audioSourceScript.GetComponent<AudioSource>().mute)
		{
			musicButton.GetComponent<Image>().sprite = unmuteButtonSprite;
		}
		else
		{
			musicButton.GetComponent<Image>().sprite = muteButtonSprite;
		}
		audioSourceScript.ToggleMusic();
	}

	public void OnCharacterSelectButton()
	{
		MenuScreenLoadParam.CharacterLoadFromMenu = true;
		SceneManager.LoadSceneAsync("CharacterSelectionScreen");
	}

	public void OnCloseSettingsButton()
	{
		settingsPopup.SetActive(false);
	}
}
