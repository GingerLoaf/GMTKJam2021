using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : AbstractDiscoverableEntity<Enemy>
{

    #region Public Properties

    public Combatant ClosestCombatant { get; set; }

    #endregion

    #region Private Fields

    [SerializeField]
    private ScriptGraphAsset m_idleState = null;

    [SerializeField]
    private ScriptGraphAsset m_combatState = null;

    [SerializeField]
    private StateMachine m_stateMachine = null;

    #endregion

    private void Start()
    {
        var test = m_stateMachine.graph.states[0].graph.title;
        //m_stateMachine.graph.states.FirstOrDefault(s => s.)
    }

}