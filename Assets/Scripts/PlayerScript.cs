using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private HP_Bar bar;
    [SerializeField] private GameObject enemy;
    [SerializeField] private static int maxHP = 10;
    [SerializeField] private GameObject[] weapon;
    
    private int _hp = maxHP;
    private EnemyScript _enemyScript;
    private Animator _anim;

    public bool isAttacking = false;
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
        isAttacking = true;
        int randomWeaponIdx = UnityEngine.Random.Range(0, weapon.Length);
        GameObject temp=Instantiate(weapon[randomWeaponIdx], new Vector3(-1.5f,2,0), Quaternion.identity) as GameObject;
        temp.SetActive(true);
        _enemyScript.DeactivateWeapon();
        AnimatorStateInfo currAnimState = weapon[randomWeaponIdx].GetComponentInChildren<Animator>()
            .GetCurrentAnimatorStateInfo(0);
        yield return new WaitUntil(()=>temp==null);
        isAttacking = false;
    }

    public void DecreaseHP()
    {
        _anim.SetTrigger("hit");
        bar.StartCoroutine(bar.UpdateBar((float)_hp / (float)maxHP, (float)(_hp - 1) / (float)maxHP));
        _hp -= 1;        
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            DecreaseHP();   
        }
    }

    public void DeactivateWeapon()
    {
        //Legacy Function TODO remove this function
    }
}
