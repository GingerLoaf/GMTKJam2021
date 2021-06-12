using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `GradientPair`. Inherits from `AtomEventReferenceListener&lt;GradientPair, GradientPairEvent, GradientPairEventReference, GradientPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/GradientPair Event Reference Listener")]
    public sealed class GradientPairEventReferenceListener : AtomEventReferenceListener<
        GradientPair,
        GradientPairEvent,
        GradientPairEventReference,
        GradientPairUnityEvent>
    { }
}
