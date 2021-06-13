using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `Gradient`. Inherits from `AtomEvent&lt;Gradient&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Gradient", fileName = "GradientEvent")]
    public sealed class GradientEvent : AtomEvent<Gradient>
    {
    }
}
