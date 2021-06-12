using System;
using UnityEngine.Events;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// None generic Unity Event of type `Gradient`. Inherits from `UnityEvent&lt;Gradient&gt;`.
    /// </summary>
    [Serializable]
    public sealed class GradientUnityEvent : UnityEvent<Gradient> { }
}
