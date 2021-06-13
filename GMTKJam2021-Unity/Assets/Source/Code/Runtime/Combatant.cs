using System;
using UnityEngine;
using UnityEngine.Events;

public class Combatant : AbstractDiscoverableEntity<Combatant>
{

    #region Public Methods

    public void DoDamage(int damage)
    {
        m_onDamage?.Invoke(damage);
    }

    #endregion

    #region Private Fields

    [SerializeField]
    private IntUnityEvent m_onDamage = null;

    #endregion

}

[Serializable]
public class IntUnityEvent : UnityEvent<int> { }