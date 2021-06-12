using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityAtoms.BaseAtoms;

[RequireComponent(typeof(NavMeshAgent))]

public class UnitBehavoir : MonoBehaviour
{
    //public GameObject unit;
    public int Health;
    public float fogClear;
    public UnitStates myState;
    public NavMeshAgent myAgent;
    public int currentResource = 0;
    public int maxResource = 10;
    public float maxTimer;
    public float currentTimer;
    public Classes myClass = Classes.NONE;
    
    public Transform baseTranform;
    bool isConnectedToBase;

    [Header("Gathering Properties")]
    public OreTypes curMinningType = OreTypes.METAL;
    public IntReference maxCarryCapcity = null;
    public float gatherRate = 1;
    public float gatherDst = 1;
    public int curMinedOxygen = 0;
    public int curMinedMetal = 0;
    float timeBetweenGathering = 0;

    [Header("Breathing Properties")]
    public float breathtimer;
    public float breathRate;
    public int oxygenLevel;
    public IntReference maxOxygen; 
    
    [Header("Global Properties")]
    public IntReference totalOxygen;
    public IntReference totalMetal;

    [Header("Debug")]
    public bool dumbUnit;
    public GameObject destinationObject;

    // Start is called before the first frame update
    public void Init(Transform _base, Classes _class)
    {
        myState = UnitStates.IDLE;
        myAgent = GetComponent<NavMeshAgent>();
        oxygenLevel = 20;
        //Debug
        isConnectedToBase = true;
        baseTranform = _base;
        myClass = _class;
        //unit = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (myState != UnitStates.DIED)
        {
            switch (myState)
            {
                case UnitStates.MOVING:
                    if (Vector3.Distance(transform.position, myAgent.destination) < 1)
                    {
                        switch (destinationObject.tag)
                        {
                            case "resource":
                                myState = UnitStates.GATHERING;
                                break;
                            case "enemy":
                                myState = UnitStates.ATTACKING;
                                break;
                            case "terrain":
                                myState = UnitStates.MOVING;
                                break;
                            case "terrarium":
                                myState = UnitStates.DEPOSIT;
                                break;
                            default:
                                myState = UnitStates.IDLE;
                                break;
                        }
                        //myState = UnitStates.IDLE;
                    }
                    break;
                case UnitStates.ATTACKING:
                    myAgent.isStopped = true;
                    // needs enemy damage script
                    break;
                case UnitStates.IDLE:
                    break;
                case UnitStates.GATHERING:
                    if (timeBetweenGathering >= gatherRate)
                    {
                        if (destinationObject.GetComponent<OreDeposit>())
                        {
                            OreDeposit _curDeposit = destinationObject.GetComponent<OreDeposit>();
                            bool _isGathering = false;
                            switch (_curDeposit.Type)
                            {
                                case OreTypes.OXYGEN:
                                    _isGathering = isGatheringResource(ref curMinedOxygen, ref _curDeposit.depositAmount, 
                                        maxCarryCapcity.Value);
                                    break;
                                case OreTypes.METAL:
                                    _isGathering = isGatheringResource(ref curMinedMetal, ref _curDeposit.depositAmount, 
                                        maxCarryCapcity.Value);
                                    break;
                                default:
                                    break;
                            }
                            if (!_isGathering)
                            {
                                if (dumbUnit == true)
                                {
                                    myState = UnitStates.IDLE;
                                }
                                else
                                {
                                    moveUnit(baseTranform.position, baseTranform.gameObject);
                                }
                                //print(curMinedMetal);
                                //print(cur);
                            }
                            timeBetweenGathering = 0;
                        }
                    }
                    else
                    {
                        timeBetweenGathering += Time.deltaTime;
                    }

                    break;
                case UnitStates.DEPOSIT:
                    if (curMinedOxygen > 0)
                    {
                        totalOxygen.Value += curMinedOxygen;
                        curMinedOxygen = 0;
                    }

                    if (curMinedMetal > 0)
                    {
                        totalMetal.Value += curMinedMetal;
                        curMinedMetal = 0;
                    }
                    myState = UnitStates.IDLE;
                    break;
                case UnitStates.SUFFICATION:
                    
                    if (currentTimer >= maxTimer)
                    {
                        takeDamage(1);
                        currentTimer = 0;
                        if (myState == UnitStates.DIED)
                        {
                            return;
                        }
                    }
                    else
                    {
                        currentTimer += Time.deltaTime;
                    }

                    break;
                case UnitStates.DIED:
                    myAgent.enabled = false;
                    break;
                default:
                    break;
            }
            /* UmbilicalCord break flag
             * if(currentTimer >= maxTimer)
             * {
             * oxygenLevel - oxygenLose;
             * }
             */
            if (breathRate <= breathtimer)
            {
                if (isConnectedToBase == true && totalOxygen.Value > 0)
                {
                    if (oxygenLevel < maxOxygen.Value)
                    {
                        while (oxygenLevel < maxOxygen && totalOxygen.Value > 0)
                        {

                            totalOxygen.Value--;
                            oxygenLevel++;
                        }

                    }
                    else
                    {
                        totalOxygen.Value--;
                    }

                }
                else
                {
                    if (oxygenLevel <= 0)
                    {

                        myState = UnitStates.SUFFICATION;
                    }
                    else
                    {
                        oxygenLevel--;
                    }
                }
                breathtimer = 0;
            }
            else
            {
                breathtimer += Time.deltaTime;
            }

        }
        else
        {
            Debug.Log("piss off im dead");
        }
    }

    public bool isGatheringResource(ref int _statToIncrease, ref int _statToDecrease, int _limit)
    {
        if (_statToIncrease < _limit && _statToDecrease > 0)
        {
            print("called");
            _statToIncrease++;
            _statToDecrease--;
            return true;
        }
        return false;
    }

    public void moveUnit(Vector3 destination, GameObject _objectTag)
    {
        myAgent.SetDestination(destination);
        
        myState = UnitStates.MOVING;
        destinationObject = _objectTag;
    
    
    }
    public void takeDamage(int _damge)
    {
        Health -= _damge;
        if (Health <= 0)
        {
            myState = UnitStates.DIED;
        }
    }

    
}

public enum UnitStates {MOVING, ATTACKING, IDLE, GATHERING, SUFFICATION, DIED, DEPOSIT}
public enum Classes {MINER,CAPTAIN,RECON,SOLDIER, NONE }


