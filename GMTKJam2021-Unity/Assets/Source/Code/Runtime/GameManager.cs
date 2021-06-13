using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;
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
    public float terrariumRadius = 40;
    [SerializeField]
    GameObject unitPrefab = null, unitUmbilicalCordPrefab = null;


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

    [Header("UI Debug Components")]
    [SerializeField]
    UIManager UI;

    bool hasPressedMouse = false;
    private GameObject lastSelectedObject = null, lastHighlightedObject = null;
    float oxygenTimer = 0;

    List<UnitBehavoir> units = new List<UnitBehavoir>();
    List<UmbilicalCordBehavior> umbilicalCordBehaviors = new List<UmbilicalCordBehavior>();

    public static GameManager GM = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (GM != null)
        {
            Destroy(GM);
        }
        else
        {
            GM = this;
        }

        hasPressedMouse = false;
        oxygenTimer = 0;
        SpawnUnits();
        UI.Init(units.ToArray());
    }

    void SpawnUnits()
    {
        for (int i = 0; i < numUnits; i++)
        {              
            UnitBehavoir _curUnit = Instantiate(unitPrefab, 
                pointWithInCirlce(unitSpawnpoint.position, spawnRadius), 
                Quaternion.identity).GetComponent<UnitBehavoir>();
            
            if (i < 3)
            {
                _curUnit.Init(terrarium, Classes.MINER);
            }
            else if (i >= 3 && i < 6)
            {
                _curUnit.Init(terrarium, Classes.SOLDIER);
            }
            else if(i >= 6 && i < 9)
            {
                _curUnit.Init(terrarium, Classes.RECON);
            }
            else
            {
                _curUnit.Init(terrarium, Classes.CAPTAIN);
            }
            
            units.Add(_curUnit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InteractWithObjects();
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
                //print("Called: " + _interactionHit.collider);
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
                                    UnitBehavoir _unit = lastSelectedObject.GetComponent<UnitBehavoir>();

                                    if (IsOnMesh(_interactionHit.point, _obj, _unit))
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
                                    if (lastSelectedObject.GetComponent<UnitBehavoir>())
                                    {
                                        print("called");
                                        if (IsOnMesh(GetClosetPoint(terrarium, lastSelectedObject.GetComponent<UnitBehavoir>().transform, terrariumRadius),  _obj, lastSelectedObject.GetComponent<UnitBehavoir>()))
                                        {
                                            lastSelectedObject.GetComponent<Outline>().enabled = false;
                                            lastSelectedObject = null;
                                        }
                                    }
                                    else
                                    {
                                        lastSelectedObject.GetComponent<Outline>().enabled = false;
                                        lastSelectedObject = _obj;
                                    }
                                    
                                }
                                else
                                {
                                    lastSelectedObject = _obj;
                                }
                                
                            }
                            //expand terrarium interaction
                            break;
                        case "resource":
                            if (lastSelectedObject != null && lastSelectedObject.GetComponent<UnitBehavoir>())
                            {
                                UnitBehavoir _curUnit = lastSelectedObject.GetComponent<UnitBehavoir>();
                                if (IsOnMesh(pointOnCircle(_interactionHit.point, _curUnit.gatherDst), _obj, _curUnit))
                                {
                                    //print("called");
                                    lastSelectedObject.GetComponent<Outline>().enabled = false;
                                    lastSelectedObject = null;
                                }
                            }
                            break;
                        case "enemy":
                            if (lastSelectedObject != null && lastSelectedObject.GetComponent<UnitBehavoir>())
                            {
                                UnitBehavoir _curUnit = lastSelectedObject.GetComponent<UnitBehavoir>();
                                if (IsOnMesh(pointOnCircle(_interactionHit.point, _curUnit.gatherDst), _obj, _curUnit))
                                {
                                    lastSelectedObject.GetComponent<Outline>().enabled = false;
                                    lastSelectedObject = null;
                                }
                            }
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

    public void DockUnit(UnitBehavoir _unit)
    {
        _unit.myAgent.enabled = false;
        _unit.transform.position = pointWithInCirlce(unitSpawnpoint.position, spawnRadius);
        _unit.transform.parent = unitSpawnpoint;
    }

    public void ReturnAllUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            NavMeshHit _navHit;
            if (NavMesh.SamplePosition(GetClosetPoint(terrarium, units[i].transform, terrariumRadius), out _navHit, 1f, NavMesh.AllAreas))
            {
                units[i].moveUnit(_navHit.position, terrarium.gameObject);
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

    bool IsOnMesh(Vector3 _point, GameObject _destination, UnitBehavoir _agent)
    {
        NavMeshHit _navHit;
        if (NavMesh.SamplePosition(_point, out _navHit, 5f, NavMesh.AllAreas))
        {
            if (_agent.myState == UnitStates.INSIDE)
            {
                _agent.transform.position = GetClosetPoint(terrarium, _navHit.position, terrariumRadius);
                GiveUmbilicalCord(_agent);
            }

            _agent.moveUnit(_navHit.position, _destination);
            return true;
        }
        print("Object not on Nav Mesh");
        return false;
    }

    Vector3 pointWithInCirlce(Vector3 _point, float _radius)
    {
        Vector2 _spawnCircle = (Random.insideUnitCircle * _radius);
        return new Vector3(_spawnCircle.x, 0, _spawnCircle.y) + _point;
    }

    Vector3 pointOnCircle(Vector3 _point, float _radius)
    {
        Vector3 _spawnCircle = Vector3.right;
        _spawnCircle = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * _spawnCircle;
        return new Ray(_point, _spawnCircle).GetPoint(_radius);
    }

    public Vector3 GetClosetPoint(Transform terrarium, Transform unitTransfrom, float _radius)
    {
        print("called Get point");
        Debug.DrawRay(terrarium.position, (unitTransfrom.position - terrarium.position).normalized * terrariumRadius, Color.blue, 10);
        return new Ray(terrarium.position, unitTransfrom.position - terrarium.position).GetPoint(_radius);
    }

    public Vector3 GetClosetPoint(Transform terrarium, Vector3 _point, float _radius)
    {
        print("called Get point");
        Debug.DrawRay(terrarium.position, (_point - terrarium.position).normalized * terrariumRadius, Color.black, 10);
        return new Ray(terrarium.position, _point - terrarium.position).GetPoint(_radius);
    }

    public void GiveUmbilicalCord(UnitBehavoir _unit)
    {
        print("called");
        UmbilicalCordBehavior _umb = Instantiate(unitUmbilicalCordPrefab, Vector3.zero, Quaternion.identity).GetComponent<UmbilicalCordBehavior>();
        _umb.Init(_unit, terrarium);
        umbilicalCordBehaviors.Add(_umb);
    }

    public void RemoveUmbilicalCord(UnitBehavoir _unit)
    {
        if (umbilicalCordBehaviors.Count > 0)
        {

        }
        for (int i = 0; i < umbilicalCordBehaviors.Count; i++)
        {
            if (umbilicalCordBehaviors[i].myUnit == _unit)
            {
                Destroy(umbilicalCordBehaviors[i].gameObject, 1f);
                umbilicalCordBehaviors.RemoveAt(i);
                break;
            }
        }
        umbilicalCordBehaviors.TrimExcess();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public GameObject FocusedObject
    {
        get { return terrarium.gameObject; }
    }

    private void OnDrawGizmos()
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
        if (terrarium != null)
        {
            Gizmos.color = Color.blue;
            Vector3 _sweepDir = Vector3.right;
            Vector3 _startPos = terrarium.position + (_sweepDir * terrariumRadius);
            for (int i = 0; i < 8; i++)
            {
                _sweepDir = Quaternion.AngleAxis(45, Vector3.up) * _sweepDir;
                Vector3 _nextPos = terrarium.position + (_sweepDir * terrariumRadius);
                Gizmos.DrawLine(_startPos, _nextPos);
                _startPos = _nextPos;
            }
        }
    }
}
