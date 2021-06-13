using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class UnitCardWidget : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]
    Image icon = null;
    [SerializeField]
    Button upgradeButton = null;
    [SerializeField]
    Text nameText = null, statText = null, upgradeText = null;
    [SerializeField]
    Slider oxygenBar = null, healthBar = null;

    [Header("Global Vars")]
    [SerializeField]
    IntReference totalMetal = null;
    [SerializeField]
    IntReference upgradeCost = null;

    UnitBehavoir myUnit = null;

    public void Init(UnitBehavoir _unit)
    {
        myUnit = _unit;

        oxygenBar.minValue = 0;
        oxygenBar.maxValue = _unit.maxOxygen;

        healthBar.minValue = 0;
        healthBar.maxValue = _unit.maxHealth;

        switch (myUnit.myClass)
        {
            case Classes.MINER:
                if (_unit.hasBeenUpgraded)
                {
                    statText.text = "Stats: O2 Cost, Speed+, Attack, Vision, Gather++, Carry++";
                }
                else
                {
                    statText.text = "Stats: O2 Cost, Speed, Attack, Vision, Gather+, Carry+";
                }
                break;
            case Classes.CAPTAIN:
                if (_unit.hasBeenUpgraded)
                {
                    statText.text = "Stats: O2 Cost, Speed, Attack++, Vision++, Gather, Carry";
                }
                else
                {
                    statText.text = "Stats: O2 Cost, Speed, Attack+, Vision+, Gather, Carry";
                }
                break;
            case Classes.RECON:
                if (_unit.hasBeenUpgraded)
                {
                    statText.text = "Stats: O2 Cost-, Speed++, Attack, Vision++, Gather, Carry";
                }
                else
                {
                    statText.text = "Stats: O2 Cost, Speed+, Attack, Vision+, Gather, Carry";
                }
                break;
            case Classes.SOLDIER:
                if (_unit.hasBeenUpgraded)
                {
                    statText.text = "Stats: O2 Cost-, Speed++, Attack++, Vision, Gather, Carry";
                }
                else
                {
                    statText.text = "Stats: O2 Cost, Speed+, Attack+, Vision, Gather, Carry";
                }
                break;
            case Classes.NONE:
                statText.text = "Stats: Debug!";
                break;
            default:
                break;
        }

        nameText.text = "Name: " + myUnit.myName +"\nUpgraded?";
        //statText.text = "Stats Debug!";
        if (_unit.hasBeenUpgraded)
        {
            upgradeText.text = "yes";
            upgradeText.color = Color.green;
        }
        else
        {
            upgradeText.text = "no";
            upgradeText.color = Color.red;
        }

        icon.color = _unit.myColor;

        UnityAction upgrade = _unit.DoUpgrade;
        upgradeButton.onClick.AddListener(upgrade);
        upgradeButton.onClick.AddListener(()=> totalMetal.Value -= upgradeCost);
        upgradeButton.onClick.AddListener(()=> Init(_unit));

        
        //upgradeIcon.SetActive(_unit.hasBeenUpgraded);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = myUnit.health;
        oxygenBar.value = myUnit.oxygenLevel;
        upgradeButton.interactable = totalMetal.Value >= upgradeCost.Value && !myUnit.hasBeenUpgraded;
    }

}
