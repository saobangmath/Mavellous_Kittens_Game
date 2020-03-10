using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPiece : MonoBehaviour
{
    [SerializeField] private Vector2 forceDirection;

    private Rigidbody2D rb2d;

    [SerializeField] private float forceMag;
    [SerializeField] private int torque;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(forceDirection*forceMag);
        rb2d.AddTorque(torque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
