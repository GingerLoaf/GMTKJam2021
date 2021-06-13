using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    #region Private Methods

    private void Update()
    {
        DetectNearbyCombatants();
    }

    private void DetectNearbyCombatants()
    {
        var enemies = Enemy.Discovered;
        var combatants = Combatant.Discovered;
        for (var i = 0; i < enemies.Count; i++)
        {
            var currentEnemy = enemies[i];
            if (currentEnemy == null)
            {
                continue;
            }

            if (currentEnemy.ClosestCombatant != null)
            {
                var distanceFromCombatant = Vector3.Distance(currentEnemy.StartPosition, currentEnemy.ClosestCombatant.transform.position);
                if (!currentEnemy.ClosestCombatant.IsAlive || distanceFromCombatant > currentEnemy.TargetDistance)
                {
                    currentEnemy.ClosestCombatant = null;
                }
                else
                {
                    continue;
                }
            }

            Tuple<int, float> indexToDistancePair = null;
            for (var ii = 0; ii < combatants.Count; ii++)
            {
                var currentCombatant = combatants[ii];
                if (currentCombatant == null)
                {
                    continue;
                }

                if (!currentCombatant.IsAlive)
                {
                    continue;
                }

                // Zack: Use the starting position of the enemy to keep things anchored to their region of influence
                var distanceFromCombatant = Vector3.Distance(currentEnemy.StartPosition, currentCombatant.transform.position);
                if (distanceFromCombatant > currentEnemy.TargetDistance)
                {
                    continue;
                }

                if (indexToDistancePair == null || distanceFromCombatant < indexToDistancePair.Item2)
                {
                    indexToDistancePair = new Tuple<int, float>(ii, distanceFromCombatant);
                }
            }

            Combatant closestCombatant = null;
            if (indexToDistancePair != null)
            {
                if (indexToDistancePair.Item2 < m_maxCombatDistance.Value)
                {
                    closestCombatant = combatants[indexToDistancePair.Item1];
                }
            }

            currentEnemy.ClosestCombatant = closestCombatant;
        }
    }

    #endregion

    #region Private Fields

    [SerializeField]
    private FloatReference m_maxCombatDistance = new FloatReference(50f);

    #endregion

}