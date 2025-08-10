using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using Framework.Utils;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : BaseCamera
{
    #region Class members
    [SerializeField] private Transform target;
    [SerializeField] Vector3 offset;
    #endregion

    #region MonoBehaviour events
    override protected void LateUpdate()
    {
        if (target)
        {
            cachedTransform.position = target.position + offset;
            cachedTransform.LookAt(target, Vector3.up);
        }

        base.LateUpdate();
    }
    #endregion

    #region Class Implementation
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    #endregion
}
