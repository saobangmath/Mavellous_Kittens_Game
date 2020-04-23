using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// The class responsible to load a new custom level  
/// </summary>
public class CustomLevelLoadController : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelIDField;
    [SerializeField] private TextMeshProUGUI lvlNotFoundTxt;

    private DataController _dataController;
   
    /// <summary>
    /// this function will called at first when the class instance is called 
    /// </summary>
    private void Start()
    {
        _dataController = FindObjectOfType<DataController>();
    }
    /// <summary>
    /// load the new CustomLevel scene 
    /// </summary>
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
    /// <summary>
    /// create new custom level 
    /// </summary>
    public void CreateLevel()
    {
        SceneManager.LoadSceneAsync("CreateCustom");
    }
    /// <summary>
    /// back to main menu for choosing other game scenario
    /// </summary>
    public void BackToMenu()
    {
        MenuScreenLoadParam.MenuLoadFromGame = false;
        SceneManager.LoadSceneAsync("MenuScreen");
    }
}
