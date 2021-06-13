using System;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `GradientPair`. Inherits from `AtomEventReference&lt;GradientPair, GradientVariable, GradientPairEvent, GradientVariableInstancer, GradientPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GradientPairEventReference : AtomEventReference<
        GradientPair,
        GradientVariable,
        GradientPairEvent,
        GradientVariableInstancer,
        GradientPairEventInstancer>, IGetEvent 
    { }
}
