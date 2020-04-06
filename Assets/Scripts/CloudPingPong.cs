using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPingPong : MonoBehaviour
{
    [SerializeField] private float minX=-5.6f;
    [SerializeField] private float maxX=19f;
    private float t;
    private float len;
    private bool goRight = true;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        len = maxX - minX;
        t = (pos.x - minX) / len;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PingPong());
    }

    private IEnumerator PingPong()
    {   
        //Makes the cloud go back and forth in the background
        pos.x = minX + len * Mathf.Lerp(0, 40, t) / 40;
        if (goRight)
        {
            t += 0.001f;
        }
        else
        {
            t -= 0.001f;
        }

        if (t >= 1 || t <= 0)
        {
            goRight = !goRight;
        }

        transform.position = pos;

        yield return null;
    }
}
