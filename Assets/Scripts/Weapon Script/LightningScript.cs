using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    private Transform _currPos;

    // Start is called before the first frame update
    void Start()
    {
        _currPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _nextPos = _currPos.position;
        _nextPos.x += 4.5f *Time.deltaTime;
        transform.position = _nextPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(transform.parent.gameObject);            
        }
    }
}
