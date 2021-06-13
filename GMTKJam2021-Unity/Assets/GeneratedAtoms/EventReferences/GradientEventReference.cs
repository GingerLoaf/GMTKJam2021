using System;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `Gradient`. Inherits from `AtomEventReference&lt;Gradient, GradientVariable, GradientEvent, GradientVariableInstancer, GradientEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GradientEventReference : AtomEventReference<
        Gradient,
        GradientVariable,
        GradientEvent,
        GradientVariableInstancer,
        GradientEventInstancer>, IGetEvent 
    { }
}
