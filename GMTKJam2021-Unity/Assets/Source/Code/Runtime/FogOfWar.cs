using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FogOfWar : AbstractSingletonBehaviour<FogOfWar>
{

    #region Public Methods

    public void RegisterCutout(FogCutout cutout)
    {
        var instance = Instantiate(CutoutPrefab);
        m_instantiatedCutouts[cutout] = instance;
    }

    public void UnregisterCutout(FogCutout cutout)
    {
        if (m_instantiatedCutouts.TryGetValue(cutout, out var foundCutout))
        {
            m_instantiatedCutouts.Remove(cutout);
            DestroyImmediate(foundCutout);
        }
    }

    #endregion

    #region Protected Methods

    /// <inheritdoc />
    protected override void OnSingletonDisabled()
    {
        // Do nothing
    }

    /// <inheritdoc />
    protected override void OnSingletonEnabled()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        InitializeFog();
    }

    #endregion

    #region Private Methods

    private void InitializeFog()
    {
        var mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("There is no main camera in the scene");
            enabled = false;
            return;
        }

        if (m_smearCamera == null)
        {
            Debug.LogError($"{nameof(m_smearCamera)} is unassigned");
            enabled = false;
            return;
        }

        if (m_outputRenderer == null)
        {
            Debug.LogError($"{nameof(m_outputRenderer)} is unassigned");
            enabled = false;
            return;
        }

        // Zack: All dependencies are valid at this point
        m_mainCamera = mainCamera;

        // Zack: Cause a single render that will clear the renderTexture buffer
        var flags = m_smearCamera.clearFlags;
        m_smearCamera.clearFlags = CameraClearFlags.Color;
        m_smearCamera.Render();
        m_smearCamera.clearFlags = flags;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            var scaleFactor = transform.localScale.x * m_fogPlaneTransform.localScale.x;
            m_smearCamera.orthographicSize = scaleFactor / 2f;

            var yScale = m_smearCamera.farClipPlane / transform.localScale.x;
            yScale /= 2f;
            m_smearCamera.transform.localPosition = new Vector3(m_smearCamera.transform.localPosition.x, yScale, m_smearCamera.transform.localPosition.z);
            return;
        }

        FollowMainCamera();
        m_smearCamera.Render();

        foreach (var kvp in m_instantiatedCutouts)
        {
            kvp.Value.transform.localScale = Vector3.one * kvp.Key.CutoutScale;
            var cutoutSource = kvp.Key.transform.position;
            kvp.Value.transform.position = cutoutSource;
        }
    }

    private void FollowMainCamera()
    {
        var targetTransform = m_outputRenderer.transform;
        var sourceTransform = m_mainCamera.transform;
        targetTransform.position = sourceTransform.position;
        targetTransform.rotation = sourceTransform.rotation;
        targetTransform.localScale = sourceTransform.localScale;

        m_outputRenderer.depth = m_mainCamera.depth + 1;
        m_outputRenderer.nearClipPlane = m_mainCamera.nearClipPlane;
        m_outputRenderer.farClipPlane = m_mainCamera.farClipPlane;
        m_outputRenderer.orthographic = m_mainCamera.orthographic;
        m_outputRenderer.orthographicSize = m_mainCamera.orthographicSize;
    }

    #endregion

    #region Private Fields

    private readonly Dictionary<FogCutout, GameObject> m_instantiatedCutouts = new Dictionary<FogCutout, GameObject>();

    [SerializeField]
    private GameObject CutoutPrefab = null;

    [SerializeField]
    private Camera m_smearCamera = null;

    [SerializeField]
    private Camera m_outputRenderer = null;

    [SerializeField]
    private Transform m_fogPlaneTransform = null;

    private Camera m_mainCamera = null;

    #endregion

}