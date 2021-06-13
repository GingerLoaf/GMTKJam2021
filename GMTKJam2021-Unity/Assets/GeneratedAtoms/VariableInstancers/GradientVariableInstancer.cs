using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Variable Instancer of type `Gradient`. Inherits from `AtomVariableInstancer&lt;GradientVariable, GradientPair, Gradient, GradientEvent, GradientPairEvent, GradientGradientFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/Gradient Variable Instancer")]
    public class GradientVariableInstancer : AtomVariableInstancer<
        GradientVariable,
        GradientPair,
        Gradient,
        GradientEvent,
        GradientPairEvent,
        GradientGradientFunction>
    { }
}
