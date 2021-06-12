#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `Gradient`. Inherits from `AtomDrawer&lt;GradientVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GradientVariable))]
    public class GradientVariableDrawer : VariableDrawer<GradientVariable> { }
}
#endif
