using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class UnitBehavoir : MonoBehaviour
{
    public GameObject unit;
    public bool isConnected;
    public int Health;
    public int oxygenLevel;
    public float gartherSpeed;
    public float fogClear;
    public UnitStates myState;
    public NavMeshAgent myAgent;
    public int currentRescource = 0;
    public int maxRescource = 10;
    public float maxTimer;
    public float currentTimer;
    public int gatherRate;
    public Transform baseTranform;
    public int oxygenLose;
    bool isConnectedToBase;
    public bool dumbUnit;
    

    // Start is called before the first frame update
    void Start()
    {
        myState = UnitStates.IDLE;
        myAgent = GetComponent<NavMeshAgent>();
        oxygenLevel = 20;


    }

    // Update is called once per frame
    void Update()
    {

        switch (myState)
        {
            case UnitStates.MOVING:
                if (myAgent.remainingDistance < 1)
                {
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
                    if (currentRescource < maxRescource)
                    {
                        currentRescource += 1;
                    }
                    else
                    {
                        if (dumbUnit == true)
                        {
                            myState = UnitStates.IDLE;
                        }
                        else
                        {
                            moveUnit(baseTranform.position);
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

        if (isConnectedToBase == true)
        {
            if (oxygenLevel <= 0)
            {
                //take oxygen form base
                //addedOxygen();
            }
        }
        else
        {
            if (oxygenLevel <= 0)
            {
                myState = UnitStates.SUFFICATION;
            }
        }
        


    }

    public void moveUnit(Vector3 destination)
    {
        myAgent.SetDestination(destination);
        myState = UnitStates.MOVING;
    
    
    }
    public void takeDamage(int _damge)
    {
        Health -= _damge;
    }

    
}

public enum UnitStates {MOVING,ATTACKING,IDLE,GATHERING,SUFFICATION,DIED}
public enum Classes {MINER,CAPTAIN,RECON,SOLDIER }
