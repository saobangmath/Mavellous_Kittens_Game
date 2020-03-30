using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnController : MonoBehaviour
{

    private int currentTurn = 0;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject enemy;

    [SerializeField] private AnimatorController[] animatorList;

    private EnemyScript _enemyScript;
    private PlayerScript _playerScript;

    private DataController _dataController;
    
    private bool attacking = false;

    private QuestionController _questionController;
    // Start is called before the first frame update
    void Start()
    {
        _enemyScript = enemy.GetComponent<EnemyScript>();
        _playerScript = player.GetComponent<PlayerScript>();
        _questionController = GetComponent<QuestionController>();

        _dataController = FindObjectOfType<DataController>();
        
        int chrIdx = int.Parse(_dataController.currentUser.chr.Substring(12, 3));
        //TODO Create all character's animation
        
        player.GetComponent<Animator>().runtimeAnimatorController = animatorList[chrIdx-1];
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
}
