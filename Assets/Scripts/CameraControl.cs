using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject levelBaseGameObject;
    [SerializeField] private GameObject levelPopup;
    private Camera _camera;
    private float panSpeed = 10f;
    private Vector3 pointerPosLastFrame;

    private float maxY = 42f;
    private float minY = -4f;

    private bool isPanningLastFrame=false;
    // Start is called before the first frame update
    void Start()
    {
        _camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Only enable camera movement when selecting level
        if (levelBaseGameObject.activeSelf && !levelPopup.activeSelf)
        {
            //MouseControl is mostly for debugging purposes, unless you want to plug a mouse to your phone
            if (Input.touchSupported)
            {
                TouchControl();
            }
            else
            {
                MouseControl();
            }   
        }
    }

    void TouchControl()
    {
        Vector2 origPos;
        Vector2 lastPos;
        
        //Using 1 finger to drag the camera
        if (Input.touchCount == 1)
        {
            Touch fp=Input.GetTouch(0);
            
            //Saves position of finger when it first touches the screen
            if (fp.phase == TouchPhase.Began)
            {
                pointerPosLastFrame = Input.GetTouch(0).position;
                isPanningLastFrame = true;
            }

            //Updates camera position based on movement of finger
            if (fp.phase == TouchPhase.Moved)
            {
                Vector3 pointerPosThisFrame = Input.GetTouch(0).position;
                Vector3 pos = transform.position;
                Vector3 offset = pointerPosLastFrame - pointerPosThisFrame;

                pos.y += 0.45f *offset.y * Time.deltaTime;
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
