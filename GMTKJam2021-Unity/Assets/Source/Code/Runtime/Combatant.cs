using System;
using UnityEngine;
using UnityEngine.Events;

public class Combatant : AbstractDiscoverableEntity<Combatant>
{

    #region Public Properties

    /// <summary>
    ///     Whether the combatant is alive or not.
    ///     Set this to false when the combatant should no longer be attacked by enemies.
    /// </summary>
    public bool IsAlive
    {
        get => gameObject.activeInHierarchy && enabled && m_isAlive;
        set => m_isAlive = value;
    }

    #endregion

    #region Public Methods

    public void DoDamage(int damage)
    {
        m_onDamage?.Invoke(damage);
    }

    #endregion

    #region Private Fields

    [SerializeField]
    private bool m_isAlive = true;

    [SerializeField]
    private IntUnityEvent m_onDamage = null;

    #endregion

}

[Serializable]
public class IntUnityEvent : UnityEvent<int> { }