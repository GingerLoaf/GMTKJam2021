using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Global Properties")]
    [SerializeField]
    private IntReference totalOxygen = null;
    [SerializeField]
    private IntReference totalMetal = null;
    [SerializeField]
    private IntReference totalMorale = null;

    [Header("Unit Spawn Properties")]
    [SerializeField]
    int numUnits = 10;
    [SerializeField]
    Transform unitSpawnpoint = null;
    [SerializeField]
    float spawnRadius = 3;
    [SerializeField]
    GameObject unitPrefab = null;

    [Header("Terrarium Properties")]
    [SerializeField]
    private float oxygenGenerationTime = 10f;
    [SerializeField]
    Transform terrarium;

    [Header("SpaceShip Properties")]
    [SerializeField]
    private int[] spaceshipComponentCosts;

    [Header("Unit Selection Properties")]
    [SerializeField]
    Camera myCamera;
    [SerializeField]
    Color highlightColor = Color.white;
    [SerializeField]
    Color SelectedColor = Color.black;

    bool hasPressedMouse = false;
    private GameObject lastSelectedObject = null, lastHighlightedObject = null;
    float oxygenTimer = 0;

    List<UnitBehavoir> units = new List<UnitBehavoir>();

    // Start is called before the first frame update
    void Awake()
    {
        hasPressedMouse = false;
        oxygenTimer = 0;
        SpawnUnits();
    }

    void SpawnUnits()
    {
        for (int i = 0; i < numUnits; i++)
        {              
            UnitBehavoir _curUnit = Instantiate(unitPrefab, 
                pointWithInCirlce(unitSpawnpoint.position, spawnRadius), 
                Quaternion.identity).GetComponent<UnitBehavoir>();
            
            units.Add(_curUnit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InteractWithObjects();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ReturnAllUnits();
        }
    }

    void InteractWithObjects()
    {
        Ray _mouseRay = myCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _interactionHit;
        Debug.DrawRay(_mouseRay.origin, _mouseRay.direction * 10);
        if (Physics.Raycast(_mouseRay, out _interactionHit))
        {
            if (lastHighlightedObject == null && _interactionHit.collider.GetComponent<Outline>())
            {
                print("Called: " + _interactionHit.collider);
                _interactionHit.collider.GetComponent<Outline>().enabled = true;
                _interactionHit.collider.GetComponent<Outline>().OutlineColor = highlightColor;
                lastHighlightedObject = _interactionHit.collider.gameObject;
            }

            else if (lastHighlightedObject != null && lastHighlightedObject != _interactionHit.collider.gameObject)
            {
                if (lastSelectedObject == null)
                {
                    lastHighlightedObject.GetComponent<Outline>().enabled = false;
                    lastHighlightedObject = null;
                }
                else
                {
                    lastSelectedObject.GetComponent<Outline>().OutlineColor = SelectedColor;

                    if (lastHighlightedObject != lastSelectedObject)
                    {
                        lastHighlightedObject.GetComponent<Outline>().enabled = false;
                        if (_interactionHit.collider.GetComponent<Outline>())
                        {
                            lastHighlightedObject = _interactionHit.collider.gameObject;
                            lastHighlightedObject.GetComponent<Outline>().enabled = true;
                            lastHighlightedObject.GetComponent<Outline>().OutlineColor = highlightColor;
                        }
                        else
                        {
                            lastHighlightedObject = null;
                        }
                    }
                    else
                    {
                        lastHighlightedObject = null;
                    }
                }
            }

            if (Mouse.current.leftButton.IsPressed())
            {
                if (!hasPressedMouse)
                {
                    GameObject _obj = _interactionHit.transform.gameObject;
                    switch (_interactionHit.collider.tag)
                    {
                        case "terrian":
                            if (lastSelectedObject != null)
                            {
                                if (lastSelectedObject.GetComponent<UnitBehavoir>())
                                {
                                    if (IsOnMesh(_interactionHit.point, lastSelectedObject.GetComponent<UnitBehavoir>()))
                                    {
                                        lastSelectedObject.GetComponent<Outline>().enabled = false;
                                        lastSelectedObject = null;
                                    }
                                }
                                else if (lastSelectedObject.GetComponent<NavMeshAgent>())
                                {
                                    if (IsOnMesh(_interactionHit.point, lastSelectedObject.GetComponent<NavMeshAgent>()))
                                    {
                                        lastSelectedObject.GetComponent<Outline>().enabled = false;
                                        lastSelectedObject = null;
                                    }
                                }
                            }
                            break;

                        case "unit":
                            if (_obj.GetComponent<UnitBehavoir>().myState != UnitStates.DIED)
                            {
                                if (lastSelectedObject == null || lastSelectedObject != _obj)
                                {
                                    if (lastSelectedObject != null && lastSelectedObject != _obj)
                                    {
                                        lastSelectedObject.GetComponent<Outline>().enabled = false;
                                    }
                                    lastSelectedObject = _obj;
                                }
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
        else if (lastHighlightedObject != null)
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

    void GenerateOxygen()
    {
        if (oxygenTimer >= oxygenGenerationTime)
        {
            totalOxygen.Value++;
        }
    }

    public void ReturnAllUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            NavMeshHit _navHit;
            if (NavMesh.SamplePosition(pointWithInCirlce(unitSpawnpoint.position, spawnRadius), out _navHit, 1f, NavMesh.AllAreas))
            {
                units[i].moveUnit(_navHit.position,terrarium.gameObject);
            }
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

    bool IsOnMesh(Vector3 _point, UnitBehavoir _agent)
    {
        NavMeshHit _navHit;
        if (NavMesh.SamplePosition(_point, out _navHit, 1f, NavMesh.AllAreas))
        {
            _agent.moveUnit(_navHit.position,terrarium.gameObject);
            return true;
        }
        return false;
    }

    Vector3 pointWithInCirlce(Vector3 _point, float _radius)
    {
        Vector2 _spawnCircle = (Random.insideUnitCircle * _radius);
        return new Vector3(_spawnCircle.x, 0, _spawnCircle.y) + _point;
    }

    private void OnDrawGizmosSelected()
    {
        if (unitSpawnpoint != null)
        {
            Vector3 _sweepDir = Vector3.right;
            Vector3 _startPos = unitSpawnpoint.position + (_sweepDir * spawnRadius);
            for (int i = 0; i < 8; i++)
            {
                _sweepDir = Quaternion.AngleAxis(45, Vector3.up) * _sweepDir;
                Vector3 _nextPos = unitSpawnpoint.position + (_sweepDir * spawnRadius);
                Gizmos.DrawLine(_startPos, _nextPos);
                _startPos = _nextPos;
            }
        }
    }
}
