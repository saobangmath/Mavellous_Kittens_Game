using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class creates a serializable dictionary for enemy animations.
/// </summary>
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

    /// <summary>
    /// This method returns the animation controller of a desired name.
    /// </summary>
    /// <param name = "nm"> Name of animation controller </param>
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
