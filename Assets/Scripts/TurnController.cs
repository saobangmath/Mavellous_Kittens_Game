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
        
        int chrIdx = int.Parse(_dataController.currentUser.chr.Substring(12, 3));
        string enemyName = _dataController.GetCurrentRoundData(_dataController.getCurrLevel()).boss;
        
        player.GetComponent<Animator>().runtimeAnimatorController = animatorList[chrIdx-1];
        Debug.Log("Enemy: "+enemyName);
        enemy.GetComponent<Animator>().runtimeAnimatorController = _enemyAnimatorMap.GetAnimatorController(enemyName);
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
        MenuScreenLoadParam.MenuLoadFromGame = true;
        SceneManager.LoadSceneAsync("MenuScreen");
    }

    public void LeaderboardButton()
    {
        MenuScreenLoadParam.MenuLoadFromGame = true;
        SceneManager.LoadSceneAsync("Leaderboard");
    }
}
