#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `Gradient`. Inherits from `AtomDrawer&lt;GradientValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GradientValueList))]
    public class GradientValueListDrawer : AtomDrawer<GradientValueList> { }
}
#endif
