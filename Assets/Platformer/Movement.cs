using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 250.0f;
    public float jumpForce = 10f;

    public Rigidbody2D body;
    private BoxCollider2D box;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;
        
        anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale=new Vector3( Mathf.Sign(deltaX), 1,1);
        }



        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        Vector2 corner1=new Vector2(max.x, min.y-0.1f);
        Vector2 corner2=new Vector2(min.x, min.y-0.1f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hit != null)
        {
            grounded = true;
            anim.SetBool("jump", false);
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            anim.SetBool("jump", true);
        }
    }
}
