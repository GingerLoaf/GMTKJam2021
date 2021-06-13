using UnityEngine;

public abstract class AbstractSingletonBehaviour<T> : MonoBehaviour
    where T : class
{

    #region Public Properties

    public static T Instance { get; private set; }

    #endregion

    #region Protected Methods

    protected abstract void OnSingletonEnabled();

    protected abstract void OnSingletonDisabled();

    #endregion

    #region Private Methods

    private void OnEnable()
    {
        Instance = this as T;

        OnSingletonEnabled();
    }

    private void OnDisable()
    {
        Instance = null;

        OnSingletonDisabled();
    }

    #endregion

}