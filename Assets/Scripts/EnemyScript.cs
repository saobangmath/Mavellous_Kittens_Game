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
    [SerializeField] private static int maxHP = 10;
    private int _hp;
    private PlayerScript _playerScript;
    private DataController _dataController;
    public bool isAttacking = false;
    [SerializeField] private GameObject weapon;
    private bool complete = false;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //_anim.runtimeAnimatorController = 
        _playerScript = player.GetComponent<PlayerScript>();
        _dataController = FindObjectOfType<DataController>();
        if (_dataController != null) //Sanity check
        {
            maxHP = _dataController.GetCurrentRoundData(_dataController.getCurrLevel()).questions.Count;
        }

        _hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //For debugging purposes
        if (Input.GetKey(KeyCode.Space))
        {
            _anim.SetTrigger("attack");
        }

        if (Input.GetKey(KeyCode.Return))
        {
            _anim.SetTrigger("attack");
        }
    }

    IEnumerator runAttack()
    {
        isAttacking = true;
        _anim.SetTrigger("attack");
        weapon.SetActive(true);
        
        //Wait until the attack animation is finished
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }

    public void DecreaseHP()
    {
        _anim.SetTrigger("hit");
        bar.StartCoroutine(bar.UpdateBar((float)-_hp / (float)maxHP, (float)-(_hp - 1) / (float)maxHP));
        _hp -= 1;
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            DecreaseHP(); 
        }
    }

    public void DeactivateWeapon()
    {
        weapon.SetActive(false);
    }
}
