using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    private Camera _camera;
    private float panSpeed = 10f;
    private Vector3 pointerPosLastFrame;

    private float maxY = 25f;
    private float minY = -5f;

    private bool isPanningLastFrame=false;
    // Start is called before the first frame update
    void Start()
    {
        _camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchSupported)
        {
            TouchControl();
        }
        else
        {
            MouseControl();
        }
    }

    void TouchControl()
    {
        Vector2 origPos;
        Vector2 lastPos;
        if (Input.touchCount == 1)
        {
            Touch fp=Input.GetTouch(0);
            
            if (fp.phase == TouchPhase.Began)
            {
                pointerPosLastFrame = Input.GetTouch(0).position;
                isPanningLastFrame = true;
            }

            if (fp.phase == TouchPhase.Moved)
            {
                Vector3 pointerPosThisFrame = Input.GetTouch(0).position;
                Vector3 pos = transform.position;
                Vector3 offset = pointerPosLastFrame - pointerPosThisFrame;

                pos.y += offset.y * 2 * Time.deltaTime;
                pos.y = Mathf.Clamp(pos.y, minY, maxY);

                transform.position = pos;

                pointerPosLastFrame = pointerPosThisFrame;
            }

            if (fp.phase == TouchPhase.Ended)
            {
                isPanningLastFrame = false;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(1))
        {
            pointerPosLastFrame = Input.mousePosition;
            isPanningLastFrame = true;
        }
        if (Input.GetMouseButton(1))
        {
                Vector3 pointerPosThisFrame = Input.mousePosition;
                Vector3 pos = transform.position;
                Vector3 offset = pointerPosLastFrame - pointerPosThisFrame;

                pos.y += offset.y * Time.deltaTime;
                pos.y = Mathf.Clamp(pos.y, minY, maxY);

                transform.position = pos;

                pointerPosLastFrame = pointerPosThisFrame;

        }

        if (Input.GetMouseButtonUp(1))
        {
            isPanningLastFrame = false;
        }
        
    }

    public float getMinY()
    {
        return minY;
    }
}
