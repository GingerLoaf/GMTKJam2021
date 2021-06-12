using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDeposit : MonoBehaviour
{
    [SerializeField]
    OreTypes oreState = OreTypes.METAL;
    [SerializeField]
    public int depositAmount = 20;

    public OreTypes Type
    {
        get { return oreState; }
    }
    /*
    public int Amount
    {
        get { return depositAmount; }
        set { depositAmount = value; }
    } 
    */
}

public enum OreTypes
{
    OXYGEN, METAL
}
