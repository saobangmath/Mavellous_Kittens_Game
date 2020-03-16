using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Animator _anim;

    [SerializeField] private HP_Bar bar;

    [SerializeField]
    private GameObject enemy;
    [SerializeField] private static int maxHP = 10;
    private int _hp = maxHP;

    private EnemyScript _enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _enemyScript = enemy.GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator runAttack()
    {
        _anim.SetTrigger("attack");
        yield return new WaitForSeconds(1.5f);
        _enemyScript.DecreaseHP();
    }

    public void DecreaseHP()
    {
        _anim.SetTrigger("hit");
        bar.StartCoroutine(bar.UpdateBar((float)_hp / (float)maxHP, (float)(_hp - 1) / (float)maxHP));
        _hp -= 1;        
    }
}
