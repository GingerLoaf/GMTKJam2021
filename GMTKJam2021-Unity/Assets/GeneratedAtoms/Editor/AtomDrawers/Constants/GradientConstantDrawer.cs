#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `Gradient`. Inherits from `AtomDrawer&lt;GradientConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(GradientConstant))]
    public class GradientConstantDrawer : VariableDrawer<GradientConstant> { }
}
#endif
