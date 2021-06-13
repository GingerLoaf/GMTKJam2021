using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Instancer of type `Gradient`. Inherits from `AtomEventInstancer&lt;Gradient, GradientEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Gradient Event Instancer")]
    public class GradientEventInstancer : AtomEventInstancer<Gradient, GradientEvent> { }
}
