using System.Collections;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class FogCutout : MonoBehaviour
{

    #region Public Properties

    public float CutoutScale => m_cutoutScale;

    #endregion

    #region Private Methods

    private void OnEnable()
    {
        StartCoroutine(OnEnable_Routine());
    }

    private IEnumerator OnEnable_Routine()
    {
        while (FogOfWar.Instance == null)
        {
            yield return null;
        }

        FogOfWar.Instance.RegisterCutout(this);
    }

    private void OnDisable()
    {
        if (FogOfWar.Instance)
        {
            FogOfWar.Instance.UnregisterCutout(this);
        }
    }

    #endregion

    #region Private Fields

    [SerializeField]
    private FloatReference m_cutoutScale = new FloatReference(1f);

    #endregion

}