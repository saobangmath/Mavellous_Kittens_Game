using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLevelLoadController : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelIDField;
    [SerializeField] private TextMeshProUGUI lvlNotFoundTxt;

    private DataController _dataController;

    private void Start()
    {
        _dataController = FindObjectOfType<DataController>();
    }

    public void LoadLevel()
    {
        string levelID = levelIDField.text.ToUpper();
        _dataController.setCustom(true);
        _dataController.setLvlID(levelID);
        if (_dataController.CheckLevelID(levelID))
        {
            SceneManager.LoadSceneAsync("Turn-Based");   
        }
        else
        {
            lvlNotFoundTxt.gameObject.SetActive(true);
        }
    }
    
    public void CreateLevel()
    {
        SceneManager.LoadSceneAsync("CreateCustom");
    }

    public void BackToMenu()
    {
        MenuScreenLoadParam.MenuLoadFromGame = false;
        SceneManager.LoadSceneAsync("MenuScreen");
    }
}
