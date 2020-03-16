using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyScript : MonoBehaviour
{
    
    private Animator _anim;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private GameObject player;
    [SerializeField] private HP_Bar bar;
    [SerializeField] private RuntimeAnimatorController[] enemyAnim;
    [SerializeField] private static int maxHP = 10;
    private int _hp;
    private PlayerScript _playerScript;
    private DataController _dataController;

    private bool complete = false;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim.runtimeAnimatorController = enemyAnim[1];
        _playerScript = player.GetComponent<PlayerScript>();
        _dataController = FindObjectOfType<DataController>();
        if (_dataController != null) //Sanity check
        {
            maxHP = _dataController.GetCurrentRoundData().questions.Length;
        }

        _hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _anim.SetTrigger("attacked");
        }

        if (Input.GetKey(KeyCode.Return))
        {
            _anim.SetBool("run", !_anim.GetBool("run"));
        }
    }

    IEnumerator runAttack()
    {
        _anim.SetTrigger("attack");
        yield return new WaitForSeconds(1.0f);        //Delay so that the animation and hp decrease looks natural
        _playerScript.DecreaseHP();
        yield return false;
    }

    public void DecreaseHP()
    {
        _anim.SetTrigger("hit");
        bar.StartCoroutine(bar.UpdateBar((float)-_hp / (float)maxHP, (float)-(_hp - 1) / (float)maxHP));
        _hp -= 1;
    }

}
