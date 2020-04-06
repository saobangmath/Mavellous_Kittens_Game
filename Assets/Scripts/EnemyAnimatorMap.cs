using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorMap : MonoBehaviour
{
    //Basically a serializable dictionary for the enemy animations
    [System.Serializable]
    public class EnemyAnimatorEntry
    {
        public string name;
        public RuntimeAnimatorController anim;
    }

    public EnemyAnimatorEntry[] enemyAnimation;

    public RuntimeAnimatorController GetAnimatorController(string nm)
    {
        for (int i = 0; i < enemyAnimation.Length; ++i)
        {
            Debug.Log(enemyAnimation[i].name);
            if (enemyAnimation[i].name == nm)
            {
                return enemyAnimation[i].anim;
            }
        }

        return enemyAnimation[0].anim;
    }
}
