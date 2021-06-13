#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `GradientPair`. Inherits from `AtomDrawer&lt;GradientPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GradientPairEvent))]
    public class GradientPairEventDrawer : AtomDrawer<GradientPairEvent> { }
}
#endif
