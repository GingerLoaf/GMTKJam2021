using System;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Variable of type `Gradient`. Inherits from `AtomVariable&lt;Gradient, GradientPair, GradientEvent, GradientPairEvent, GradientGradientFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Gradient", fileName = "GradientVariable")]
    public sealed class GradientVariable : AtomVariable<Gradient, GradientPair, GradientEvent, GradientPairEvent, GradientGradientFunction>
    {
        protected override bool ValueEquals(Gradient other)
        {
            throw new NotImplementedException();
        }
    }
}
