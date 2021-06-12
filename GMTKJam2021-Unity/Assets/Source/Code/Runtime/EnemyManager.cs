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

            Tuple<int, float> indexToDistancePair = null;
            for (var ii = 0; ii < combatants.Count; ii++)
            {
                var currentCombatant = combatants[ii];
                if (currentCombatant == null)
                {
                    continue;
                }

                var distanceFromCombatant = Vector3.Distance(currentEnemy.transform.position, currentCombatant.transform.position);
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