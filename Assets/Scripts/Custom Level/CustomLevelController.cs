using System;
using System.Collections;
using Boo.Lang;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CustomLevelController : MonoBehaviour
{
    private DataController _dataController;
    private FirebaseScript _firebaseScript;

    private System.Collections.Generic.List<QnA> _questionCopy;
    private System.Collections.Generic.List<RoundData> _levelCopy;
    private EnemyAnimatorMap _enemyAnimatorMap;
    private ToggleContainer _toggleContainer;

    private int enemyChrIdx=0;

    private Vector3 enemyChrPos;
    [SerializeField] private GameObject templateToggle;
    [SerializeField] private GameObject enemySprite;
    [SerializeField] private string[] enemySpriteNames;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private GameObject confButton;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject lvlIdTxtBack;
    
    // Start is called before the first frame update
    void Start()
    {
        _dataController = FindObjectOfType<DataController>();
        _firebaseScript = FindObjectOfType<FirebaseScript>();
        _questionCopy = _firebaseScript.GetQuestionData();
        _levelCopy = _firebaseScript.GetLevelData();
        _enemyAnimatorMap = GetComponent<EnemyAnimatorMap>();
        _toggleContainer = GetComponent<ToggleContainer>();

        //Create toggle objects which acts as the container to be chosen for the question
        for (int i = 0; i < _questionCopy.Count; ++i)
        {
            GameObject tempToggle = Instantiate(templateToggle, templateToggle.transform.parent, false);
            tempToggle.SetActive(true);
            tempToggle.GetComponentInChildren<TextMeshProUGUI>().text=_questionCopy[i].QuestionText;
            _toggleContainer.AddToggle(tempToggle);
        }

        //Set default enemy sprite as "chicken"
        enemyChrPos = enemySprite.transform.position;
        UpdateEnemyChr(0);

    }

    public void OnNextEnemyButton()
    {
        if (enemyChrIdx == enemySpriteNames.Length - 1)
        {
            enemyChrIdx = 0;
        }
        else
        {
            enemyChrIdx++;
        }
        UpdateEnemyChr(enemyChrIdx);
    }

    public void OnPrevEnemyButton()
    {
        if (enemyChrIdx == 0)
        {
            enemyChrIdx = enemySpriteNames.Length-1;
        }
        else
        {
            enemyChrIdx--;
        }
        UpdateEnemyChr(enemyChrIdx);
    }

    private string GenerateLevelID()
    {
        //characters available as level id
        var chars = "ABCDEFGHIJKLMNOPQRSTUVW123456789";
        //Length of level id is 6
        var stringChars = new Char[6];
        var random = new System.Random();

        bool conflict;
        string resultString;
        do
        {
            conflict = false;
            //Choose 6 random characters as level ID
            for (int i = 0; i < stringChars.Length; ++i)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            
            resultString=new string(stringChars);
            
            //Checks if there are any other custom level with the same level ID (just in case)
            for (int i = 0; i < _levelCopy.Count; ++i)
            {
                if (_levelCopy[i].lvlId == resultString)
                {
                    conflict = true;
                    break;
                }
            }
        } while (conflict);

        return resultString;
    }

    public void OnFinishLevel()
    {
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        finishButton.SetActive(false);
        confButton.SetActive(true);
        cancelButton.SetActive(true);
        
        _toggleContainer.DeactivateOffToggle();
    }

    public void OnConfirmButton()
    {
        //Create the chosen enemy sprite and question into a new level object
        RoundData newLevel=new RoundData();
        newLevel.boss = enemySpriteNames[enemyChrIdx];
        newLevel.lvlId = GenerateLevelID();
        newLevel.name = newLevel.lvlId;
        foreach (var i in _toggleContainer.GetOnToggleIdx())
        {
            newLevel.questions.Add(_questionCopy[i]);
        }
        
        lvlIdTxtBack.SetActive(true);
        lvlIdTxtBack.GetComponentInChildren<TextMeshProUGUI>().text = "Level ID: " + newLevel.lvlId;
            
        _firebaseScript.AddLevel(newLevel);
    }

    public void OnCancelButton()
    {
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        finishButton.SetActive(true);
        confButton.SetActive(false);
        cancelButton.SetActive(false);
        
        _toggleContainer.ActivateAllToggle();
    }
    private void UpdateEnemyChr(int idx)
    {
        enemySprite.GetComponent<Animator>().runtimeAnimatorController =
            _enemyAnimatorMap.GetAnimatorController(enemySpriteNames[idx]);
        enemySprite.transform.position = enemyChrPos;
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("CustomLevels");
    }
    
}
