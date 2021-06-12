using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Reference of type `Gradient`. Inherits from `AtomReference&lt;Gradient, GradientPair, GradientConstant, GradientVariable, GradientEvent, GradientPairEvent, GradientGradientFunction, GradientVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GradientReference : AtomReference<
        Gradient,
        GradientPair,
        GradientConstant,
        GradientVariable,
        GradientEvent,
        GradientPairEvent,
        GradientGradientFunction,
        GradientVariableInstancer>, IEquatable<GradientReference>
    {
        public GradientReference() : base() { }
        public GradientReference(Gradient value) : base(value) { }
        public bool Equals(GradientReference other) { return base.Equals(other); }
        protected override bool ValueEquals(Gradient other)
        {
            throw new NotImplementedException();
        }
    }
}
