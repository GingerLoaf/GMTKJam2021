using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField]
    Text statReadout = null;
    //[SerializeField]
    //Button repairButton = null;
    [SerializeField]
    Button shipUpgradeButton = null;
    [SerializeField]
    GameObject unitCardPrefab = null;
    [SerializeField]
    Transform unitCardSlot = null;
    [SerializeField]
    GameObject winScreen = null;

    [Header("Properties")]
    [SerializeField]
    int[] shipComponentCosts = null;

    int curUpgradeIndex = 0;

    [Header("Global Properties")]
    [SerializeField]
    IntReference totalMetal = null, totalOxygen = null;
    [SerializeField]
    IntReference terrariumCurrentHealth = null;
    [SerializeField]
    IntReference terrariumMaxHealth = null, repairCost = null, healthPerRpair;

    UnitBehavoir[] units = null;

    GameObject[] spawnedUnitCards = null;

    public void Init(UnitBehavoir[] _units) 
    {
        units = _units;
        spawnedUnitCards = new GameObject[_units.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        curUpgradeIndex = 0;
        
        //repairButton.transform.GetChild(0).GetComponent<Text>().text = "repair (" + repairCost.Value + ")";

        shipUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = 
            "buy ship component (" + shipComponentCosts[curUpgradeIndex] + ")";
    }

    // Update is called once per frame
    void Update()
    {
        //repairButton.interactable = (totalMetal.Value >= repairCost && terrariumCurrentHealth < terrariumMaxHealth);
        
        if (curUpgradeIndex < shipComponentCosts.Length)
        {
            shipUpgradeButton.interactable = totalMetal.Value >= shipComponentCosts[curUpgradeIndex];
        }

        statReadout.text = "Terriarum Health " + terrariumCurrentHealth.Value + " / " + terrariumMaxHealth.Value + 
            "\nMetal Storage: " + totalMetal.Value + "\nOxygen Storage: " + totalOxygen.Value;
    }

    public void UpgradeShip()
    {
        if (curUpgradeIndex < shipComponentCosts.Length)
        {
            totalMetal.Value -= shipComponentCosts[curUpgradeIndex];
            curUpgradeIndex++;

            if (curUpgradeIndex < shipComponentCosts.Length)
            {
                shipUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = 
                    "buy ship component (" + shipComponentCosts[curUpgradeIndex] + ")";
            }
            else
            {
                winScreen.SetActive(true);

            }
        }
        
    }

    /*
    public void RepairTerrarium()
    {
        if (totalMetal.Value >= repairCost && terrariumCurrentHealth < terrariumMaxHealth)
        {
            totalMetal.Value -= repairCost;
            terrariumCurrentHealth.Value += healthPerRpair;
        }
    }
    */
    public void OpenCrewMenu()
    {
        for (int i = 0; i < units.Length; i++)
        {
            GameObject _curCard = Instantiate(unitCardPrefab, unitCardSlot);
            _curCard.GetComponent<UnitCardWidget>().Init(units[i]);
            spawnedUnitCards[i] = _curCard;
        }
    }

    public void CloseCrewMenu()
    {
        for (int i = 0; i < spawnedUnitCards.Length; i++)
        {
            Destroy(spawnedUnitCards[i]);
        }
    }
}
