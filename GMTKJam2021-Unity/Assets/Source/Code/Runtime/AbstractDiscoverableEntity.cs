using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDiscoverableEntity<T> : MonoBehaviour
    where T : class
{

    #region Public Properties

    public static IReadOnlyList<T> Discovered => s_enabledEntities;

    #endregion

    #region Private Methods

    private void OnEnable()
    {
        s_enabledEntities.Add(this as T);
    }

    private void OnDisable()
    {
        s_enabledEntities.Remove(this as T);
    }

    #endregion

    #region Private Fields

    private static readonly List<T> s_enabledEntities = new List<T>();

    #endregion

}