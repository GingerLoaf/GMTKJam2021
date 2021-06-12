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
    public int oxygenLevel;
    public float gartherSpeed;
    public float fogClear;
    public UnitStates myState;
    public NavMeshAgent myAgent;
    public int currentResource = 0;
    public int maxResource = 10;
    public float maxTimer;
    public float currentTimer;
    public int gatherRate;
    public Transform baseTranform;
    public int oxygenLose;
    bool isConnectedToBase;
    public bool dumbUnit;
    public GameObject destinationObject;
    public float breathtimer;
    public float breathRate;
    public IntReference totalOxygen;
    public IntReference maxOxygen; 
    

    // Start is called before the first frame update
    void Start()
    {
        myState = UnitStates.IDLE;
        myAgent = GetComponent<NavMeshAgent>();
        oxygenLevel = 20;
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
                    if (myAgent.remainingDistance < 1)
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
                                myState = UnitStates.IDLE;
                                break;
                            case "terrarium":
                                myState = UnitStates.IDLE;
                                break;
                            default:
                                break;
                        }
                        myState = UnitStates.IDLE;
                    }
                    break;
                case UnitStates.ATTACKING:
                    myAgent.isStopped = true;
                    // needs enemy damage script
                    break;
                case UnitStates.IDLE:
                    break;
                case UnitStates.GATHERING:
                    if (currentTimer >= maxTimer)
                    {
                        if (currentResource < maxResource)
                        {
                            currentResource += 1;
                            // destination to remove resources
                        }
                        else
                        {
                            if (dumbUnit == true)
                            {
                                myState = UnitStates.IDLE;
                            }
                            else
                            {
                                moveUnit(baseTranform.position, baseTranform.gameObject);
                            }

                        }

                    }
                    else
                    {
                        currentTimer += Time.deltaTime * gartherSpeed;
                    }

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

public enum UnitStates {MOVING,ATTACKING,IDLE,GATHERING,SUFFICATION,DIED}
public enum Classes {MINER,CAPTAIN,RECON,SOLDIER }


