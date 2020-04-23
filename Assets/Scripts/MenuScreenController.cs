using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary>
/// This is the main controller that handles the user inputs during World and Level Selection.
/// It consists of all the dynamic changing UI components in the World and Level Selection scene.
/// </summary>
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
	[SerializeField] private GameObject exitPopup;
	[SerializeField] private GameObject creditsPopup;
	
	[SerializeField] private Sprite[] worldImageList;

	[SerializeField] private GameObject[] worldPrefabs;

	[SerializeField] private RuntimeAnimatorController[] animatorList;
	
	private GameObject activeWorld;
	private DataController _dataController;
	private RoundData currentRoundData;
	
	private string[] worldTopic=new string[]{"Planning", "Analysis, Design & Implementation", "Testing & Integration", "Maintenance", "General", "Levels"};

	/// <summary>
	/// During the Start() method of the Menu Screen Controller, it initialises the DataController, which stores the overall Game states.
	/// Using data retrieved from the DataController, it updates the Menu screen with the appropriate World image and greets the user using
	/// the appropriate user's username.
	/// </summary>
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
			_dataController.currentUser.llv = 0.ToString();
		}
		
		hiUser.text = "Hi, " + _dataController.currentUser.usr;

		//Change the shown user character
		int chrIdx = int.Parse(_dataController.currentUser.chr.Substring(12, 3));
		playerAnimator.runtimeAnimatorController = animatorList[chrIdx - 1];
		
		//Update the world image shown in the menu
		updateWorldShown(_dataController.getCurrWorld());
		
		//Handles which button is active in the menu
		if (_dataController.getCurrWorld() == 1)
		{
			backButton.SetActive(false);
			nextButton.SetActive(true);
		}else if (_dataController.getCurrWorld() == worldImageList.Length)
		{
			backButton.SetActive(true);
			nextButton.SetActive(false);
		}
		else
		{
			backButton.SetActive(true);
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

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			//Back button in android is mapped to escape button
			if (Input.GetKey(KeyCode.Escape))
			{
				//If in level select, go back to world select. If in world select, show exit confirmation
				if (levelBaseGameObject.activeSelf)
				{
					BackToWorld();
				}
				else
				{
					OnExitButton();
				}
			}
		}
	}

	/// <summary>
	/// SelectWorld() is the trigger function for the Select World button in the Menu Screen. It checks with the DataController on what is the current World
	/// that the user is looking at and instantiates the Level Selection screen from that world. If the user has selected World 6, it loads the scene for 
	/// Custom Levels.
	/// </summary>
	public void SelectWorld()
	{
		//If the current world is 6, the it is the custom level
		if (_dataController.getCurrWorld() > worldPrefabs.Length)
		{
			MenuScreenLoadParam.MenuLoadFromGame = false;
			SceneManager.LoadSceneAsync("CustomLevels");
		}
		else
		{
			//Resets camera position, instantiate the world object to show, and assign relevant variables.
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
	}

	/// <summary>
	/// BackToWorld() is the trigger function for the Back button in the Level Selection screen.
	/// It brings the user back to the World Selection screen by hiding most of the Level Selection UI
	/// </summary>
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

	/// <summary>
	/// This function updates the level name that is displayed on the user interface.
	/// </summary>
	public void changeLvl()
	{
		lvlName.text = _dataController.GetLevelName(_dataController.getCurrLevel());
	}

	/// <summary>
	/// This function is triggered by the Play button from the Level Selection screen.
	/// It updates the DataController to inform that the upcoming game is not a custom game and loads the
	/// Gameplay scene.
	/// </summary>
	public void StartGame()
	{
		_dataController.setCustom(false);
		//Saves current position before entering gameplay
		MenuScreenLoadParam.currentLevel = _dataController.getCurrLevel();
		SceneManager.LoadScene("Turn-Based");
	}

	/// <summary>
	/// This function is triggered when the user taps on a level during the Level Selection screen. It takes the level
	/// that the user has selected as a parameter. After the character sprite traverses
	/// to the Level on screen, this function triggers the Pop-Up informing the user of the World and Level and provides the user with
	/// the Play and Leaderboard buttons.
	/// </summary>
	/// <param name="level">This is the Level that the user has selected in the Level Selection screen. It ranges from 1-6</param>
	public void showLevelPopup(int level)
	{
		_dataController.setCurrLevel(level);
		changeLvl();
		levelPopup.SetActive(true);
		StartCoroutine(PopupAnim());
	}

	/// <summary>
	/// This function in charge of animating the level pop up
	/// </summary>
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

	/// <summary>
	/// This function in charge of closing the level pop up. This is often triggered when the user do not want to play the level and chooses to close the pop up.
	/// It does so by disabling the levelPopup UI component.
	/// </summary>
	public void closeLevelPopup()
	{
		levelPopup.SetActive(false);
	}


	/// <summary>
	/// This function is used to update the World Selection UI with the appropriate World number, World description and World Icon.
	/// </summary>
	/// <param name="world">The World number. Ranges from 1-6</param>
	private void updateWorldShown(int world)
	{
		var txts=worldImage.GetComponentsInChildren<TextMeshProUGUI>();
		for (var i = 0; i < txts.Length; ++i)
		{
			if (txts[i].name == "worldName")
			{
				if (world <= worldPrefabs.Length)
				{
					txts[i].text = "World " + world.ToString();
				}
				else
				{
					txts[i].text = "Custom";
				}
			}
			else if (txts[i].name == "worldTopic")
			{
				txts[i].text = worldTopic[world - 1];
			}
		}
		
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


	/// <summary>
	/// The trigger function for the Right arrow button in the World Selection screen. It updates the game state to the next World and calls on <code>updateWorldShown(nextWorld)</code>
	/// to update the UI.
	/// </summary>
	public void nextLevelButton()
	{
		int currWorld = _dataController.getCurrWorld();
		_dataController.setCurrWorld(++currWorld);
		
		//If the shown world is the last one, deactivate, else activate
		if (currWorld == 2)
		{
			backButton.SetActive(true);
		}else if (currWorld == worldPrefabs.Length+1)
		{
			nextButton.SetActive(false);
		}
		
		updateWorldShown(currWorld);
		
	}

	/// <summary>
	/// The trigger function for the Right arrow button in the World Selection screen. It updates the game state to the previous World and calls on <code>updateWorldShown(nextWorld)</code>
	/// to update the UI.
	/// </summary>
	public void backLevelButton()
	{
		int currWorld = _dataController.getCurrWorld();
		_dataController.setCurrWorld(--currWorld);
		
		//If the shown world is the first one, deactivate, else activate
		if (currWorld == 1)
		{
			backButton.SetActive(false);
		}else if (currWorld == worldPrefabs.Length)
		{
			nextButton.SetActive(true);
		}
		if (currWorld <= worldPrefabs.Length)
		{
			updateWorldShown(currWorld);
		}
	}

	/// <summary>
	/// The trigger function for the Leaderboard button that appears on the Level Pop Up after the user has selected a level.
	/// It loads the appropriate parameters to the MenuScreenLoadParam and calls on the Leaderboard scene for the Leaderboard display.
	/// </summary>
	public void LeaderboardButton()
	{
		//Saves the position of player before loading leaderboard
		MenuScreenLoadParam.MenuLoadFromGame = true;
		MenuScreenLoadParam.currentLevel = _dataController.getCurrLevel();
		SceneManager.LoadSceneAsync("Leaderboard");
	}

	/// <summary>
	/// The trigger function for the Settings button in the World Selection screen. It enables the settingsPopup UI component.
	/// </summary>
	public void OnSettingsPopupButton()
	{
		settingsPopup.SetActive(true);
	}

	/// <summary>
	/// The trigger function for the Mute button in the Settings Pop up. It mutes the game audio.
	/// </summary>
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

	/// <summary>
	/// The trigger function for the Character Selection button in the Settings Pop up. It loads the Character Selection scene.
	/// </summary>
	public void OnCharacterSelectButton()
	{
		MenuScreenLoadParam.CharacterLoadFromMenu = true;
		SceneManager.LoadSceneAsync("CharacterSelectionScreen");
	}

	/// <summary>
	/// The trigger function for the close button for the Settings Pop Up. It disables the settings pop up.
	/// </summary>
	public void OnCloseSettingsButton()
	{
		settingsPopup.SetActive(false);
	}

	/// <summary>
	/// The trigger function for the exit button. It enables the exit pop up which confirms with the user if he/she wants to quit the game.
	/// </summary>
	public void OnExitButton()
	{
		exitPopup.SetActive(true);
	}

	/// <summary>
	/// The trigger function for the Cancel button within the Exit Pop Up. It disables the exit pop up. 
	/// </summary>
	public void OnExitCancel()
	{
		exitPopup.SetActive(false);
	}

	/// <summary>
	/// The trigger function for the Confirm button within the Exit Pop Up. It quits the aplication.
	/// </summary>
	public void OnExitConfirm()
	{
		Application.Quit();
	}

	/// <summary>
	/// The trigger function for the Credits button within the Settings Pop Up. It opens the Credits UI pop up.
	/// </summary>
	public void OnCreditsButton()
	{
		creditsPopup.SetActive(true);
	}

	/// <summary>
	/// The trigger function for the close Credits button within the Credits Pop Up. It disables the Credits UI pop up.
	/// </summary>
	public void OnCreditsClose()
	{
		creditsPopup.SetActive(false);
	}
}
