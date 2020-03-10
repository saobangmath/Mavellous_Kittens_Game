using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{

    private int currentTurn = 0;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject enemy;

    private EnemyScript _enemyScript;
    private PlayerScript _playerScript;

    private bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
        _enemyScript = enemy.GetComponent<EnemyScript>();
        _playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //++currentTurn;
        if (Input.GetKeyUp(KeyCode.A))
        {
            _enemyScript.StartCoroutine("runAttack");
        }

    }

    public void EnemyAttack()
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(delayedEnemyAttack());
        }
    }

    public void PlayerAttack()
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(delayedPlayerAttack());

        }
    }

    IEnumerator delayedEnemyAttack()
    {
        _enemyScript.StartCoroutine("runAttack");   
        yield return new WaitForSeconds(2.1f);
        attacking = false;
    }

    IEnumerator delayedPlayerAttack()
    {
        _playerScript.StartCoroutine("runAttack");
        yield return new WaitForSeconds(3.2f);
        attacking = false;
    }
}
