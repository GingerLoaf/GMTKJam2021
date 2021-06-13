using UnityEngine;

public class Enemy : AbstractDiscoverableEntity<Enemy>
{

    #region Public Properties

    public float DamageRate => m_damageRate;

    public int Damage => m_damage;

    public float AttackDistance => m_attackDistance;

    public Combatant ClosestCombatant { get; set; }

    public Vector3 StartPosition { get; private set; }

    public Quaternion StartRotation { get; private set; }

    #endregion

    #region Protected Methods

    /// <inheritdoc />
    protected override void OnEnableProtected()
    {
        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }

    #endregion

    #region Private Fields

    [SerializeField]
    private int m_damage = 1;

    [SerializeField]
    private float m_attackDistance = 1f;

    [SerializeField]
    private float m_damageRate = 5f;

    #endregion

}