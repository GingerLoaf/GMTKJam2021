using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `GradientPair`. Inherits from `AtomEvent&lt;GradientPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/GradientPair", fileName = "GradientPairEvent")]
    public sealed class GradientPairEvent : AtomEvent<GradientPair>
    {
    }
}
