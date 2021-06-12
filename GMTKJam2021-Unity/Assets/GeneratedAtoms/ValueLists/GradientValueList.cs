using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Value List of type `Gradient`. Inherits from `AtomValueList&lt;Gradient, GradientEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/Gradient", fileName = "GradientValueList")]
    public sealed class GradientValueList : AtomValueList<Gradient, GradientEvent> { }
}
