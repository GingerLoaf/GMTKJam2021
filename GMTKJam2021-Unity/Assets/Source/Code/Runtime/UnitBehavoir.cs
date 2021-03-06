using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityAtoms.BaseAtoms;

[RequireComponent(typeof(NavMeshAgent), typeof(FogCutout))]

public class UnitBehavoir : MonoBehaviour
{
    //public GameObject unit;
    public int maxHealth;
    public int health;
    public int attack;
    public bool isSufficating;
    public FogCutout fogClear;
    public UnitStates myState;
    public NavMeshAgent myAgent;
    public Classes myClass = Classes.NONE;
    public Transform baseTranform;
    public float umbiCordLength;

    public Color myColor = Color.white;
    public string myName = "";
    public bool isConnectedToBase;

    public bool hasBeenUpgraded;

    [Header("Combat Properties")]
    public Combatant combatant = null;
    
    [Header("Gathering Properties")]
    public OreTypes curMinningType = OreTypes.METAL;
    public int maxCarryCapcity = 10;
    public float gatherRate = 1;
    public float gatherDst = 1;
    public int curMinedOxygen = 0;
    public int curMinedMetal = 0;
    float timeBetweenGathering = 0;

    [Header("Breathing Properties")]
    public float breathTimer;
    public float breathRate;
    public int oxygenLevel;
    public IntReference maxOxygen; 
    
    [Header("Global Properties")]
    public IntReference totalOxygen;
    public IntReference totalMetal;

    [Header("Debug")]
    public bool dumbUnit;
    public GameObject destinationObject;
    public float maxTimer;
    public float currentTimer;
    public TextAsset randomNameText = null;
    public Renderer suitMat = null;
    

    // Start is called before the first frame update
    public void Init(Transform _base, Classes _class)
    {
        fogClear = GetComponent<FogCutout>();

        myState = UnitStates.INSIDE;
        myAgent = GetComponent<NavMeshAgent>();
        oxygenLevel = maxOxygen;
        
        
        isConnectedToBase = true;
        baseTranform = _base;
        myClass = _class;

        maxHealth = 40;
        attack = 1;
        fogClear.m_cutoutScale = 10;
        myAgent.speed = 2;
        maxCarryCapcity = 5;
        breathRate = 10;
        umbiCordLength = 100f;
        health = maxHealth;
        isSufficating = false;

        gatherDst = 1;
        breathTimer = Random.Range(0, breathRate * 0.75f);

        myColor = Random.ColorHSV();

        string[] _names = randomNameText.text.Split(',');
        myName = _names[Random.Range(0, _names.Length)];

        suitMat.material.color = myColor;

        GameManager.GM.DockUnit(this);
        switch (myClass)
        {
            case Classes.MINER:
                gatherRate = 2;
                maxCarryCapcity += 5;
                break;
            case Classes.CAPTAIN:
                attack += 1;
                fogClear.m_cutoutScale += 3;
                myAgent.speed -= 1;
                break;
            case Classes.RECON:
                myAgent.speed += 3;
                //breathRate += 2;
                fogClear.m_cutoutScale += 2;
                break;
            case Classes.SOLDIER:
                maxHealth += 5;
                health = maxHealth;
                attack += 2;
                myAgent.speed += 1;
                break;
            case Classes.NONE:
                break;
            default:
                break;
        }
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
                    var enemyAttacked = destinationObject.GetComponent<Enemy>();

                    enemyAttacked.DoDamage(attack);
                    if (enemyAttacked.IsAlive == false)
                    {
                        myState = UnitStates.IDLE;
                    }
                    myAgent.isStopped = true;
                    // needs enemy damage script
                    break;
                case UnitStates.IDLE:
                    break;
                case UnitStates.INSIDE:
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
                                        maxCarryCapcity);
                                    break;
                                case OreTypes.METAL:
                                    _isGathering = isGatheringResource(ref curMinedMetal, ref _curDeposit.depositAmount, 
                                        maxCarryCapcity);
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
                                    moveUnit(GameManager.GM.GetClosetPoint(baseTranform, transform, GameManager.GM.terrariumRadius), baseTranform.gameObject);
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
                    GameManager.GM.RemoveUmbilicalCord(this);
                    GameManager.GM.DockUnit(this);
                    myState = UnitStates.INSIDE;
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
            if (breathRate <= breathTimer)
            {
                if (isConnectedToBase == true && totalOxygen.Value > 0)
                {
                    if (oxygenLevel < maxOxygen.Value)
                    {
                        while (oxygenLevel < maxOxygen && totalOxygen.Value > 0)
                        {

                            totalOxygen.Value--;
                            oxygenLevel++;
                            isSufficating = false;
                        }

                    }
                    else
                    {
                        totalOxygen.Value--;
                        isSufficating = false;
                    }

                }
                else
                {
                    if (oxygenLevel <= 0)
                    {

                        isSufficating = true;
                    }
                    else
                    {
                        oxygenLevel--;
                    }
                }
                if (isSufficating == true)
                {
                    takeDamage(1);
                }
                else
                {
                    Debug.Log("piss off im dead");
                }
                breathTimer = 0;
                combatant.IsAlive = myState != UnitStates.DIED;
            }
            else
            {
                breathTimer += Time.deltaTime;
            }

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
        if (_statToDecrease <= 0)
        {
            destinationObject.gameObject.SetActive(false);
        }
        return false;
    }

    public void DoUpgrade()
    {
        hasBeenUpgraded = true;
        switch (myClass)
        {
            case Classes.MINER:
                maxCarryCapcity += 5;
                gatherRate += 1;
                gatherDst += 1;
                myAgent.speed += 1;
                break;
            case Classes.CAPTAIN:
                break;
            case Classes.RECON:
                fogClear.m_cutoutScale += 2;
                myAgent.speed += 7;
                health += 5;
                breathRate += 10;
                health = maxHealth;
                break;
            case Classes.SOLDIER:
                attack += 3;
                myAgent.speed += 2;
                maxHealth += 15;
                breathRate += 3;
                health = maxHealth;
                break;
            case Classes.NONE:
                Debug.Log("What did you do" + " why did you do it");
                break;
        }
    }
    public void moveUnit(Vector3 destination, GameObject _objectTag)
    {
        myAgent.enabled = false;
        myAgent.enabled = true;
        myAgent.SetDestination(destination);

        myState = UnitStates.MOVING;
        destinationObject = _objectTag;
    
    
    }

    public void takeDamage(int _damge)
    {
        health -= _damge;
        if (health <= 0)
        {
            myState = UnitStates.DIED;
        }
    }
}

public enum UnitStates {MOVING, ATTACKING, IDLE, INSIDE, GATHERING, DIED, DEPOSIT}
public enum Classes {MINER,CAPTAIN,RECON,SOLDIER, NONE }


