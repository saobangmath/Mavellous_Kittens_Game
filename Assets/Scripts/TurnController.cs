using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public void EnemyAttack()
    {
        if (!attacking)         //Checks if there is currently any attacking animation in progress
        {
            attacking = true;
            StartCoroutine(delayedEnemyAttack());
        }
    }

    public void PlayerAttack()
    {
        if (!attacking)        //Checks if there is currently any attacking animation in progress
        {
            attacking = true;
            StartCoroutine(delayedPlayerAttack());

        }
    }

    IEnumerator delayedEnemyAttack()
    {
        _enemyScript.StartCoroutine("runAttack");
        yield return new WaitUntil(() => _enemyScript.isAttacking == false);        //Delay until the animation is finished
        attacking = false;
        playerHP = _playerScript.GetCurrentHp();
        _questionController.EndRound();
    }

    IEnumerator delayedPlayerAttack()
    {
        _playerScript.StartCoroutine("runAttack");
        yield return new WaitUntil(() => _playerScript.isAttacking == false);    //Delay until the animation is finished
        attacking = false;
        _questionController.EndRound();
    }

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

    public int GetPlayerHP()
    {
        return playerHP;
    }
}
