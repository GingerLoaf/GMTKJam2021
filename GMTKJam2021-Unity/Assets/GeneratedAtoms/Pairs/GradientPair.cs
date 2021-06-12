using System;
using UnityEngine;
namespace UnityAtoms
{
    /// <summary>
    /// IPair of type `&lt;Gradient&gt;`. Inherits from `IPair&lt;Gradient&gt;`.
    /// </summary>
    [Serializable]
    public struct GradientPair : IPair<Gradient>
    {
        public Gradient Item1 { get => _item1; set => _item1 = value; }
        public Gradient Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private Gradient _item1;
        [SerializeField]
        private Gradient _item2;

        public void Deconstruct(out Gradient item1, out Gradient item2) { item1 = Item1; item2 = Item2; }
    }
}