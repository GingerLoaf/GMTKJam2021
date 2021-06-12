using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField]
    Button repairButton;
    [SerializeField]
    Button shipUpgradeButton;
    [SerializeField]
    GameObject unitCard = null;

    [Header("Properties")]
    [SerializeField]
    int[] shipComponentCosts = null;

    int curUpgradeIndex = 0;

    [Header("Global Properties")]
    [SerializeField]
    IntReference totalMetal = null;
    [SerializeField]
    IntReference terrariumCurrentHealth = null;
    [SerializeField]
    IntReference terrariumMaxHealth = null, repairCost = null, healthPerRpair;

    public void Init(UnitBehavoir[] _units) 
    {
        for (int i = 0; i < _units.Length; i++)
        {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curUpgradeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        repairButton.interactable = (totalMetal.Value > repairCost && terrariumCurrentHealth < terrariumMaxHealth);
        shipUpgradeButton.interactable = totalMetal.Value > shipComponentCosts[curUpgradeIndex];
    }

    public void UpgradeShip()
    {
        if (curUpgradeIndex < shipComponentCosts.Length)
        {
            totalMetal.Value -= shipComponentCosts[curUpgradeIndex];
            curUpgradeIndex++;
        }
        
    }

    public void RepairTerrarium()
    {
        if (totalMetal.Value > repairCost && terrariumCurrentHealth < terrariumMaxHealth)
        {
            totalMetal.Value -= repairCost;
            terrariumCurrentHealth.Value += healthPerRpair;
        }
    }

    public void OpenCrewMenu()
    {

    }
}
