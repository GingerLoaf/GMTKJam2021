using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UmbilicalCordBehavior : MonoBehaviour
{
    LineRenderer LR = null;

    Transform tarrariumTransform = null;
    UnitBehavoir unit = null;

    public void Init(UnitBehavoir _unit, Transform _terrarium)
    {
        unit = _unit;
        tarrariumTransform = _terrarium;
        LR = GetComponent<LineRenderer>();
        LR.SetPosition(0, tarrariumTransform.position);
        LR.SetPosition(1, unit.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        bool _isWithinRange = Vector3.Distance(tarrariumTransform.position, unit.transform.position) <= unit.umbiCordLength;
        
        unit.isConnectedToBase = _isWithinRange;
        LR.enabled = _isWithinRange;

        if(_isWithinRange)
        {
            LR.SetPosition(0, tarrariumTransform.position);
            LR.SetPosition(1, unit.transform.position);
        }
        
    }

    public UnitBehavoir myUnit
    {
        get { return unit; }
    }
}


