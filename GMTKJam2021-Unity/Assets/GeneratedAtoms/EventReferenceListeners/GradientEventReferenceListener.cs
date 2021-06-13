using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `Gradient`. Inherits from `AtomEventReferenceListener&lt;Gradient, GradientEvent, GradientEventReference, GradientUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/Gradient Event Reference Listener")]
    public sealed class GradientEventReferenceListener : AtomEventReferenceListener<
        Gradient,
        GradientEvent,
        GradientEventReference,
        GradientUnityEvent>
    { }
}
