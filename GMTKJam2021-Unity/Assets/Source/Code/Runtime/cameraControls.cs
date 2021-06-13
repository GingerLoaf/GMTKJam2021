using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    public GameManager GM;

    public float panBorder = 10f;
    [Range(5,10)]
    public float panSpeed = 5;
    [Range(5,10)]
    public float zoomSpeed = 5;
    public float range = 10f;
    public Vector3 camerapos;
    public Camera mainCamera;
    public Vector3 CameraDepth;
    public GameObject startPos;
    //public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = mainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mouse.current.position.ReadValue());
        Vector3 _mousePos = Mouse.current.position.ReadValue();
        Vector3 pos = mainCamera.transform.position;
        //camerapos = GetComponent<GameObject>(gm).transform.position;

        if (_mousePos.y <= 0 + panBorder)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (_mousePos.y >= Screen.height - panBorder)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (_mousePos.x <= 0 + panSpeed)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (_mousePos.x >= Screen.width - panSpeed)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (GM.FocusedObject != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                print("called");
                camerapos = GM.FocusedObject.transform.position;
                StartCoroutine(focusCamera());
            }
            else
            {
                mainCamera.transform.position = pos;
            }
        }
        else
        {
            mainCamera.transform.position = pos;
        }
        if (Mouse.current.scroll.ReadValue().y > 0)
        {
            float Zoom = Mouse.current.scroll.ReadValue().normalized.y * zoomSpeed;
            mainCamera.orthographicSize -= Zoom;
            float clampCamSize = Mathf.Clamp(mainCamera.orthographicSize, 7f, 80f);
            mainCamera.orthographicSize = clampCamSize;
        }
        if(Mouse.current.scroll.ReadValue().y < 0)
        {
            
            float Zoom = Mouse.current.scroll.ReadValue().normalized.y * -zoomSpeed;
            mainCamera.orthographicSize += Zoom;
            float clampCamSize = Mathf.Clamp(mainCamera.orthographicSize, 7f, 80f);
            mainCamera.orthographicSize = clampCamSize;
        }
        //Debug.Log(Mouse.current.scroll.ReadValue());

    }

    IEnumerator focusCamera()
    {
        print("called");
        mainCamera.transform.position = camerapos;
        yield break;
    }

}

