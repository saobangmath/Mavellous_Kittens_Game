using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator anim;
    
    [SerializeField] GameObject destRef;
        
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroyEnter()
    {
        GameObject dest = (GameObject) Instantiate(destRef);
        Destroy(gameObject);
        dest.transform.position = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && other.collider.attachedRigidbody.velocity.y < 0)
        {
            OnDestroyEnter();
        }
    }
}
