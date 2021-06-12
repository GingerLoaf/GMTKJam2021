#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using UnityEngine;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `GradientPair`. Inherits from `AtomEventEditor&lt;GradientPair, GradientPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(GradientPairEvent))]
    public sealed class GradientPairEventEditor : AtomEventEditor<GradientPair, GradientPairEvent> { }
}
#endif
