using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The turn controller handles the animation that shows the player and enemy attacks.
/// </summary>

public class TurnController : MonoBehaviour
{

    private int currentTurn = 0;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject enemy;

    [SerializeField] private RuntimeAnimatorController[] animatorList;

    private EnemyScript _enemyScript;
    private PlayerScript _playerScript;

    private DataController _dataController;
    
    private bool attacking = false;
    private int playerHP=3;

    private QuestionController _questionController;

    private EnemyAnimatorMap _enemyAnimatorMap;
    
    /// <summary>
    /// In the Start() method, the enemy and player characters are first obtained. 
    /// This is for displaying the correct animation character later on. 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        _enemyScript = enemy.GetComponent<EnemyScript>();
        _playerScript = player.GetComponent<PlayerScript>();
        _questionController = GetComponent<QuestionController>();
        _enemyAnimatorMap = GetComponent<EnemyAnimatorMap>();
        _dataController = FindObjectOfType<DataController>();
        
        //Gets player character and enemy character info
        int chrIdx = int.Parse(_dataController.currentUser.chr.Substring(12, 3));
        string enemyName;
        if (_dataController.getCustom())
        {
            enemyName = _dataController.GetCurrentRoundData(_dataController.getLvlID()).boss;
        }
        else
        {
            enemyName = _dataController.GetCurrentRoundData(_dataController.getCurrLevel()).boss;
        }
        //Displays player character according to the player's choice
        player.GetComponent<Animator>().runtimeAnimatorController = animatorList[chrIdx-1];
        
        //Displays the enemy character based on the level data
        enemy.GetComponent<Animator>().runtimeAnimatorController = _enemyAnimatorMap.GetAnimatorController(enemyName);
        playerHP = _playerScript.GetCurrentHp();
    }

    /// <summary>
    /// This method calls the delayedEnemyAttack() method if there is no existing animation in progress.
    /// It also updates the player's health points after the attack.
    /// </summary>
    public void EnemyAttack()
    {
        if (!attacking)         //Checks if there is currently any attacking animation in progress
        {
            attacking = true;
            StartCoroutine(delayedEnemyAttack());
            playerHP = _playerScript.GetCurrentHp();
        }
    }

    /// <summary>
    /// This method calls the delayedPlayerAttack() method if there is no existing animation in progress.
    /// </summary>
    public void PlayerAttack()
    {
        if (!attacking)        //Checks if there is currently any attacking animation in progress
        {
            attacking = true;
            StartCoroutine(delayedPlayerAttack());

        }
    }

    /// <summary>
    /// This method calls the _enemyScript to run the attack on the character. It also ends the question once the attack ends.
    /// </summary>
    IEnumerator delayedEnemyAttack()
    {
        _enemyScript.StartCoroutine("runAttack");
        yield return new WaitUntil(() => _enemyScript.isAttacking == false);        //Delay until the animation is finished
        attacking = false;
        _questionController.EndRound();
    }

    /// <summary>
    /// This method calls the _playerScript to run the attack on the enemy. It also ends the question once the attack ends.
    /// </summary>
    IEnumerator delayedPlayerAttack()
    {
        _playerScript.StartCoroutine("runAttack");
        yield return new WaitUntil(() => _playerScript.isAttacking == false);    //Delay until the animation is finished
        attacking = false;
        _questionController.EndRound();
    }

    /// <summary>
    /// This method loads the relevant scenes (depending on whether the level is a custom level or not) once the entire level has ended.
    /// </summary>
    public void finishLvl()
    {
        //If this is a custom game, load back to custom levels scene, else go back to level select scene
        if (_dataController.getCustom())
        {
            MenuScreenLoadParam.MenuLoadFromGame = false;
            SceneManager.LoadSceneAsync("CustomLevels");
        }
        else
        {
            MenuScreenLoadParam.MenuLoadFromGame = true;    
            SceneManager.LoadSceneAsync("MenuScreen");
        }
        
    }

    /// <summary>
    /// This method loads the leaderboard for the level when the leaderboard button is selected.
    /// </summary>
    public void LeaderboardButton()
    {
        if (_dataController.getCustom())
        {
            MenuScreenLoadParam.MenuLoadFromGame = false;
        }
        else
        {
            MenuScreenLoadParam.MenuLoadFromGame = true;    
        }
        SceneManager.LoadSceneAsync("Leaderboard");
    }

    /// <summary>
    /// This method returns the health points of the player.
    /// </summary>
    /// <returns>Returns health points of player.</returns>
    public int GetPlayerHP()
    {
        return playerHP;
    }
}
