using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// This class handles the attack animations of the player and the effect on the enemy.
/// </summary>
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private HP_Bar bar;
    [SerializeField] private GameObject enemy;
    [SerializeField] private static int maxHP = 3;
    [SerializeField] private GameObject[] weapon;
    
    private int _hp = maxHP;
    private EnemyScript _enemyScript;
    private Animator _anim;

    public bool isAttacking = false;
    
    /// <summary>
    /// The Start() method initialises the Animator and Enemy Script.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _enemyScript = enemy.GetComponent<EnemyScript>();
    }

    /// <summary>
    /// This method spawns a weapon for the player to attack the enemy. It provides the animation of the player attack.
    /// </summary>
    IEnumerator runAttack()
    {
        isAttacking = true;
        
        //Spawns different weapon to attack enemy
        int randomWeaponIdx = UnityEngine.Random.Range(0, weapon.Length);
        GameObject temp=Instantiate(weapon[randomWeaponIdx], new Vector3(-1.5f,2,0), Quaternion.identity) as GameObject;
        temp.SetActive(true);
        
        //Wait until the weapon hits enemy and is destroyed
        yield return new WaitUntil(()=>temp==null);
        isAttacking = false;
    }

    /// <summary>
    /// This method decreases the health points in the healthpoint bar of the enemy
    /// </summary>
    public void DecreaseHP()
    {
        _anim.SetTrigger("hit");
        bar.StartCoroutine(bar.UpdateBar((float)_hp / (float)maxHP, (float)(_hp - 1) / (float)maxHP));
        _hp -= 1;
    }

    /// <summary>
    /// This method detects if there is a 2D collision.
    /// </summary>
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            DecreaseHP();   
        }
    }

    /// <summary>
    /// This method returns the health points of the player.
    /// </summary>
    /// <returns> Health points of the player </returns>
    public int GetCurrentHp()
    {
        return _hp;
    }
}
