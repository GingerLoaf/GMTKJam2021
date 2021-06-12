using UnityEditor;
using UnityAtoms.Editor;
using UnityEngine;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `Gradient`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(GradientVariable))]
    public sealed class GradientVariableEditor : AtomVariableEditor<Gradient, GradientPair> { }
}
