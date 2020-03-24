using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnController : MonoBehaviour
{

    private int currentTurn = 0;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject enemy;

    private EnemyScript _enemyScript;
    private PlayerScript _playerScript;

    private bool attacking = false;

    private QuestionController _questionController;
    // Start is called before the first frame update
    void Start()
    {
        _enemyScript = enemy.GetComponent<EnemyScript>();
        _playerScript = player.GetComponent<PlayerScript>();
        _questionController = GetComponent<QuestionController>();
    }

    // Update is called once per frame
    void Update()
    {


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
        yield return new WaitForSeconds(2.1f);        //Delay until the animation is finished
        attacking = false;
        _questionController.EndRound();
    }

    IEnumerator delayedPlayerAttack()
    {
        _playerScript.StartCoroutine("runAttack");
        yield return new WaitForSeconds(3.2f);    //Delay until the animation is finished
        attacking = false;
        _questionController.EndRound();
    }

    public void finishLvl()
    {
        MenuScreenLoadParam.MenuLoadFromGame = true;
        SceneManager.LoadSceneAsync("MenuScreen");
    }
}
