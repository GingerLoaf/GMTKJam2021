using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Set variable value Action of type `Gradient`. Inherits from `SetVariableValue&lt;Gradient, GradientPair, GradientVariable, GradientConstant, GradientReference, GradientEvent, GradientPairEvent, GradientVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/Gradient", fileName = "SetGradientVariableValue")]
    public sealed class SetGradientVariableValue : SetVariableValue<
        Gradient,
        GradientPair,
        GradientVariable,
        GradientConstant,
        GradientReference,
        GradientEvent,
        GradientPairEvent,
        GradientGradientFunction,
        GradientVariableInstancer>
    { }
}
