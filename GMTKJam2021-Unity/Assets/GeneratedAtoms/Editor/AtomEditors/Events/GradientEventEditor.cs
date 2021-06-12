#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using UnityEngine;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Gradient`. Inherits from `AtomEventEditor&lt;Gradient, GradientEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(GradientEvent))]
    public sealed class GradientEventEditor : AtomEventEditor<Gradient, GradientEvent> { }
}
#endif
