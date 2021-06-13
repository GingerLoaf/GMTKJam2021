#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Gradient`. Inherits from `AtomDrawer&lt;GradientEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GradientEvent))]
    public class GradientEventDrawer : AtomDrawer<GradientEvent> { }
}
#endif
