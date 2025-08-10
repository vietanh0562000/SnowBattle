using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;

public class Trails : OptimizedGameObject
{
    #region Class members

    private TrailRenderer[] trailRenderers;

    #endregion

    #region MonoBehaviour events

    private void Awake()
    {
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        cachedPoolManaged.onSpawn += OnSpawn;
    }

    #endregion

    #region Class implementation

    public void SetEmiting(bool emitting)
    {
        foreach (TrailRenderer trailRenderer in trailRenderers)
            trailRenderer.emitting = emitting;
    }

    #endregion

    #region PoolManaged delegates

    public void OnSpawn()
    {
        SetEmiting(true);
    }
    #endregion
}
