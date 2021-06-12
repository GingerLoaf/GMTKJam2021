using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Camera myCamera;
    
    [SerializeField]
    private IntReference totalOxygen;
    [SerializeField]
    private IntReference totalMetal;
    [SerializeField]
    private NavMeshAgent terrariumAgent = null;
    [SerializeField]
    Color highlightColor = Color.white;
    [SerializeField]
    Color SelectedColor = Color.black;

    bool hasPressedMouse = false;
    private GameObject lastSelectedObject = null, lastHighlightedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        hasPressedMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray _mouseRay = myCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _interactionHit;
        Debug.DrawRay(_mouseRay.origin, _mouseRay.direction * 10);
        if (Physics.Raycast(_mouseRay, out _interactionHit))
        {
            if (lastHighlightedObject == null && _interactionHit.collider.GetComponent<Outline>())
            {
                _interactionHit.collider.GetComponent<Outline>().enabled = true;
                _interactionHit.collider.GetComponent<Outline>().OutlineColor = highlightColor;
                lastHighlightedObject = _interactionHit.collider.gameObject;
            }
            else if(lastHighlightedObject != null && lastHighlightedObject != _interactionHit.collider.gameObject)
            {
                if (lastSelectedObject == null)
                {
                    lastHighlightedObject.GetComponent<Outline>().enabled = false;
                    lastHighlightedObject = null;
                }
                else
                {
                    lastSelectedObject.GetComponent<Outline>().OutlineColor = SelectedColor;
                    lastHighlightedObject = null;
                }
            }
            if (Mouse.current.leftButton.IsPressed())
            {
                if (!hasPressedMouse)
                {
                    GameObject _obj = _interactionHit.transform.gameObject;
                    //print(_interactionHit.transform.gameObject);
                    switch (_interactionHit.collider.tag)
                    {
                        case "terrian":
                            if (lastSelectedObject != null)
                            {
                                if (lastSelectedObject.GetComponent<NavMeshAgent>())
                                {
                                    print("called 2");
                                    if (IsOnMesh(_interactionHit.point, lastSelectedObject.GetComponent<NavMeshAgent>()))
                                    {
                                        print("is Moving");
                                    }
                                }
                            }
                            break;

                        case "unit":
                            if (lastSelectedObject == null || lastSelectedObject != _obj)
                            {
                                if (lastSelectedObject != null && lastSelectedObject != _obj)
                                {
                                    lastSelectedObject.GetComponent<Outline>().enabled = false;
                                }
                                lastSelectedObject = _obj;
                            }
                            //expand unit interaction
                            break;

                        case "terrarium":
                            if (lastSelectedObject == null || lastSelectedObject != _obj)
                            {
                                if (lastSelectedObject != null && lastSelectedObject != _obj)
                                {
                                    lastSelectedObject.GetComponent<Outline>().enabled = false;
                                }
                                lastSelectedObject = _obj;
                            }
                            //expand terrarium interaction
                            break;
                        default:
                            print("Not Interactable");
                            break;
                    }
                    hasPressedMouse = true;
                }
                
            }
            else
            {
                if (hasPressedMouse)
                {
                    hasPressedMouse = false;
                }
            }

        }
        else if(lastHighlightedObject != null)
        {
            if (lastHighlightedObject.GetComponent<Outline>().enabled)
            {
                lastHighlightedObject.GetComponent<Outline>().enabled = false;
            }
            lastHighlightedObject = null;
        }

        if (hasPressedMouse)
        {
            hasPressedMouse = false;
        }
    }

    bool IsOnMesh(Vector3 _point, NavMeshAgent _agent)
    {
        NavMeshHit _navHit;
        if (NavMesh.SamplePosition(_point, out _navHit, 1f, NavMesh.AllAreas))
        {
            _agent.SetDestination(_navHit.position);
            return true;
        }
        return false;
    }
}
