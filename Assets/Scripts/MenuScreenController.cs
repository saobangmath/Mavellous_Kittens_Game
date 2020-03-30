using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.Animations;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuScreenController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI lvlName;
	[SerializeField] private TextMeshProUGUI lvlDifficulty;
	[SerializeField] private TextMeshProUGUI lvlHighScore;
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

	[SerializeField] private Sprite[] worldImageList;

	[SerializeField] private GameObject[] worldPrefabs;

	[SerializeField] private AnimatorController[] animatorList;
	
	private GameObject activeWorld;
	private DataController _dataController;
	private RoundData currentRoundData;
	

	void Start()
	{
		_dataController = FindObjectOfType<DataController>();    // Store a reference to the DataController so we can request the data we need for this round
		
		_dataController.LoadGameData();
		
		if (_dataController.currentUser == null)
		{
			_dataController.currentUser=new User();
			_dataController.currentUser.usr = "siaii";
			_dataController.currentUser.chr = "pipo-nekonin002";
		}
		hiUser.text = "Hi, " + _dataController.currentUser.usr;

		int chrIdx = int.Parse(_dataController.currentUser.chr.Substring(12, 3));

		playerAnimator.runtimeAnimatorController = animatorList[chrIdx - 1];
		
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
		worldBaseGameObject.SetActive(false);
		levelBaseGameObject.SetActive(true);

	}

	public void BackToWorld()
	{
		levelPopup.SetActive(false);
		levelBaseGameObject.SetActive(false);
		worldBaseGameObject.SetActive(true);
		
		Destroy(activeWorld);
		
		Vector3 camPos = camera.transform.position;
		camPos.y = camera.GetComponent<CameraControl>().getMinY();
		camera.transform.position = camPos;
	}

	public void changeLvl()
	{
		lvlName.text = _dataController.GetLevelName(_dataController.getCurrLevel());
		lvlHighScore.text = "High Score: " + _dataController.GetHighestPlayerScore().ToString();
	}

	public void StartGame()
	{
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
		while (t <= 1)
		{
			float scale=Mathf.Lerp(0, 1, t);
			Vector3 currScale=new Vector3(scale,scale,1);
			levelPopup.transform.localScale = currScale;
			t += 0.05f;
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void closeLevelPopup()
	{
		levelPopup.SetActive(false);
	}

	private void updateWorldShown(int world)
	{
		worldImage.GetComponentInChildren<TextMeshProUGUI>().text = "World " + world.ToString();
		Component[] list = worldImage.GetComponentsInChildren<Image>();
		foreach (Image img in list)
		{
			if (img.name == "Image")
			{
				img.sprite = worldImageList[world - 1];
			}
		}
	}
	public void nextLevelButton()
	{
		int currWorld = _dataController.getCurrWorld();
		_dataController.setCurrWorld(++currWorld);
		if (currWorld == 2)
		{
			backButton.SetActive(true);
		}
		//TODO else if currWorld==max, nextButton set active false
		updateWorldShown(currWorld);
		
	}

	public void backLevelButton()
	{
		int currWorld = _dataController.getCurrWorld();
		_dataController.setCurrWorld(--currWorld);
		if (currWorld == 1)
		{
			backButton.SetActive(false);
		}
		updateWorldShown(currWorld);
	}
	
}
