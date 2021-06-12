using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    public float panBorder = 10f;
    [Range(5,10)]
    public float panSpeed = 5;
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
        Debug.Log(Input.mousePosition);
        Vector3 pos = transform.position;
        //camerapos = GetComponent<GameObject>(gm).transform.position;

        if (Input.mousePosition.y <= 0 + panBorder)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - panBorder)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= 0 + panSpeed)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panSpeed)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(focusCamera());
        }
        transform.position = pos;

    }

    IEnumerator focusCamera()
    {
        mainCamera.transform.position = camerapos;
        yield break;
    }

}

