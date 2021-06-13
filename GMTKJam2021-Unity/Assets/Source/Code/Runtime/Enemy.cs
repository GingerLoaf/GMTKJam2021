using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Enemy : AbstractDiscoverableEntity<Enemy>
{

    #region Public Properties

    /// <summary>
    ///     Whether, or not, the enemy is alive
    /// </summary>
    public bool IsAlive => m_health > 0;

    /// <summary>
    ///     The rate (in seconds) between each attack
    /// </summary>
    public float DamageRate => m_damageRate;

    /// <summary>
    ///     The Damage dealt with each attack
    /// </summary>
    public int Damage => m_damage;

    /// <summary>
    ///     The distance (in meters) required for attacks to be possible.
    ///     The enemy must be <see cref="AttackDistance" /> close to the target to begin attacking.
    /// </summary>
    public float AttackDistance => m_attackDistance;

    /// <summary>
    ///     The distance (in meters) at which the enemy will scout out targets.
    ///     If a target is outside of this range, it will not be considered.
    /// </summary>
    public float TargetDistance => m_targetDistance;

    /// <summary>
    ///     The target that will be pursued by the enemy
    /// </summary>
    public Combatant ClosestCombatant { get; set; }

    /// <summary>
    ///     The position the enemy was at when the game started.
    ///     This will be used for leashing back when no targets are found.
    /// </summary>
    public Vector3 StartPosition { get; private set; }

    #endregion

    #region Public Methods

    public void DoDamage(int damage)
    {
        m_health.Value -= damage;
    }

    #endregion

    #region Protected Methods

    /// <inheritdoc />
    protected override void OnEnableProtected()
    {
        StartPosition = transform.position;
    }

    #endregion

    #region Private Methods

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_attackDistance);
        Gizmos.DrawWireSphere(StartPosition, m_targetDistance);
    }

    #endregion

    #region Private Fields

    [SerializeField]
    private IntReference m_health = new IntReference(10);

    [SerializeField]
    private IntReference m_damage = new IntReference(1);

    [SerializeField]
    private FloatReference m_attackDistance = new FloatReference(1f);

    [SerializeField]
    private FloatReference m_damageRate = new FloatReference(5f);

    [SerializeField]
    private FloatReference m_targetDistance = new FloatReference(20f);

    #endregion

}